using UnityEngine;

public class SetGameObjectsActiveAnimationEvent : MonoBehaviour
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private GameObject[] _gameObjects;
#pragma warning restore 0067, 0649

    public void SetActive()
    {
        foreach (GameObject gameObject in _gameObjects)
        {
            gameObject.SetActive(true);
        }
    }

    public void SetInactive()
    {
        foreach (GameObject gameObject in _gameObjects)
        {
            gameObject.SetActive(false);
        }
    }
}

