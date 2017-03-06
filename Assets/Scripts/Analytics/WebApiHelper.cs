using System;
using System.IO;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;

public class WebApiHelper {
    public static string AccessToken(string baseUrl, string username, string password, string client_id) {
        SHA256Managed hash = new SHA256Managed();
        byte[] correctHash = hash.ComputeHash(Encoding.UTF8.GetBytes(password));

        string hexPassword = BitConverter.ToString(correctHash).Replace("-", "").ToLower();

        var request = (HttpWebRequest)WebRequest.Create(new Uri(baseUrl + "/Token"));
        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";

        var str = "grant_type=password"
                + "&client_id=" + client_id
                + "&userName=" + username
                + "&password=" + hexPassword;

        var byteArray = Encoding.UTF8.GetBytes(str);
        request.ContentLength = byteArray.Length;
        using (var dataStream = request.GetRequestStream()) { dataStream.Write(byteArray, 0, byteArray.Length); }

        var resp = (HttpWebResponse)request.GetResponse();
        var reader = new StreamReader(resp.GetResponseStream());
        string token = reader.ReadToEnd();

        var qs = JObject.Parse(token);
        var accessToken = qs["access_token"].ToString();

        return accessToken;
    }

    public static string WebInfo(string baseUrl, string accessToken, string targetFunc, string value = null) {
        var apiUrl = "/api/v1/"+ targetFunc + value;

        WebRequest request  = HttpWebRequest.Create(baseUrl + apiUrl);
        request.Method      = "GET";
        request.ContentType = "application/json";
        request.Headers.Add("Authorization", "Bearer " + accessToken);
        request.ContentLength = 0;
        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse) {
            StreamReader reader = new StreamReader(response.GetResponseStream());
            return reader.ReadToEnd();
        }
    }
}
