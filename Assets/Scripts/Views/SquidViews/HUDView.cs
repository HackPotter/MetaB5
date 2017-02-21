//using System;
//using System.Collections;
//using Squid;
//using UnityEngine;
//using Metablast.UI;

//public class HUDView : Frame, IHudView
//{
//    private Frame _contextMessageFrame;

//    //public event MouseEvent ScanButtonPressed;
//    //public event MouseEvent ToolButtonPressed;
//    //public event MouseEvent LightButtonPressed;

//    // Objectives...
//    // Help Button
//    // Exit Button?

    

//    // Current Objectives
//    private Button _objectivesWidgetToggle;
//    private Frame _objectivesWidgetVerticalAnimationFrame;
//    private Frame _objectivesWidgetHorizontalAnimationFrame;

//    // Biolog Widget
//    private Button _biologWidgetToggle;
//    private Frame _biologWidgetVerticalAnimationFrame;
//    private Frame _biologWidgetHorizontalAnimationFrame;
//    private Label _biologWidgetEntryLabel;
//    private ImageControl _biologWidgetPreviewImage;
//    private Button _biologWidgetOpenButton;

//    // Slide Frames
//    private Frame _leftSlideFrame;
//    private Frame _rightSlideFrame;
//    private Frame _bottomSlideFrame;
//    private Frame _centerSlideFrame;

//    // Tool stuff
//    private Button _scanButton;
//    private Button _toolButton;
//    private Button _lightButton;

//    // Animation state stuff
//    private float _animationDuration = 0.4f;
//    private bool _animating = false;

//    // Resource stuff
//    private int _nadphFillSize;
//    private Frame _nadphFillFrame;
//    private int _o2FillSize;
//    private Frame _o2FillFrame;
//    private int _atpFillSize;
//    private Frame _atpFillFrame;

//    private float _currentNADPH;
//    private float _currentO2;
//    private float _currentATP;

//    private bool _biologWidgetExpanded = true;
//    private Point _biologWidgetExpandedSize;
//    private BiologEntry _currentWidgetEntry;

//    private bool _objectiveWidgetExpanded = true;
//    private Point _objectiveWidgetExpandedSize;

//    public HUDView(Desktop hudDesktop)
//    {
//        GameContext.Instance.Player.CurrentObjectives.UserObjectiveAdded += new UserObjectiveAdded(CurrentObjectives_UserObjectiveAdded);
//        GameContext.Instance.Player.CurrentObjectives.UserObjectiveTaskAdded += new UserObjectiveTaskAdded(CurrentObjectives_UserObjectiveTaskAdded);
//        GameContext.Instance.Player.CurrentObjectives.UserObjectiveCompleted += new UserObjectiveCompleted(CurrentObjectives_UserObjectiveCompleted);
//        GameContext.Instance.Player.CurrentObjectives.UserObjectiveRemoved += new UserObjectiveRemoved(CurrentObjectives_UserObjectiveRemoved);
//        GameContext.Instance.Player.BiologProgress.BiologEntryScanned += new BiologEntryScannedHandler(BiologProgress_BiologEntryScanned);

//        hudDesktop.Dock = DockStyle.Fill;
//        Controls.Add(hudDesktop);

//        _leftSlideFrame = (Frame)GetControl("LeftSlideFrame");
//        _rightSlideFrame = (Frame)GetControl("RightSlideFrame");
//        _bottomSlideFrame = (Frame)GetControl("BottomSlideFrame");
//        _centerSlideFrame = (Frame)GetControl("CenterSlideFrame");

//        Button helpButton = (Button)GetControl("HelpButton");
//        Control helpFrame = GetControl("HelpFrame");
//        helpButton.MouseClick += (sender, args) => { helpFrame.Visible = !helpFrame.Visible; };
//        Button closeHelpButton = (Button)GetControl("CloseHelpButton");
//        closeHelpButton.MouseClick += (sender, args) => { helpFrame.Visible = false; };

//        Button exitGameButton = (Button)GetControl("Exit Game");
//        exitGameButton.MouseClick += new MouseEvent(exitGameButton_MouseClick);

