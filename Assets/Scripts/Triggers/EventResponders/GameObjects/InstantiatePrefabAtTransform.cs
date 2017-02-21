using UnityEngine;

[Trigger(Description = "Instantiates a prefab at the position of the given transform.", DisplayPath = "GameObject")]
[AddComponentMenu("Metablast/Triggers/Actions/GameObjects/Instantiate Prefab at Transform")]
public class InstantiatePrefabAtTransform : EventResponder
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
    [Infobox("The transform the location of which is where the instantiated prefab will be placed.")]
    private Transform _locationTransform;

    [SerializeField]
    [Infobox("If true, the given offsets will be in the location transform's local space. Otherwise, they will be in world space.")]
    private bool _useLocationTransformSpace = true;

    [SerializeField]
    [Infobox("The Euler angle rotation offset.")]
    private Vector3 _rotationOffset;

    [SerializeField]
    [Infobox("The local position offset.")]
    private Vector3 _positionOffset;

    [SerializeField]
    [Infobox("If true, the the location transform's scale will be used for the instantiated prefab.")]
    private bool _useLocationTransformScale;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        if (!_locationTransform)
        {
            DebugFormatter.LogError(this, "Location transform is null. {0} must have a location transform.", GetType().Name);
            return;
        }
        if (!_prefab)
        {
            DebugFormatter.LogError(this, "Prefab to instantiate is null.");
            return;
        }

        Vector3 offset = (_useLocationTransformSpace ? _locationTransform.TransformDirection(_positionOffset) : _positionOffset);
        Vector3 spawnPosition = _locationTransform.position + offset;
        Quaternion spawnRotation = _locationTransform.rotation * Quaternion.Euler(_rotationOffset);
        GameObject gameObject = (GameObject)GameObject.Instantiate(_prefab, spawnPosition, spawnRotation);

        if (_useLocationTransformScale)
        {
            gameObject.transform.localScale = _locationTransform.localScale;
        }

        if (_optionalParent)
        {
            gameObject.transform.parent = _optionalParent.transform;
        }

        if (!string.IsNullOrEmpty(_optionalName))
        {
            gameObject.name = _optionalName;
        }
    }
}
