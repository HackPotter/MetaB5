using Squid;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ControlServices))]
public class ScanningFunction : MonoBehaviour, IFunction
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private KeyCode _shortcutKey;
    [SerializeField]
    private ToolEnabledEvent _scannerEnabled;
    [SerializeField]
    private ToolEnabledEvent _scannerDisabled;
#pragma warning restore 0067, 0649

    private ControlServices _controlServices;
    private ScannableObject _highlighted;

    public KeyCode ShortcutKey
    {
        get { return _shortcutKey; }
    }

    void Start()
    {
        //MetablastUI.Instance.HudView.ScanButtonPressed += (HudView_ScanButtonPressed);
        _controlServices = GetComponent<ControlServices>();
    }

    void OnDestroy()
    {
        //MetablastUI.Instance.HudView.ScanButtonPressed -= (HudView_ScanButtonPressed);
    }

    void HudView_ScanButtonPressed(Control sender, MouseEventArgs args)
    {
        //_scanningActivated = !_scanningActivated;
    }

    void OnEnable()
    {
        _scannerEnabled.Invoke();
    }

    void OnDisable()
    {
        //MetablastUI.Instance.Desktop.CurrentCursor = null;
        _scannerDisabled.Invoke();
        MetablastUI.Instance.HudView.ContextMessageView.SetLowPriorityText("");
        DisableScannedObject();
    }

    void Update()
    {
        //MetablastUI.Instance.Desktop.CurrentCursor = "ScanCursor";

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            DoScan();
            if (Input.GetMouseButtonDown(0))
            {
                if (_highlighted != null)
                {
                    AnalyticsLogger.Instance.AddLogEntry(new ObjectScannedLogEntry(GameContext.Instance.Player.UserGuid, _highlighted));
                    GameContext.Instance.Player.BiologProgress.UnlockEntry(_highlighted.BiologEntry);
                    MetablastUI.Instance.Desktop.CurrentCursor = null;
                    GameContext.Instance.Player.ActiveTool = ActiveTool.None;
                    //GetComponent<ToolController>().ToggleScanner();
                }
                DisableScannedObject();
            }
        }
    }

    private void EnableScannedObject(ScannableObject scannable)
    {
        if (scannable)
        {
            scannable.enabled = true;
            _highlighted = scannable;
            if (GameContext.Instance.Player.BiologProgress.IsEntryUnlocked(scannable.BiologEntry))
            {
                MetablastUI.Instance.HudView.ContextMessageView.SetLowPriorityText("Left click to scan " + _highlighted.BiologEntry);
            }
            else
            {
                MetablastUI.Instance.HudView.ContextMessageView.SetLowPriorityText("Left click to scan unknown object");
            }
        }
    }

    private void DisableScannedObject()
    {
        if (_highlighted)
        {
            _highlighted.enabled = false;
            _highlighted = null;
            MetablastUI.Instance.HudView.ContextMessageView.SetLowPriorityText("");
        }
    }

    private void DoScan()
    {
        Camera cam = _controlServices.ControlledCamera.Camera;
        if (!cam)
        {
            DisableScannedObject();
            return;
        }

        Ray r = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (!Physics.Raycast(r, out hitInfo, 500))
        {
            DisableScannedObject();
            return;
        }

        GameObject hitObject = hitInfo.collider.gameObject;
        ScannableObject scannable = hitObject.GetComponent<ScannableObject>();
        if (scannable != _highlighted)
        {
            DisableScannedObject();
            EnableScannedObject(scannable);
            return;
        }
    }

    public void OnAcquiredControl()
    {
        this.enabled = true;
    }

    public void OnLostControl()
    {
        this.enabled = false;
    }
}
