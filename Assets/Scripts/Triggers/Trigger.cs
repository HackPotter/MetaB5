using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField]
    private string _triggerComment = "";

    [SerializeField]
    private bool _enabled = true;

    public bool Enabled
    {
        get
        {
            return _enabled;
        }

        set
        {
            _enabled = value;
            this.gameObject.SetActive(value);
        }
    }

    public string Comment
    {
        get { return _triggerComment; }
        set { _triggerComment = value; }
    }
}

