using UnityEngine;
using System;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Xml;
using System.Diagnostics;

public class JsonToXmlConverter : MonoBehaviour {

    static string id = "s1";//NOT SAFE AT ALL but works for now... needs fixed
    static string password = "123123123";//NOT SAFE AT ALL but works for now... needs fixed
    static string clientID = "javascript";
    static string webUrl = "http://metablastweb.gdcb.iastate.edu/MetablastApp";
    private string accessToken = WebApiHelper.AccessToken(webUrl, id, password, clientID);
    private List<Topic> topics = new List<Topic>();


    public void ConvertToXml() {
        Stopwatch watch = new Stopwatch();
        watch.Start();
        LoadTopics();

        foreach (Topic topic in topics) {
            GetData(topic);
        }
        watch.Stop();
        TimeSpan ts = watch.Elapsed;

        string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
        UnityEngine.Debug.Log("Total elapsed time: " + elapsedTime);

        //Garbage Collect to help improve memory:
        Resources.UnloadUnusedAssets();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    } //end ConvertToXml


    private void LoadTopics() {
        var apiURL = "/api/v1/question/GetmblTopicList";
        WebRequest request = HttpWebRequest.Create(webUrl + apiURL);

        request.Method = "GET";
        request.ContentType = "application/json";
        request.Headers.Add("Authorization", "Bearer " + accessToken);
        request.ContentLength = 0;

        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            var qs = JArray.Parse(json);

            for (int i = 0; i < qs.Count; i++) {
                topics.Add(new Topic(qs[i]["Name"].ToString(), qs[i]["Description"].ToString()));
            }

        }
    }//end LoadTopics


    private void GetData(Topic topic) {
        Stopwatch watch = new Stopwatch();
        var apiURL = "/api/v1/question/GetmblTopicList";
        WebRequest request = HttpWebRequest.Create(webUrl + apiURL);

        apiURL = "/api/v1/question/GetmblContentListByTopic?topic=" + topic.Title;
        request = HttpWebRequest.Create(webUrl + apiURL);
        request.Method = "GET";
        request.ContentType = "application/json";
        request.Headers.Add("Authorization", "Bearer " + accessToken);
        request.ContentLength = 0;

        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            var qs = JArray.Parse(json);
            int len = qs.Count;

            if (len > 0) {
                CreateDirectory(topic.Title);
                LoadPictures(qs, topic.Title);
                CreateXml(qs, topic);
            }
            json = null;
            qs = null;

            //Garbage Collect to help improve memory:
            Resources.UnloadUnusedAssets();
            GC.WaitForPendingFinalizers();
            GC.Collect();

        }
    } //end GetData


    private void CreateDirectory(string topic) {
        try {
            string path = string.Format(@"Assets/Resources/Minigames/Millionaire/NEW{0}", topic);
            string imgPath = string.Format(@"Assets/Resources/Minigames/Millionaire/NEW{0}/Images/", topic);

            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
            }

            if (!Directory.Exists(imgPath)) {
                Directory.CreateDirectory(imgPath);
            }

        }
        catch (Exception ex) {
            UnityEngine.Debug.LogError("Error creating directory: " + topic.Trim() + "\n" + ex.Message);
        }

    }//end CreateDirectory


    private void CreateXml(JArray qs, Topic topic) {
        if (!File.Exists(string.Format(@"Assets/Resources/Minigames/Millionaire/NEW{0}/{0}Questions.xml", topic.Title))) {
            XmlDocument doc = new XmlDocument();

            //Creates the XML header: <?xml version="1.0" encoding="UTF-8"?>
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            //Creates the questions Node: <questions type="array">
            XmlNode questionNode = doc.CreateElement("questions");
            XmlAttribute questionAttribute = doc.CreateAttribute("type");
            questionAttribute.Value = "array";
            questionNode.Attributes.Append(questionAttribute);
            doc.AppendChild(questionNode);

            //Creates the title Node: <title>TOPIC</title>
            XmlNode titleNode = doc.CreateElement("title");
            titleNode.AppendChild(doc.CreateTextNode(topic.Title));
            questionNode.AppendChild(titleNode);

            //Creates the description Node: <description>EMPTY RIGHT NOW</description>
            XmlNode descritionNode = doc.CreateElement("description");
            descritionNode.AppendChild(doc.CreateTextNode(topic.Description));
            questionNode.AppendChild(descritionNode);

            //Creates the category Node: <category>UNUSED</category>
            XmlNode categoryNode = doc.CreateElement("category");
            categoryNode.AppendChild(doc.CreateTextNode("Has been unused since I started"));
            questionNode.AppendChild(categoryNode);

            //Creates the path Node: <path>PATH WHERE IMAGES ARE STORED</path>
            XmlNode imgPathNode = doc.CreateElement("path");
            imgPathNode.AppendChild(doc.CreateTextNode(string.Format(@"NEW{0}/Images/", topic.Title)));
            questionNode.AppendChild(imgPathNode);

            //Creates the previewImage Node: <previewImage>EMPTY RIGHT NOW</previewImage>
            XmlNode previewImageNode = doc.CreateElement("previewImage");
            previewImageNode.AppendChild(doc.CreateTextNode(""));
            questionNode.AppendChild(imgPathNode);

            //Creates The tags Node: <tags type="array" />
            XmlNode tagsNode = doc.CreateElement("tags");
            XmlAttribute tagsAttribute = doc.CreateAttribute("type");
            tagsAttribute.Value = "array";
            tagsNode.Attributes.Append(tagsAttribute);
            questionNode.AppendChild(tagsNode);

            int questionNumber = 0;
            for (int currentLevel = 1; currentLevel <= 15; currentLevel++) {
                int numOfQuestions = 0;

                for (int i = 0; i < qs.Count; i++) {
                    if (qs[i]["Level"].ToString().Equals(currentLevel.ToString())) {
                        numOfQuestions++;
                    }
                }

                if (numOfQuestions > 0) { // While there are questions do the following
                                          //Creates the level node: <level type="array" name="LEVELNUMBER">
                    XmlNode levelNode = doc.CreateElement("level");
                    XmlAttribute levelAttribute = doc.CreateAttribute("type");
                    levelAttribute.Value = "array";
                    levelNode.Attributes.Append(levelAttribute);
                    levelAttribute = doc.CreateAttribute("name");
                    levelAttribute.Value = currentLevel.ToString();
                    levelNode.Attributes.Append(levelAttribute);
                    questionNode.AppendChild(levelNode);

                    for (int currentQuestion = 0; currentQuestion < numOfQuestions; currentQuestion++) {
                        //Creates the question node: <question name="QUESTIONNUMBER">
                        XmlNode gameQuestionNode = doc.CreateElement("question");
                        XmlAttribute gameQuestionAttributes = doc.CreateAttribute("name");
                        gameQuestionAttributes.Value = (currentQuestion + 1).ToString();
                        gameQuestionNode.Attributes.Append(gameQuestionAttributes);
                        levelNode.AppendChild(gameQuestionNode);

                        //Creates the content node the contains each question: <content>QUESTION HERE</content>
                        XmlNode actualQuestionNode = doc.CreateElement("content");
                        actualQuestionNode.AppendChild(doc.CreateTextNode(qs[questionNumber]["Content"].ToString()));
                        gameQuestionNode.AppendChild(actualQuestionNode);

                        //Creates the image node: <image>IMAGEFILENAME</image>
                        XmlNode imageNode = doc.CreateElement("image");
                        imageNode.AppendChild(doc.CreateTextNode("question" + questionNumber.ToString()));
                        gameQuestionNode.AppendChild(imageNode);

                        string filename = string.Format(@"Assets/Resources/Minigames/Millionaire/NEW{0}/Images/question{1}.png", topic.Title, questionNumber.ToString());
                        FileInfo f = new FileInfo(filename);
                        if (f.Exists) {
                            ImageHeader.Vector2Int imgSize = ImageHeader.GetDimensions(filename);
                            UnityEngine.Debug.Log("Topic: " + topic.Title + "\nQuestionNumber: " + questionNumber + "\nimgSize.x =" + imgSize.x + "\nimgSize.y =" + imgSize.y);

                            XmlNode imgWidth = doc.CreateElement("imgWidth");
                            XmlNode imgHeight = doc.CreateElement("imgHeight");
                            if (imgSize.x == 0 || imgSize.y == 0) {
                                imgWidth.AppendChild(doc.CreateTextNode("0"));
                                gameQuestionNode.AppendChild(imgWidth);

                                imgHeight.AppendChild(doc.CreateTextNode("0"));
                                gameQuestionNode.AppendChild(imgHeight);
                            }
                            else {

                                imgWidth.AppendChild(doc.CreateTextNode(XmlConvert.ToString(imgSize.x)));
                                gameQuestionNode.AppendChild(imgWidth);

                                imgHeight.AppendChild(doc.CreateTextNode(XmlConvert.ToString(imgSize.y)));
                                gameQuestionNode.AppendChild(imgHeight);
                            }
                        }
                        else {
                            XmlNode imgWidth = doc.CreateElement("imgWidth");
                            XmlNode imgHeight = doc.CreateElement("imgHeight");
                            imgWidth.AppendChild(doc.CreateTextNode("0"));
                            gameQuestionNode.AppendChild(imgWidth);

                            imgHeight.AppendChild(doc.CreateTextNode("0"));
                            gameQuestionNode.AppendChild(imgHeight);
                        }

                        //Creates the answers node: <answers type="array">
                        XmlNode answersNode = doc.CreateElement("answers");
                        XmlAttribute answersAttrubutes = doc.CreateAttribute("type");
                        answersAttrubutes.Value = "array";
                        answersNode.Attributes.Append(answersAttrubutes);
                        gameQuestionNode.AppendChild(answersNode);

                        string[] answers = qs[questionNumber]["Answers"].ToObject<string[]>();

                        for (int i = 0; i < 4; i++) {
                            XmlNode answerNode = doc.CreateElement("answer");
                            answersNode.AppendChild(answerNode);

                            //Creates the correct node which says whether the current answer is the correct answer or not: <correct>TRUE or FALSE</correct>
                            XmlNode correctNode = doc.CreateElement("correct");
                            correctNode.AppendChild(doc.CreateTextNode((int)qs[questionNumber]["CorrectAnswerIndex"] == i ? "true" : "false"));
                            answerNode.AppendChild(correctNode);

                            //Creates the answer node which contains the answer: <answer>ANSWERDATA</answer>
                            XmlNode answerContentNode = doc.CreateElement("content");
                            answerContentNode.AppendChild(doc.CreateTextNode(answers[i]));
                            answerNode.AppendChild(answerContentNode);
                        }
                        questionNumber++;
                    }//end for
                }//end if
            }//end for

            //Saves the data after it has all been retrieved
            doc.Save(string.Format(@"Assets/Resources/Minigames/Millionaire/NEW{0}/{0}Questions.xml", topic.Title));

            doc = null;

            //Garbage Collect to help improve memory:
            Resources.UnloadUnusedAssets();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    } //end CreateXML


    private void LoadPictures(JArray qs, string topic) {
        //Get image data from online, convert it to a .png file, and save it in the correct location
        //Very memory heavy, try to improve memory usage.
        int imageSize = -1;
        string byteImage = string.Empty;

        try {

            for (int i = 0; i < qs.Count; i++) {
                if (!qs[i]["Image"].ToString().Equals(string.Empty)) { // if the image data is NOT an empty string, do the following...
                    byteImage = qs[i]["Image"].ToString();
                    var img = Convert.FromBase64String(qs[i]["Image"].ToString().Replace("data:image/jpeg;base64,", "").Replace("data:image/tiff;base64,", "").Replace("data:image/png;base64,", ""));
                    imageSize = img.Length;

                    if (img.Length > 0) {
                        var filename = string.Format(@"Assets/Resources/Minigames/Millionaire/NEW{0}/Images/question{1}.png", topic, i.ToString());
                        using (var imageFile = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None)) {

                            imageFile.Write(img, 0, img.Length);
                            imageFile.Flush();
                            imageFile.Close();
                            imageFile.Dispose();
                        }
                    }
                    else {
                        UnityEngine.Debug.LogError(topic + "\nError creating images (Length):" + img.Length.ToString());
                    }
                    img = null;
                }
            }
        }
        catch (Exception ex) {
            UnityEngine.Debug.LogError(topic + "\nError creating images:" + ex.Message + " : byteImage: " + byteImage);
        }

        byteImage = null;

        //Garbage Collect to help improve memory:
        Resources.UnloadUnusedAssets();
        GC.WaitForPendingFinalizers();
        GC.Collect();
    }


} //end WebJsonToXmlConverter


class Topic {
    private string title = null;
    private string description = null;

    public string Title { get; set; }

    public string Description { get; set; }

    public Topic(string title, string description) {
        Title = title;
        Description = description;
    }
}
