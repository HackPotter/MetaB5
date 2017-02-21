using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class GlobalSymbolTable2 : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField]
    private string _jsonSerializedVariables;


    private List<Variable2> _variables = new List<Variable2>();


    public void OnAfterDeserialize()
    {
        Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();

        _variables = serializer.Deserialize<List<Variable2>>(new Newtonsoft.Json.JsonTextReader(new StringReader(_jsonSerializedVariables)));
    }

    public void OnBeforeSerialize()
    {
        Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
        StringWriter stringWriter = new StringWriter(new StringBuilder());
        serializer.Serialize(stringWriter, _variables, typeof(List<Variable2>));
        _jsonSerializedVariables = stringWriter.GetStringBuilder().ToString();
    }
}

