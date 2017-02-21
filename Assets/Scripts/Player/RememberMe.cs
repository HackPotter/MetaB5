using System.Collections.Generic;
using UnityEngine;

public class RememberMe : MonoBehaviour
{
    private static Dictionary<string, Dictionary<string, Vector3>> _lastPositions = new Dictionary<string, Dictionary<string, Vector3>>();
    private static Dictionary<string, Dictionary<string, Quaternion>> _lastRotations = new Dictionary<string, Dictionary<string, Quaternion>>();

#pragma warning disable 0067, 0649
    [SerializeField]
    private string _saveName;
    [SerializeField]
    private GameObject _gameObjectToRemember;
#pragma warning restore 0067, 0649

    public static void Clear()
    {
        _lastPositions.Clear();
        _lastRotations.Clear();
    }

    void OnEnable()
    {
        if (!_lastPositions.ContainsKey(Application.loadedLevelName))
        {
            _lastPositions.Add(Application.loadedLevelName, new Dictionary<string, Vector3>());
        }
        if (!_lastRotations.ContainsKey(Application.loadedLevelName))
        {
            _lastRotations.Add(Application.loadedLevelName, new Dictionary<string, Quaternion>());
        }

        if (_lastPositions[Application.loadedLevelName].ContainsKey(_saveName))
        {
            _gameObjectToRemember.transform.position = _lastPositions[Application.loadedLevelName][_saveName];
        }
        if (_lastRotations[Application.loadedLevelName].ContainsKey(_saveName))
        {
            _gameObjectToRemember.transform.rotation = _lastRotations[Application.loadedLevelName][_saveName];
        }
    }

    void OnDisable()
    {
        if (!_lastPositions.ContainsKey(Application.loadedLevelName))
        {
            _lastPositions.Add(Application.loadedLevelName, new Dictionary<string, Vector3>());
        }
        if (!_lastRotations.ContainsKey(Application.loadedLevelName))
        {
            _lastRotations.Add(Application.loadedLevelName, new Dictionary<string, Quaternion>());
        }

        _lastPositions[Application.loadedLevelName][_saveName] = _gameObjectToRemember.transform.position;
        _lastRotations[Application.loadedLevelName][_saveName] = _gameObjectToRemember.transform.rotation;
    }
}
