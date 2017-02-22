using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        string currentScene = SceneManager.GetActiveScene().name;

        if (!_lastPositions.ContainsKey(currentScene))
        {
            _lastPositions.Add(currentScene, new Dictionary<string, Vector3>());
        }
        if (!_lastRotations.ContainsKey(currentScene))
        {
            _lastRotations.Add(currentScene, new Dictionary<string, Quaternion>());
        }

        if (_lastPositions[currentScene].ContainsKey(_saveName))
        {
            _gameObjectToRemember.transform.position = _lastPositions[currentScene][_saveName];
        }
        if (_lastRotations[currentScene].ContainsKey(_saveName))
        {
            _gameObjectToRemember.transform.rotation = _lastRotations[currentScene][_saveName];
        }
    }

    void OnDisable()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (!_lastPositions.ContainsKey(currentScene))
        {
            _lastPositions.Add(currentScene, new Dictionary<string, Vector3>());
        }
        if (!_lastRotations.ContainsKey(currentScene))
        {
            _lastRotations.Add(currentScene, new Dictionary<string, Quaternion>());
        }

        _lastPositions[currentScene][_saveName] = _gameObjectToRemember.transform.position;
        _lastRotations[currentScene][_saveName] = _gameObjectToRemember.transform.rotation;
    }
}