//        _scanButton = (Button)GetControl("ScanButton");
//        _scanButton.MouseClick +=
//            (s, a) =>
//            {
//                Point center = _scanButton.Location + _scanButton.Size / 2;

//                float distance = GuiHost.MousePosition.Distance(center);
//                if (distance < _scanButton.Size.x / 2)
//                {
//                    ActiveTool currentTool = GameContext.Instance.Player.ActiveTool;
//                    GameContext.Instance.Player.ActiveTool = currentTool == ActiveTool.Scanner ? ActiveTool.None : ActiveTool.Scanner;
//                }
//            };

//        _toolButton = (Button)GetControl("ToolButton");
//        _toolButton.MouseClick +=
//            (s, a) =>
//            {
//                Point mousePos = GuiHost.MousePosition;
//                Point center = _toolButton.Location + _toolButton.Size / 2;

//                float distance = GuiHost.MousePosition.Distance(center);

//                if (distance < _toolButton.Size.x / 2)
//                {
//                    ActiveTool currentTool = GameContext.Instance.Player.ActiveTool;
//                    GameContext.Instance.Player.ActiveTool = currentTool == ActiveTool.ImpulseBeam ? ActiveTool.None : ActiveTool.ImpulseBeam;
//                }
//            };

//        _lightButton = (Button)GetControl("LightButton");
//        _lightButton.MouseClick +=
//            (s, a) =>
//            {
//                Point mousePos = GuiHost.MousePosition;
//                Point center = _lightButton.Location + _lightButton.Size / 2;

//                float distance = GuiHost.MousePosition.Distance(center);

//                if (distance < _toolButton.Size.x / 2)
//                {
//                    if (!GameContext.Instance.Player.LightEnabled)
//                    {
//                        GameContext.Instance.Player.LightEnabled = GameContext.Instance.Player.ATP > 0f;
//                    }
//                    else
//                    {
//                        GameContext.Instance.Player.LightEnabled = false;
//                    }
//                }
//            };


//        _contextMessageFrame = (Frame)GetControl("ContextMessageFrame");
//        ContextMessageView = new ContextMessageView(_contextMessageFrame);

//        TransmissionView = new TransmissionView((Frame)GetControl("Transmission"));

//        _nadphFillFrame = (Frame)(GetControl("NADPHFill"));
//        _nadphFillSize = _nadphFillFrame.Size.x;

//        _o2FillFrame = (Frame)(GetControl("O2Fill"));
//        _o2FillSize = _o2FillFrame.Size.x;

//        _atpFillFrame = (Frame)(GetControl("ATPFill"));
//        _atpFillSize = _atpFillFrame.Size.x;

//        Button expandOptionsButton = (Button)GetControl("OptionsToggle");
//        expandOptionsButton.MouseUp += GetOptionsToggleMouseEvent();

//        ((Frame)GetControl("Resource Bars")).Animation.Custom(AnimateResources());

//        _objectivesWidgetVerticalAnimationFrame = (Frame)GetControl("ObjectiveListFrame");
//        _objectivesWidgetHorizontalAnimationFrame = (Frame)GetControl("ObjectiveWidgetFrame");
//        _objectivesWidgetToggle = (Button)GetControl("ObjectivesWidgetToggle");

//        _biologWidgetVerticalAnimationFrame = (Frame)GetControl("BioLog");
//        _biologWidgetHorizontalAnimationFrame = (Frame)GetControl("BiologWidgetFrame");
//        _biologWidgetToggle = (Button)GetControl("BiologWidgetToggle");
//        _biologWidgetEntryLabel = (Label)GetControl("BiologWidgetEntryName");
//        _biologWidgetPreviewImage = (ImageControl)GetControl("BiologWidgetEntryPreview");
//        _biologWidgetOpenButton = (Button)GetControl("BiologWidgetOpenButton");

//        _biologWidgetOpenButton.MouseClick += new MouseEvent(_biologWidgetOpenButton_MouseClick);

//        ((Button)GetControl("OpenBiologButton")).MouseClick += _biologWidgetOpenButton_MouseClick;

