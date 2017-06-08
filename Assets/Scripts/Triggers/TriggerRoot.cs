using UnityEngine;

[RequireComponent(typeof(GlobalSymbolTableAccessor))]
public class TriggerRoot : MonoBehaviour
{
    private bool _initialized = false;
    private GlobalSymbolTable _globalSymbolTable;

    [SerializeField]
    [Header("HACK: Workaround for refactorization process.")]
    private DialogueController _dialogueController;
    [SerializeField]
    private MetablastUI _metablastUI;
    [SerializeField]
    private WaypointView _waypointView;

    public GlobalSymbolTable GlobalSymbolTable
    {
        get
        {
            Initialize();
            return _globalSymbolTable;
        }
    }

    public DialogueController DialogueController
    {
        get { return _dialogueController; }
    }

    public MetablastUI MetablastUI
    {
        get { return _metablastUI; }
    }

    public WaypointView WaypointView
    {
        get { return _waypointView; }
    }

    private void Initialize()
    {
        Cursor.lockState = CursorLockMode.None;
        if (_initialized)
            return;
        _initialized = true;

        _globalSymbolTable = GetComponentInParent<GlobalSymbolTableAccessor>().GlobalSymbolTable;
    }
}