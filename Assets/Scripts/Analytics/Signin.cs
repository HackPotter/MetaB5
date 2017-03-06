#pragma warning disable 0414, 0219
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json;

public class Signin : MonoBehaviour {
    public const int kMaxLevel = 15;

    public GameObject error;
    public GameObject menu;
    public InputField id;
    public InputField pw;

    private static string _websiteUrl = "http://metablastweb.gdcb.iastate.edu/MetablastApp"; 
    private static string client_id = "javascript";
    private string accessToken = WebApiHelper.AccessToken(_websiteUrl, "s1", "123123123", client_id);

    private List<string> topics = new List<string>();

    public void TrySignin() {
        LoadTopics();
        foreach (string topic in topics) {
            StartCoroutine(Post(topic));
        }
    }


    private void LoadTopics() {
        /**
         * Replaces all instances of the following: ]  [  \n  "
         * Splits the data retrieved from https://.../question/GetTopicList
         * and adds it to List<string> topics
         * 
         * probably could use a Deserializer or something to split this all up but this currently works fine right now...
         *
         */
        foreach (string x in WebApiHelper.WebInfo(_websiteUrl, accessToken, "question/GetTopicList").Replace("[", "").Replace("]", "").Replace("\n", "").Replace("\"", "").Split(',')) {
            topics.Add(x.Remove(0, 3));
        }
    } //end LoadTopics
    IEnumerator Post(string topic) {
        var apiURL = "/api/v1/question/GetmblTopicList";
        WebRequest request = HttpWebRequest.Create(_websiteUrl + apiURL);

        apiURL = "/api/v1/question/GetmblContentListByTopic?topic=" + topic;

        request = HttpWebRequest.Create(_websiteUrl + apiURL);
        request.Method = "GET";
        request.ContentType = "application/json";
        request.Headers.Add("Authorization", "Bearer " + accessToken);

        request.ContentLength = 0;
        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string json = reader.ReadToEnd();
            var qs = JArray.Parse(json);
            int len = qs.Count;

            //Debug.Log(json.Replace("[","{").Replace("]","}").ToString()
            var test = Convert.FromBase64String(qs[0]["Image"].ToString().Replace("data:image/jpeg;base64,", ""));
            using (var imageFile = new FileStream(string.Format(@"C:\Users\bradr555\Desktop\{0}.png",topic), FileMode.Create)) {
                imageFile.Write(test, 0, test.Length);
                imageFile.Flush();
            }
        }

        yield return new WaitForSeconds(3);
    }

}



//Debug.Log("TOTAL: " + total);
//int test1 = 0;
//foreach (string answer in qs[1]["Answers"]) {
//    Debug.Log(answer + ((int)qs[1]["CorrectAnswerIndex"] == test1 ? "correct" : ""));
//    test1++;
//}
//Debug.Log(json);
//Debug.Log(i);
//string[] ans = qs[i]["Answers"].ToObject<string[]>();
//string correctAnswerIndex1 = qs[0]["CorrectAnswerIndex"].ToString();

////string[] answers2 = qs[1]["Answers"].ToObject<string[]>();
////string correctAnswerIndex2 = qs[1]["correctAnswerIndex"].ToString();

//Debug.Log("ID: " + qs[i]["_id"].ToString());
//Debug.Log("Level: " + qs[i]["Level"]);
//Debug.Log("Content: " + qs[i]["Content"].ToString());
//Debug.Log("Answers: " + qs[i]["Answers"].ToString());
//Debug.Log("Correct answer Index: " + qs[i]["CorrectAnswerIndex"].ToString());
//Debug.Log("ANSWER: " + ans.GetValue(qs[i]["CorrectAnswerIndex"].ToObject<int>()));
//Debug.Log("---------------------------------------------");  