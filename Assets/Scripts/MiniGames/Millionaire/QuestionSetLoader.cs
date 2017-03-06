using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Xml;

public class QuestionSetLoader : MonoBehaviour {
    public const int kMaxLevel = 15;

#pragma warning disable 0067, 0649
    [SerializeField]
    private string _gameDataResourcePath;
    [SerializeField]
    private List<TextAsset> _questionSetData;
#pragma warning restore 0067, 0649

    private QuestionSet[] _questionSets;

    public QuestionSet[] QuestionSets {
        get {
            if (_questionSets == null) {
                ReadXml();
                Debug.Log("Done");
            }
            return _questionSets;
        }
    }

    private void ReadXml() {
        _questionSets = new QuestionSet[_questionSetData.Count];

        try {
            // Read each Question Set.
            for (int i = 0; i < _questionSetData.Count; i++) {
                TextAsset textAsset = _questionSetData[i];
                XmlDocument xmlDoc = new XmlDocument();

                // Need to get all files in this folder.
                string data = textAsset.text;
                try {
                    xmlDoc.LoadXml(data);
                }
                catch (Exception e) {
                    Debug.LogError(string.Format("Failed to parse xml file {0}: {1}", textAsset.name, e.ToString()), this);
                }

                QuestionSet questionSet;
                if (xmlDoc == null) {
                    Debug.Log("Test");
                    continue;
                }

                int level_i = 0;

                string title = xmlDoc.SelectSingleNode("questions/title").InnerText;

                var descriptionNode = xmlDoc.SelectSingleNode("questions/description");
                string description = descriptionNode != null ? descriptionNode.InnerText : "";

                var categoryNode = xmlDoc.SelectSingleNode("questions/category");
                string category = categoryNode != null ? categoryNode.InnerText : "";

                var pathNode = (XmlElement)xmlDoc.SelectSingleNode("questions/path");
                string imagePath = pathNode.InnerText;

                var imageNode = xmlDoc.SelectSingleNode("questions/previewImage");
                string previewImagePath = (imageNode != null ? (_gameDataResourcePath + "/" + imagePath + imageNode.InnerText) : "");


                questionSet = new QuestionSet(title, previewImagePath, description, category, GetLevels(xmlDoc));
                _questionSets[i] = questionSet;

                foreach (XmlElement level_node in xmlDoc.SelectNodes("questions/level")) {
                    int question_i = 0;
                    if (questionSet.Levels[level_i] == null) {
                        Debug.LogError("QuestionSet levels[level_i] is null " + level_i.ToString());
                    }
                    Level temp_level = questionSet.Levels[level_i];


                    foreach (XmlElement question_node in level_node.SelectNodes("question")) {
                        int answer_i = 0;

                        Question temp_question = temp_level.questions[question_i];

                        temp_question.content = question_node.SelectSingleNode("content").InnerText;
                        if (!imagePath.Equals(string.Empty)) {
                            temp_question.imagePath = _gameDataResourcePath + "/" + imagePath + (question_node.SelectSingleNode("image") ?? level_node.SelectSingleNode("image")).InnerText;
                            try {
                                if (question_node.SelectSingleNode("imgHeight").InnerText != string.Empty && question_node.SelectSingleNode("imgWidth").InnerText != string.Empty) {
                                    temp_question.imgHeight = Convert.ToDouble(question_node.SelectSingleNode("imgHeight").InnerText);
                                    temp_question.imgWidth = Convert.ToDouble(question_node.SelectSingleNode("imgWidth").InnerText);
                                }
                                else {
                                    temp_question.imgHeight = 0;
                                    temp_question.imgWidth = 0;
                                }
                            }
                            catch (Exception ex) {
                                Debug.LogError(ex.Message);
                            }
                        }
                        foreach (XmlElement answer_node in question_node.SelectNodes("answers/answer")) {
                            Answer temp_answer = temp_question.answers[answer_i];

                            temp_answer.content = answer_node.SelectSingleNode("content").InnerText;
                            temp_answer.correct = bool.Parse(answer_node.SelectSingleNode("correct").InnerText);

                            answer_i++;
                        }
                        //DEBUGGING ONLY
                        //Display if current question doesn't have a correct answer.
                        if (temp_question.answers.Where(x => x.correct == true).Count() != 1) {
                            Debug.LogError(string.Format("{0}: Level {1}: Question {2} has less or more than one correct answers", textAsset.name, (level_i + 1), (question_i + 1)));
                        }
                        question_i++;
                    }
                    level_i++;
                }
            }
        }
        catch (Exception ex) {
            Debug.LogError(ex.Message);
        }
    }

    private Level[] GetLevels(XmlDocument xmlDoc) {
        Level[] levels = null;

        try {
            levels = new Level[kMaxLevel];
            for (int i = 0; i < kMaxLevel; i++) {
                // Read path next to title.

                XmlNode node = (XmlNode)xmlDoc.SelectNodes("questions/level")[i];
                if (node == null) {
                    continue;
                }
                else {
                    levels[i] = new Level(node.SelectNodes("question").Count);

                    for (int j = 0; j < levels[i].questions.Length; j++) {

                        levels[i].questions[j] = new Question();

                        for (int k = 0; k < 4; k++) {
                            levels[i].questions[j].answers[k] = new Answer();
                        }
                    }
                }
            }
        }
        catch (Exception ex) {
            Debug.LogError("QuestionSetLoader\\GetLevels(): " + ex.Message);
        }
        return levels;
    }

}