//        _objectivesWidgetToggle.MouseClick += GetObjectivesToggleMouseEvent();
//        _biologWidgetToggle.MouseClick += GetBiologToggleMouseEvent();

//        _biologWidgetExpandedSize = _biologWidgetVerticalAnimationFrame.Size;

//        ContractObjectiveWidget();
//        ContractBiologWidget();
//    }


//    public event Action BiologButtonPressed;
//    public event Action OptionsButtonPressed;
//    public event Action ResumeGameButtonPressed;
//    public event Action ExitGameButtonPressed;

//    public IContextMessageView ContextMessageView
//    {
//        get;
//        private set;
//    }

//    public ITransmissionView TransmissionView
//    {
//        get;
//        private set;
//    }

//    public IObjectiveView ObjectiveFrameView
//    {
//        get { return null; }
//    }

//    public void Show(Action onCompleted, float animationTime = 0.4f)
//    {
//        if (_animating)
//        {
//            Animation.Stop();
//        }
//        _animating = true;
//        this.Visible = true;
//        Animation.Custom(SlideInFrames(animationTime, () => { _animating = false; onCompleted(); }));
//    }

//    public void Hide(Action onCompleted, float animationTime = 0.4f)
//    {
//        if (_animating)
//        {
//            Animation.Stop();
//        }
//        _animating = true;
//        Animation.Custom(SlideOutFrames(animationTime, () => { _animating = false; if (onCompleted != null) onCompleted(); }));
//    }



//    private IEnumerator SlideInFrames(float duration, Action onCompleted)
//    {
//        this.Visible = true;
//        float startTime = Time.time;

//        Point leftStart = _leftSlideFrame.Position;
//        Point rightStart = _rightSlideFrame.Position;
//        Point centerStart = _centerSlideFrame.Position;
//        Point bottomStart = _bottomSlideFrame.Position;

//        Point leftEnd = new Point(14, leftStart.y);
//        Point rightEnd = new Point(0, rightStart.y);
//        Point centerEnd = new Point(centerStart.x, 0);
//        Point bottomEnd = new Point(bottomStart.x, 0);

//        while (Time.time - startTime < duration)
//        {
//            float t = (Time.time - startTime) / duration;

//            _leftSlideFrame.Position = MathExt.SmoothStepPoint(leftStart, leftEnd, t);
//            _rightSlideFrame.Position = MathExt.SmoothStepPoint(rightStart, rightEnd, t);
//            _centerSlideFrame.Position = MathExt.SmoothStepPoint(centerStart, centerEnd, t);
//            _bottomSlideFrame.Position = MathExt.SmoothStepPoint(bottomStart, bottomEnd, t);
//            yield return null;
//        }

//        _leftSlideFrame.Position = leftEnd;
//        _rightSlideFrame.Position = rightEnd;
//        _centerSlideFrame.Position = centerEnd;
//        _bottomSlideFrame.Position = bottomEnd;

//        onCompleted();
//    }

//    private IEnumerator SlideOutFrames(float duration, Action onCompleted)
//    {
//        float startTime = Time.time;

//        Point leftStart = _leftSlideFrame.Position;
//        Point rightStart = _rightSlideFrame.Position;
//        Point centerStart = _centerSlideFrame.Position;
//        Point bottomStart = _bottomSlideFrame.Position;

//        Point leftEnd = new Point(-_leftSlideFrame.Size.x, leftStart.y);
//        Point rightEnd = new Point(_rightSlideFrame.Size.x, rightStart.y);
//        Point centerEnd = new Point(centerStart.x, -_centerSlideFrame.Size.y);
//        Point bottomEnd = new Point(bottomStart.x, _bottomSlideFrame.Size.y);

//        while (Time.time - startTime < duration)
//        {
//            float t = (Time.time - startTime) / duration;

