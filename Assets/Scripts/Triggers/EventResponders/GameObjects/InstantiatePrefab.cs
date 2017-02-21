using UnityEngine;

[Trigger(Description = "Instantiates the given prefab with the given parameters.", DisplayPath = "GameObject")]
[AddComponentMenu("Metablast/Triggers/Actions/GameObjects/Instantiate Prefab")]
public class InstantiatePrefab : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("Optional parent under which the prefab will be instantiated.")]
    private GameObject _optionalParent;

    [SerializeField]
    [Infobox("Optional name of instantiated prefab.")]
    private string _optionalName;

    [SerializeField]
    [Infobox("The prefab to instantiate.")]
    private GameObject _prefab;

    [SerializeField]
    [Infobox("Whether or not the instantiated prefab will inherit the transform of the parent.")]
    private bool _inheritTransform;

    [SerializeField]
    [Infobox("Euler angle rotation of instantiated prefab.")]
    private Vector3 _rotation;

    [SerializeField]
    [Infobox("Position of instantiated prefab.")]
    private Vector3 _position;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        GameObject gameObject = (GameObject)GameObject.Instantiate(_prefab, Vector3.zero, Quaternion.identity);

        if (_inheritTransform && _optionalParent)
        {
            gameObject.transform.parent = _optionalParent.transform;
            gameObject.transform.localPosition = _position;
            gameObject.transform.localRotation = Quaternion.Euler(_rotation);
        }
        else if (_optionalParent)
        {
            gameObject.transform.parent = _optionalParent.transform;
            gameObject.transform.position = _position;
            gameObject.transform.rotation = Quaternion.Euler(_rotation);
        }
        else
        {
            gameObject.transform.position = _position;
            gameObject.transform.rotation = Quaternion.Euler(_rotation);
        }

        if (!string.IsNullOrEmpty(_optionalName))
        {
            gameObject.name = _optionalName;
        }
    }
}
