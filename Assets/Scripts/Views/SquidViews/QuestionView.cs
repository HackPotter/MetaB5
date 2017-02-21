using Squid;
using System;
using UnityEngine;

public class QuestionView : Frame, IQuestionView
{
    private Button _exitButton;

    private Label _feedbackQuestion;
    private Frame _feedbackFrame;
    private Button _feedbackBackButton;
    private Label _feedbackLabel;


    private TextArea _questionBody;
    private Frame _answerFrame;
    private Frame _supplementalImageFrame;
    private ImageControl _supplementalImageControl;

    public QuestionView(Desktop questionDesktop)
    {
        questionDesktop.Dock = DockStyle.Fill;
        Controls.Add(questionDesktop);

        _feedbackQuestion = (Label)GetControl("FeedbackQuestion");
        _feedbackFrame = (Frame)GetControl("Feedback");
        _feedbackLabel = (Label)GetControl("FeedbackLabel");
        _feedbackBackButton = (Button)GetControl("FeedbackBack");

        _exitButton = (Button)GetControl("ExitButton");
        _questionBody = (TextArea)GetControl("QuestionBody");
        _answerFrame = (Frame)GetControl("Answers");
        _supplementalImageFrame = (Frame)GetControl("SupplementalImageFrame");
        _supplementalImageControl = (ImageControl)GetControl("SupplementalImage");

        _supplementalImageFrame.Visible = false;

        _answerFrame.Controls.Clear();

        _exitButton.MouseClick += new MouseEvent(_exitButton_MouseClick);

        _feedbackBackButton.MouseClick += (s, a) =>
        {
            _feedbackFrame.Visible = false;
            _answerFrame.Visible = true;
        };
    }

    public event Action QuestionViewExited;

    public void Show(QuestionData question)
    {
        AnalyticsLogger.Instance.AddLogEntry(new QuestionViewedLogEntry(GameContext.Instance.Player.UserGuid, question));
        _feedbackFrame.Visible = false;
        _answerFrame.Visible = true;

        _questionAnswered = null;


        QuestionProgress progress = GameContext.Instance.Player.QuestionProgress.GetQuestionProgress(question);

        Visible = true;
        _questionBody.Text = question.QuestionText;

        if (question.QuestionImage)
        {
            ((UnityRenderer)GuiHost.Renderer).InsertTexture(question.QuestionImage, question.QuestionImage.name);
            _supplementalImageControl.Texture = question.QuestionImage.name;
            _supplementalImageControl.TextureRect = new Rectangle(0, 0, question.QuestionImage.width, question.QuestionImage.height);

            float scaleRatioX = ((float)question.QuestionImage.width) / ((float)_supplementalImageFrame.Size.x);
            float scaleRatioY = ((float)question.QuestionImage.height) / ((float)_supplementalImageFrame.Size.y);

            float scaleRatio = Mathf.Max(scaleRatioX, scaleRatioY);
            _supplementalImageControl.Size = new Point((int)(question.QuestionImage.width / scaleRatio), (int)(question.QuestionImage.height / scaleRatio));
            _supplementalImageFrame.Visible = true;
        }
        else
        {
            _supplementalImageFrame.Visible = false;
        }

        _answerFrame.Controls.Clear();
        PerformLayout();

        foreach (QuestionAnswer answer in question.QuestionAnswers)
        {
            Frame answerFrame = CreateAnswerFrame(progress, answer);
            _answerFrame.Controls.Add(answerFrame);
        }
        _answerFrame.PerformLayout();

        PerformLayout();
    }

    void _exitButton_MouseClick(Control sender, MouseEventArgs args)
    {
        Visible = false;
        if (QuestionViewExited != null)
        {
            QuestionViewExited();
        }
    }

    private event Action<QuestionAnswer> _questionAnswered;

