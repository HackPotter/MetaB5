using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;
using System;

/// <summary>
/// WebOperation is the entry point for performing any HTTP operation. The operation to perform is specified using a WebOperationCriteria object.
/// 
/// TODO: It might be better and more consistent to return a WebOperationResult object instead of a WWW directly.
/// 
/// For both consistency as well as to prevent the need to heavily process the result of the operation by callers.
/// 
/// This stuff should really all be behind an interface, too, so we can insert proxies for debugging and logging.
/// </summary>
public sealed class WebOperation
{
    /// <summary>
    /// Executes a WebOperation with the given criteria.
    /// </summary>
    /// <param name="criteria">Criteria that specifies how to invoke the operation.</param>
    /// <returns>A WWW object containing the results of the operation.</returns>
    public static ResultSet Execute(WebOperationCriteria criteria)
    {
        Asserter.NotNull(criteria, "WebOperation.Execute:criteria is null");

        if (_webOperation == null)
        {
            _webOperation = new WebOperation();
        }

        return _webOperation.ExecuteOperation(criteria);
    }

    /// <summary>
    /// Executes a WebOperation with the given criteria asynchronously. Upon completion, the given WebOperationCallback is invoked with the results of the operation.
    /// </summary>
    /// <param name="criteria">Criteria that specifies how to invoke the operation.</param>
    /// <param name="callback">The callback to be invoked upon completion of the operation.</param>
    public static void ExecuteAsync(WebOperationCriteria criteria, WebOperationCallback webOperationCallback)
    {
        Asserter.NotNull(criteria, "WebOperation.ExecuteAsync:criteria is null");
        Asserter.NotNull(webOperationCallback, "WebOpreation.ExecuteAsync:webOperationCallback");

        if (_webOperation == null)
        {
            _webOperation = new WebOperation();
        }

        _webOperation.ExecuteOperationAsync(criteria, webOperationCallback);
    }

    private static WebOperation _webOperation;

    // CoroutineRunner is an empty MonoBehaviour.
    // We need to be able to start Coroutines, but only monobehaviours can do that.
    // We create a junk GameObject for the sole purpose of executing coroutines for asynchronous functionality.
    private CoroutineRunner _coroutineRunner;

    /// <summary>
    /// Constructs a new WebOperation object.
    /// </summary>
    private WebOperation()
    {
        GameObject coroutineRunnerObject = new GameObject("WebOperation.CoroutineRunner");
        _coroutineRunner = coroutineRunnerObject.AddComponent<CoroutineRunner>();
    }

    /// <summary>
    /// Executes the web operation from the given WebOperationCriteria.
    /// </summary>
    /// <param name="criteria">The criteria containing the data that governs the action of this operation.</param>
    /// <returns></returns>
    private ResultSet ExecuteOperation(WebOperationCriteria criteria)
    {
        WWWForm form = new WWWForm();

        foreach (KeyValuePair<string, string> pair in criteria.Fields)
        {
            form.AddField(pair.Key, pair.Value);
        }

        WWW www = new WWW(criteria.URL, form);
        while (!www.isDone) { }
        return ParseResponse(www);
    }

    /// <summary>
    /// Executes the web operation asynchronously from the given WebOperationCriteria, and invokes the given WebOperationCallback upon completion.
    /// </summary>
    /// <param name="criteria">The criteria containing the data that governs the action of this operation.</param>
    /// <param name="callback">The callback to be invoked upon completion of the operation.</param>
    private void ExecuteOperationAsync(WebOperationCriteria criteria, WebOperationCallback callback)
    {
        WWWForm form = new WWWForm();

        foreach (KeyValuePair<string, string> pair in criteria.Fields)
        {
            form.AddField(pair.Key, pair.Value);
        }

        WWW www = new WWW(criteria.URL, form);
        _coroutineRunner.StartCoroutine(WaitForWWW(www, callback));
    }

    /// <summary>
    /// Coroutine function to yield control flow back to Unity while waiting for the WWW operation to complete. Upon completion,
    /// the WebOperationCallback is invoked.
    /// </summary>
    /// <param name="request">The WWW to yield for while waiting for completion.</param>
    /// <param name="callback">The callback to be invoked upon completion of the operation.</param>
    /// <returns></returns>
    private static IEnumerator WaitForWWW(WWW request, WebOperationCallback callback)
    {
        yield return request;
        callback(ParseResponse(request));
    }

