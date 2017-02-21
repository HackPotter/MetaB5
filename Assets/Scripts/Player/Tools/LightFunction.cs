using UnityEngine;

public class LightFunction : MonoBehaviour
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private KeyCode _shortcutKey;
    [SerializeField]
    private GameObject LightGameObject;
    [SerializeField]
    private float _ATPCostPerSecond = 0.1f;
#pragma warning restore 0067, 0649

    public KeyCode ShortcutKey
    {
        get { return _shortcutKey; }
    }

    public float ATPCostPerSecond
    {
        get { return _ATPCostPerSecond; }
    }

    void OnEnable()
    {
        LightGameObject.SetActive(true);
    }

    void Update()
    {
        GameContext.Instance.Player.ATP -= _ATPCostPerSecond * Time.deltaTime;

        if (GameContext.Instance.Player.ATP == 0)
        {
            GameContext.Instance.Player.LightEnabled = false;
        }
    }

    void OnDisable()
    {
        LightGameObject.SetActive(false);
    }
}