//            _leftSlideFrame.Position = MathExt.SmoothStepPoint(leftStart, leftEnd, t);
//            _rightSlideFrame.Position = MathExt.SmoothStepPoint(rightStart, rightEnd, t);
//            _centerSlideFrame.Position = MathExt.SmoothStepPoint(centerStart, centerEnd, t);
//            _bottomSlideFrame.Position = MathExt.SmoothStepPoint(bottomStart, bottomEnd, t);
//            yield return null;
//        }

//        _leftSlideFrame.Position = leftEnd;
//        _rightSlideFrame.Position = rightEnd;
//        _centerSlideFrame.Position = centerEnd;
//        _bottomSlideFrame.Position = bottomEnd;

//        this.Visible = false;

//        onCompleted();
//    }

//    private IEnumerator AnimateResources()
//    {
//        _currentNADPH = GameContext.Instance.Player.NADPH;
//        _currentO2 = GameContext.Instance.Player.O2;
//        _currentATP = GameContext.Instance.Player.ATP;

//        while (true)
//        {
//            IPlayer player = GameContext.Instance.Player;
//            float atp = player.ATP;
//            float nadph = player.NADPH;
//            float o2 = player.O2;

//            _currentATP = Mathf.MoveTowards(_currentATP, atp, 1f * Time.deltaTime);
//            _currentO2 = Mathf.MoveTowards(_currentO2, o2, 1f * Time.deltaTime);
//            _currentNADPH = Mathf.MoveTowards(_currentNADPH, nadph, 1f * Time.deltaTime);


//            _nadphFillFrame.Size = new Point((int)(_currentNADPH * _nadphFillSize), _nadphFillFrame.Size.y);
//            _o2FillFrame.Size = new Point((int)(_currentO2 * _o2FillSize), _o2FillFrame.Size.y);
//            _atpFillFrame.Size = new Point((int)(_currentATP * _atpFillSize), _atpFillFrame.Size.y);

//            yield return null;
//        }
//    }

//    protected override void OnUpdate()
//    {
//        base.OnUpdate();

//        (GetControl("PointsLabel") as Label).Text = "" + GameContext.Instance.Player.Points;

//        switch (GameContext.Instance.Player.ActiveTool)
//        {
//            case ActiveTool.ImpulseBeam:
//                _toolButton.State = ControlState.Hot;
//                break;
//            case ActiveTool.Scanner:
//                _scanButton.State = ControlState.Hot;
//                break;
//            case ActiveTool.None:
//                break;
//        }

//        if (GameContext.Instance.Player.LightEnabled)
//        {
//            _lightButton.State = ControlState.Hot;
//        }
//    }

//    void _toolButton_MouseClick(Control sender, MouseEventArgs args)
//    {

//    }

//    void _biologWidgetOpenButton_MouseClick(Control sender, MouseEventArgs args)
//    {
//        AnalyticsLogger.Instance.AddLogEntry(new BiologOpenedLogEntry(GameContext.Instance.Player.UserGuid));
//        if (_currentWidgetEntry != null)
//        {
//            // G
//            //MetablastUI.Instance.OpenBiologToEntry(_currentWidgetEntry);
//        }
//        else
//        {
//            // G
//            //MetablastUI.Instance.OpenBiolog();
//        }
//    }

//    void BiologProgress_BiologEntryScanned(BiologEntry unlockedEntry, bool notify)
//    {
//        if (notify)
//        {
//            SetBiologWidgetEntry(unlockedEntry);
//            _currentWidgetEntry = unlockedEntry;
//            ExpandBiologWidget(_animationDuration);
//        }
//    }

//    void CurrentObjectives_UserObjectiveAdded(GameplayObjective objective)
//    {
//        ExpandObjectiveWidget(_animationDuration);
//    }

//    void CurrentObjectives_UserObjectiveTaskAdded(GameplayObjective objective, ObjectiveTask task)
//    {
//        ExpandObjectiveWidget(_animationDuration);
//    }

//    void CurrentObjectives_UserObjectiveRemoved(GameplayObjective objective)
//    {
//        ExpandObjectiveWidget(_animationDuration);
//    }

//    void CurrentObjectives_UserObjectiveCompleted(GameplayObjective objective)
//    {
//        ExpandObjectiveWidget(_animationDuration);
//    }