    private Frame CreateAnswerFrame(QuestionProgress questionProgress, QuestionAnswer questionAnswer)
    {
        Button showFeedbackButton = new Button();
        showFeedbackButton.Dock = DockStyle.Fill;
        showFeedbackButton.Style = "Button - Rounded";
        showFeedbackButton.Text = "?";
        showFeedbackButton.UseTextColor = true;
        showFeedbackButton.Visible = false;

        Frame feedbackButtonFrame = new Frame();
        feedbackButtonFrame.AutoSize = Squid.AutoSize.Vertical;
        feedbackButtonFrame.Dock = DockStyle.Right;
        feedbackButtonFrame.Size = new Point(42, 31);
        feedbackButtonFrame.Controls.Add(showFeedbackButton);
        

        Button chooseAnswerButton = new Button();
        chooseAnswerButton.Dock = DockStyle.Fill;
        chooseAnswerButton.Style = "Button - Pointed";

        ImageControl answerIndicatorImage = new ImageControl();
        answerIndicatorImage.Dock = DockStyle.Center;
        answerIndicatorImage.Size = new Point(22, 22);
        answerIndicatorImage.Texture = "circle_frame";
        answerIndicatorImage.Visible = false;

        Frame answerIndicatorFrame = new Frame();
        answerIndicatorFrame.AutoSize = Squid.AutoSize.Vertical;
        answerIndicatorFrame.Dock = DockStyle.Left;
        answerIndicatorFrame.Margin = new Squid.Margin(12, 0, 0, 0);
        answerIndicatorFrame.Size = new Point(22, 26);

        answerIndicatorFrame.Controls.Add(answerIndicatorImage);

        Label questionTextLabel = new Label();
        questionTextLabel.Style = "Label - Bold";
        questionTextLabel.AutoSize = Squid.AutoSize.Vertical;
        questionTextLabel.Dock = DockStyle.Fill;
        questionTextLabel.UseTextColor = true;
        questionTextLabel.AutoEllipsis = false;
        questionTextLabel.TextWrap = true;
        questionTextLabel.NoEvents = true;

        questionTextLabel.Text = questionAnswer.QuestionAnswerText;

        Frame answerFrame = new Frame();
        answerFrame.AutoSize = Squid.AutoSize.Vertical;
        answerFrame.Dock = DockStyle.Top;
        answerFrame.Margin = new Squid.Margin(12, 0, 12, 3);

        answerFrame.Controls.Add(feedbackButtonFrame);
        answerFrame.Controls.Add(chooseAnswerButton);
        answerFrame.Controls.Add(answerIndicatorFrame);
        answerFrame.Controls.Add(questionTextLabel);

        _questionAnswered +=
            (chosenAnswer) =>
            {
                // When a question is answered, if it's the correct question or this question, then show the feedback button.
                if (chosenAnswer == questionProgress.CorrectAnswer || chosenAnswer == questionAnswer)
                {
                    // Show feedback frame because either the correct answer was chosen or this answer was chosen.
                    showFeedbackButton.Visible = true;
                    chooseAnswerButton.NoEvents = true;

                    // Change color 
                    if (questionProgress.CorrectAnswer == questionAnswer)
                    {
                        questionTextLabel.TextColor = ColorInt.ARGB(255, 136, 255, 137);
                        answerIndicatorImage.Visible = true;
                        answerIndicatorImage.Color = ColorInt.ARGB(255, 136, 255, 137);

                        GameContext.Instance.Player.Points += questionProgress.CalculatePoints();
                    }
                    else
                    {
                        questionTextLabel.TextColor = ColorInt.ARGB(255, 255, 86, 86);
                        if (questionProgress.AnswerChosen(questionAnswer))
                        {
                            answerIndicatorImage.Visible = true;
                            answerIndicatorImage.Color = ColorInt.ARGB(255, 255, 86, 86);
                        }
                    }
                }
            };

        if (questionProgress.AnswerChosen(questionAnswer) || questionProgress.Completed)
        {
            showFeedbackButton.Visible = true;
            chooseAnswerButton.NoEvents = true;
            if (questionAnswer == questionProgress.CorrectAnswer)
            {
                // answered correctly
                chooseAnswerButton.State = ControlState.Pressed;
                questionTextLabel.TextColor = ColorInt.ARGB(255, 136, 255, 137);
                answerIndicatorImage.Visible = true;
                answerIndicatorImage.Color = ColorInt.ARGB(255, 136, 255, 137);
            }
            else
            {
                // answered incorrectly
                questionTextLabel.TextColor = ColorInt.ARGB(255, 255, 86, 86);
                if (questionProgress.AnswerChosen(questionAnswer))
                {
                    chooseAnswerButton.State = ControlState.Pressed;
                    answerIndicatorImage.Visible = true;
                    answerIndicatorImage.Color = ColorInt.ARGB(255, 255, 86, 86);
                }
            }
        }


        showFeedbackButton.MouseClick +=
            (s, a) =>
            {
                _feedbackFrame.Visible = true;
                _answerFrame.Visible = false;
                _feedbackLabel.Text = questionAnswer.QuestionFeedback;
                _feedbackQuestion.Text = questionAnswer.QuestionAnswerText;
                _feedbackQuestion.TextColor = questionProgress.CorrectAnswer == questionAnswer ? ColorInt.ARGB(255, 136, 255, 137) : ColorInt.ARGB(255, 255, 86, 86);
            };

        chooseAnswerButton.MouseClick +=
            (s, a) =>
            {
                if (questionProgress.Completed || questionProgress.AnswerChosen(questionAnswer))
                {
                    return;
                }

                questionProgress.ChooseAnswer(questionAnswer);
                _questionAnswered(questionAnswer);
            };

        return answerFrame;
    }
}