    private static ResultSet ParseResponse(WWW response)
    {
        ResultSet.Builder builder = ResultSet.Builder.Create();
        if (!string.IsNullOrEmpty(response.error))
        {
            return builder.Status(ResultSetStatus.ConnectionError).ConnectionErrorMessage(response.error).Build();
        }

        // TODO need to rewrite this parser: broken for GetGroups

        //ResultSet resultSet;

        //ResultSetArray array = resultSet.GetArray("game-user-groups");

        //array.GetStructure("

        // Unable to parse xml like following:

        //<?xml version="1.0" encoding="UTF-8"?>
        //<game-user-groups type="array">
        //  <game-user-group>
        //    <id type="integer">1298</id>
        //    <game-user-id type="integer">1225</game-user-id>
        //    <group-id type="integer">10</group-id>
        //    <created-at type="datetime">2013-08-03T03:30:40Z</created-at>
        //    <updated-at type="datetime">2013-08-03T03:30:40Z</updated-at>
        //    <state>accepted</state>
        //    <game-role-id type="integer">2</game-role-id>
        //    <inviter-id type="integer" nil="true"/>
        //    <group>
        //      <id type="integer">10</id>
        //      <name>Meta!Blast</name>
        //      <owner-id type="integer">18</owner-id>
        //      <deleted-at type="datetime" nil="true"/>
        //      <created-at type="datetime">2011-02-28T11:41:09Z</created-at>
        //      <updated-at type="datetime">2011-02-28T11:41:10Z</updated-at>
        //      <collections type="array">
        //        <collection>
        //          <id type="integer">11</id>
        //          <description>Meta!Blast Basic</description>
        //          <group-id type="integer">10</group-id>
        //          <creator-id type="integer">18</creator-id>
        //          <created-at type="datetime">2011-09-13T19:32:53Z</created-at>
        //          <updated-at type="datetime">2012-09-29T04:49:24Z</updated-at>
        //          <name>Meta!Blast Basic</name>
        //        </collection>
        //        <collection>
        //          <id type="integer">210</id>
        //          <description>Meta!Blast Best Bio Questions</description>
        //          <group-id type="integer">10</group-id>
        //          <creator-id type="integer">440</creator-id>
        //          <created-at type="datetime" nil="true"/>
        //          <updated-at type="datetime" nil="true"/>
        //          <name>Meta!Blast Best Bio Questions</name>
        //        </collection>
        //      </collections>
        //    </group>
        //  </game-user-group>
        //</game-user-groups>

        // Need new ResultSet type: Structure
        // Need new ResultSet type: Array
        //  the <group> tag will be parsed into a structure.
        //  The group structure will contain:
        //      int id
        //      string name
        //      int owner-id
        //      datetime deleted-at,created-at,updated-at
        //      array collections

        // where collections is an array of structures, each element with:
        //  int id
        //  string description
        //  int group-id
        //  int creator-id
        //  datetime created-at
        //  datetime updated-at
        //  string name

        Debug.Log(response.text);

        XmlReader reader = XmlReader.Create(new StringReader(response.text));

        // XmlDeclaration
        reader.Read();
        // Whitespace
        reader.Read();

        //Table-name
        reader.Read();

        string tableName = reader.Name;
        if (tableName == "error")
        {
            reader.Close();
            return builder.Status(ResultSetStatus.InvalidFormError).InvalidFormErrorDetails(ResultSetErrorDetails.CreateFromXML(response.text)).Build();
        }

        builder.TableName(reader.Name);

        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case (XmlNodeType.Element):
                    string name = reader.Name;
                    string typeAttribute = reader.GetAttribute("type");
                    string nilAttribute = reader.GetAttribute("nil");

                    bool isNull = false;

                    if (nilAttribute != null)
                    {
                        isNull = bool.Parse(nilAttribute);
                    }

                    object content = null;

                    if (!isNull)
                    {
                        switch (typeAttribute)
                        {
                            case "integer":
                                content = reader.ReadElementContentAsInt();
                                break;
                            case "datetime":
                                string dateTimeString = reader.ReadElementContentAsString();
                                content = DateTime.Parse(dateTimeString);
                                break;
                            case "array":
                                // TODO handle this:
                                // if an array is encountered,
                                //      Must create child ResultSet sort of object that has accessors for fields of whatever object is in the array.
                                //      
                                break;
                            default:
                                content = reader.ReadElementContentAsString();
                                break;
                        }
                    }

                    builder.Field(name, content);
                    break;
                default:
                    break;
            }
        }

        return builder.Build();
    }
}