//    private void SetBiologWidgetEntry(BiologEntry entry)
//    {
//        //_biologWidgetPreviewImage.Texture = entry.GalleryItems.Count != 0 ? entry.GalleryItems[0].GalleryPreviewResourcePath : "";
//        //_biologWidgetEntryLabel.Text = entry.EntryName;
//    }

//    private void ExpandBiologWidget()
//    {
//        ExpandBiologWidget(0);
//    }

//    private void ExpandBiologWidget(float duration)
//    {
//        if (_biologWidgetExpanded)
//        {
//            return;
//        }

//        Point startPosition = _biologWidgetVerticalAnimationFrame.Position;
//        Point endPosition = startPosition;
//        endPosition.x = 0;
//        Point startingSize = _biologWidgetVerticalAnimationFrame.Size;
//        Point endSize = _biologWidgetExpandedSize;

//        _biologWidgetVerticalAnimationFrame.Animation.Custom(ExpandWidget(_biologWidgetVerticalAnimationFrame, _biologWidgetHorizontalAnimationFrame, endSize, duration));
//        _biologWidgetExpanded = true;
//    }

//    private void ContractBiologWidget()
//    {
//        ContractBiologWidget(0);
//    }

//    private void ContractBiologWidget(float duration)
//    {
//        if (!_biologWidgetExpanded)
//        {
//            return;
//        }
//        Point startPosition = _biologWidgetVerticalAnimationFrame.Position;
//        Point endPosition = startPosition;
//        endPosition.x = 0;
//        Point startingSize = _biologWidgetVerticalAnimationFrame.Size;
//        Point endSize = _biologWidgetExpandedSize;

//        _biologWidgetVerticalAnimationFrame.Animation.Custom(ContractWidget(_biologWidgetVerticalAnimationFrame, _biologWidgetHorizontalAnimationFrame, 270, duration));
//        _biologWidgetExpanded = false;
//    }

//    private void ExpandObjectiveWidget()
//    {
//        ExpandObjectiveWidget(0);
//    }

//    private void ExpandObjectiveWidget(float duration)
//    {
//        _objectivesWidgetVerticalAnimationFrame.Animation.Stop();
//        Control targetSizeControl = _objectivesWidgetVerticalAnimationFrame.GetControl("ObjectiveListFrameContents");
//        targetSizeControl.PerformLayout();
//        Point endSize = targetSizeControl.Size;
//        _objectivesWidgetVerticalAnimationFrame.Animation.Custom(ExpandWidget(_objectivesWidgetVerticalAnimationFrame, _objectivesWidgetHorizontalAnimationFrame, endSize, duration));
//        _objectiveWidgetExpanded = true;
//    }

//    private void ContractObjectiveWidget()
//    {
//        ContractObjectiveWidget(0);
//    }

//    private void ContractObjectiveWidget(float duration)
//    {
//        _objectivesWidgetVerticalAnimationFrame.Animation.Stop();
//        _objectivesWidgetVerticalAnimationFrame.Animation.Custom(ContractWidget(_objectivesWidgetVerticalAnimationFrame, _objectivesWidgetHorizontalAnimationFrame, 270, duration));
//        _objectiveWidgetExpanded = false;
//    }

//    MouseEvent GetOptionsToggleMouseEvent()
//    {
//        Control control = GetControl("ScissorFrame");
//        Point startingSize = control.Size;
//        Point endSize = startingSize;
//        endSize.y = 0;
//        bool expand = false;

//        return (s, a) =>
//            {
//                if (expand)
//                {
//                    control.Animation.Stop();
//                    control.Animation.Custom(OptionsToggleAnimation(control, startingSize, _animationDuration));
//                    expand = !expand;
//                }
//                else
//                {
//                    control.Animation.Stop();
//                    control.Animation.Custom(OptionsToggleAnimation(control, endSize, _animationDuration));
//                    expand = !expand;
//                }
//            };
//    }

