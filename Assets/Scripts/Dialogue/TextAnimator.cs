using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class TextAnimator : MonoBehaviour
{
    private float _startTime;
    private bool _textAnimationRunning = false;

    public string Text
    {
        get;
        private set;
    }

    public bool AnimationFinished
    {
        get { return !_textAnimationRunning; }
    }

    private enum TextTokenType
    {
        Text,
        Bold,
        Italic,

        Color,
        ColorHex,
        Size,
        Speed,
        Pause,

        EndBold,
        EndItalic,
        EndColor,
        EndSize,
        EndSpeed
    }

    private class TextToken
    {
        public TextTokenType TokenType;
        public string Text;
        public float Size;
        public string ColorString;
        public float DisplaySpeed;
        public float PauseDuration;
    }

    private void OnTextAnimationFinished()
    {
    }

    public void ShowText(string text, Action<string> textCallback, Action finishedCallback)
    {
        if (!_textAnimationRunning)
        {
            StartCoroutine(ShowTextCoroutine(ParseText(text), 20, textCallback, finishedCallback));
        }
    }

    private IEnumerator ShowTextCoroutine(List<TextToken> tokens, float defaultDisplaySpeed, Action<string> callback, Action finishedCallback)
    {
        _textAnimationRunning = true;
        StringBuilder stringBuilder = new StringBuilder(100);
        float time = 0;

        int characterCount = 0;
        int maxCharacters = 0;
        Stack<float> displayRateStack = new Stack<float>();
        Stack<string> endTagStack = new Stack<string>();
        float displayRate = defaultDisplaySpeed;

        int i = 0;
        int currentTextStartingIndex = 0;
        yield return new WaitForSeconds(0.2f);
        while (true)
        {
            time += Time.deltaTime * displayRate;

            int newMaxCharacters = (int)(time);
            if (newMaxCharacters == maxCharacters)
            {
                yield return null;
                continue;
            }
            maxCharacters = newMaxCharacters;

            for (; i < tokens.Count && (characterCount != maxCharacters); i++)
            {
                bool breakLoop = false;
                switch (tokens[i].TokenType)
                {
                    case TextTokenType.Bold:
                        stringBuilder.Append("<b>");
                        endTagStack.Push("</b>");
                        break;
                    case TextTokenType.EndBold:
                        endTagStack.Pop();
                        stringBuilder.Append("</b>");
                        break;
                    case TextTokenType.Italic:
                        stringBuilder.Append("<i>");
                        endTagStack.Push("</i>");
                        break;
                    case TextTokenType.EndItalic:
                        endTagStack.Pop();
                        stringBuilder.Append("</i>");
                        break;
                    case TextTokenType.Color:
                        stringBuilder.Append("<color=" + tokens[i].ColorString + ">");
                        endTagStack.Push("</color>");
                        break;
                    case TextTokenType.ColorHex:
                        stringBuilder.Append("<color=#" + tokens[i].ColorString + ">");
                        endTagStack.Push("</color>");
                        break;
                    case TextTokenType.EndColor:
                        endTagStack.Pop();
                        stringBuilder.Append("</color>");
                        break;
                    case TextTokenType.Speed:
                        displayRateStack.Push(displayRate);
                        displayRate = tokens[i].DisplaySpeed;
                        break;
                    case TextTokenType.EndSpeed:
                        displayRate = displayRateStack.Pop();
                        break;
                    case TextTokenType.Size:
                        stringBuilder.Append("<size=" + tokens[i].Size + ">");
                        endTagStack.Push("</size>");
                        break;
                    case TextTokenType.EndSize:
                        endTagStack.Pop();
                        stringBuilder.Append("</size>");
                        break;
                    case TextTokenType.Pause:
                        yield return new WaitForSeconds(tokens[i].PauseDuration);
                        i++;
                        breakLoop = true;
                        break;
                    case TextTokenType.Text:
                        string text = tokens[i].Text;
                        // Number of characters we still need to add to match target number of characters.
                        int charactersToAdd = maxCharacters - characterCount;

                        // If our text length minus the index of the last character we added is GREATER than the maximum we can add, then we'll have to try again next time.

                        // We're done with this string.
                        if (text.Length - currentTextStartingIndex <= charactersToAdd)
                        {
                            // Add the rest of the characters
                            int substringLength = text.Length - currentTextStartingIndex;
                            stringBuilder.Append(text.Substring(currentTextStartingIndex, substringLength));
                            characterCount += substringLength;
                            currentTextStartingIndex = 0;
                        }
                        else
                        {
                            stringBuilder.Append(text.Substring(currentTextStartingIndex, charactersToAdd));
                            currentTextStartingIndex += charactersToAdd;
                            characterCount += charactersToAdd;
                            breakLoop = true;
                        }
                        break;
                }
                if (breakLoop)
                    break;
            }

            if (i == tokens.Count)
            {
                _textAnimationRunning = false;
                Text = stringBuilder.ToString();
                callback(Text);
                OnTextAnimationFinished();
                finishedCallback();
                yield break;
            }
            else
            {
                string unclosedDialogueText = stringBuilder.ToString();
                StringBuilder closingStringBuilder = new StringBuilder(unclosedDialogueText);
                foreach (var endTag in endTagStack)
                {
                    closingStringBuilder.Append(endTag);
                }

                Text = stringBuilder.ToString();
                callback(Text);
            }
        }
    }

    private List<TextToken> ParseText(string text)
    {
        string[] split = text.Split('<', '>');
        List<TextToken> tokens = new List<TextToken>();
        for (int i = 0; i < split.Length; i++)
        {
            string lowerCaseToken = split[i].ToLower();
            if (lowerCaseToken.StartsWith("color=#"))
            {
                string colorToken = lowerCaseToken.Replace("color=#", "");

                tokens.Add(new TextToken()
                {
                    TokenType = TextTokenType.ColorHex,
                    ColorString = colorToken
                });
                continue;
            }

            if (lowerCaseToken.StartsWith("color="))
            {
                string colorToken = lowerCaseToken.Replace("color=", "");
                tokens.Add(new TextToken()
                {
                    TokenType = TextTokenType.Color,
                    ColorString = colorToken
                });
                continue;
            }

            if (lowerCaseToken == "b")
            {
                tokens.Add(new TextToken()
                {
                    TokenType = TextTokenType.Bold
                });
                continue;
            }

            if (lowerCaseToken == "i")
            {
                tokens.Add(new TextToken()
                {
                    TokenType = TextTokenType.Italic
                });
                continue;
            }

            if (lowerCaseToken.StartsWith("size="))
            {
                string sizeValue = lowerCaseToken.Replace("size=", "");
                float size;
                if (float.TryParse(sizeValue, out size))
                {
                    tokens.Add(new TextToken()
                    {
                        TokenType = TextTokenType.Size,
                        Size = size
                    });
                    continue;
                }
            }

            if (lowerCaseToken.StartsWith("speed="))
            {
                string speedToken = lowerCaseToken.Replace("speed=", "");
                float speed;
                if (float.TryParse(speedToken, out speed))
                {
                    tokens.Add(new TextToken()
                    {
                        TokenType = TextTokenType.Speed,
                        DisplaySpeed = speed
                    });
                    continue;
                }
            }

            if (lowerCaseToken.StartsWith("pause="))
            {
                string pauseToken = lowerCaseToken.Replace("pause=", "");
                float pause;
                if (float.TryParse(pauseToken, out pause))
                {
                    tokens.Add(new TextToken()
                    {
                        TokenType = TextTokenType.Pause,
                        PauseDuration = pause
                    });
                    continue;
                }
            }

            if (lowerCaseToken.StartsWith("/"))
            {
                string endToken = lowerCaseToken.Replace("/", "");
                TextTokenType endTokenType;
                switch (endToken)
                {
                    case "b":
                        endTokenType = TextTokenType.EndBold;
                        break;
                    case "i":
                        endTokenType = TextTokenType.EndItalic;
                        break;
                    case "color":
                        endTokenType = TextTokenType.EndColor;
                        break;
                    case "speed":
                        endTokenType = TextTokenType.EndSpeed;
                        break;
                    case "size":
                        endTokenType = TextTokenType.EndSize;
                        break;
                    default:
                        continue;
                }
                tokens.Add(new TextToken()
                {
                    TokenType = endTokenType
                });
                continue;
            }

            tokens.Add(new TextToken()
            {
                TokenType = TextTokenType.Text,
                Text = split[i]
            });
        }

        return tokens;
    }
}