//    MouseEvent GetObjectivesToggleMouseEvent()
//    {
//        return (s, a) =>
//        {
//            if (_objectiveWidgetExpanded)
//            {
//                ContractObjectiveWidget(_animationDuration);
//            }
//            else
//            {
//                ExpandObjectiveWidget(_animationDuration);
//            }
//        };
//    }

//    MouseEvent GetBiologToggleMouseEvent()
//    {
//        return (s, a) =>
//        {
//            if (_biologWidgetExpanded)
//            {
//                ContractBiologWidget(_animationDuration);
//            }
//            else
//            {
//                ExpandBiologWidget(_animationDuration);
//            }
//        };
//    }

//    IEnumerator ExpandWidget(Control expandingControl, Control movedControl, Point size, float duration)
//    {
//        yield return new WaitForSeconds(0.1f);
//        var animation = movedControl.Animation.Custom(MoveControlHorizontal(movedControl, 0, duration));
//        while (!animation.IsFinished)
//        {
//            yield return null;
//        }

//        animation = expandingControl.Animation.Custom(OptionsToggleAnimation(expandingControl, size, duration));
//        while (!animation.IsFinished)
//        {
//            yield return null;
//        }
//    }

//    IEnumerator ContractWidget(Control expandingControl, Control movedControl, int width, float duration)
//    {
//        Point targetSize = expandingControl.Size;
//        targetSize.y = 0;
//        var animation = Animation.Custom(OptionsToggleAnimation(expandingControl, targetSize, duration));

//        while (!animation.IsFinished)
//        {
//            yield return null;
//        }

//        animation = Animation.Custom(MoveControlHorizontal(movedControl, width, duration));
//        while (!animation.IsFinished)
//        {
//            yield return null;
//        }
//    }

//    IEnumerator MoveControlHorizontal(Control controlToMove, int horizontalPosition, float duration)
//    {
//        float startingTime = Time.time;
//        Point startingPosition = controlToMove.Position;

//        while (Time.time - startingTime < duration)
//        {
//            Point targetPosition = new Point(horizontalPosition, controlToMove.Position.y);
//            float t = (Time.time - startingTime) / duration;
//            controlToMove.Position = new Point((int)Mathf.SmoothStep(startingPosition.x, targetPosition.x, t), targetPosition.y);
//            controlToMove.Parent.PerformLayout();
//            yield return null;
//        }
//        controlToMove.Position = new Point(horizontalPosition, controlToMove.Position.y);
//    }

//    IEnumerator OptionsToggleAnimation(Control control, Point size, float duration)
//    {
//        float startingTime = Time.time;
//        Point startingSize = control.Size;
//        while (Time.time - startingTime < duration && control.Size != size)
//        {
//            float t = (Time.time - startingTime) / duration;
//            control.Size = new Point((int)Mathf.SmoothStep(startingSize.x, size.x, t), (int)Mathf.SmoothStep(startingSize.y, size.y, t));
//            control.Parent.PerformLayout();
//            yield return null;
//        }
//        control.Size = size;
//    }

//    IEnumerator MoveControl(Control control, Point newPosition, float duration)
//    {
//        float startingTime = Time.time;
//        Point startingPosition = control.Position;
//        while (Time.time - startingTime < duration)
//        {
//            float t = (Time.time - startingTime) / duration;
//            control.Position = new Point((int)Mathf.SmoothStep(startingPosition.x, newPosition.x, t), (int)Mathf.SmoothStep(startingPosition.y, newPosition.y, t));
//            yield return null;
//        }
//        control.Position = newPosition;
//    }

//    void exitGameButton_MouseClick(Control sender, MouseEventArgs args)
//    {
//        if (ExitGameButtonPressed != null)
//        {
//            ExitGameButtonPressed();
//        }
//        Application.LoadLevel(0);
//    }

//    void resumeGameButton_MouseClick(Control sender, MouseEventArgs args)
//    {
//        if (ResumeGameButtonPressed != null)
//        {
//            ResumeGameButtonPressed();
//        }
//    }

//    void optionsButton_MouseClick(Control sender, MouseEventArgs args)
//    {
//        if (OptionsButtonPressed != null)
//        {
//            OptionsButtonPressed();
//        }
//    }
//}

