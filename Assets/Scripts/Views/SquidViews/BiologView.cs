#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0649 // Field is never assigned to, and will always have its default value.

using Squid;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiologView : Frame
{
    public event Action ExitButtonPressed;

    //private BiologData _biologData;
    private FlowLayoutFrame _tagsList;
	
	private Frame _backgroundTintFrame;
	
    // Database stuff
    private FlowLayoutFrame _databaseList;
    private Frame _databaseScrollFrame;
    private Slider _databaseSlider;

    // Gallery frame.
    private ImageControl _galleryImage;
    private Frame _navigatorPreviewFrame;

    // Content Frame stuff.
    private Slider _centerSlider;
    private Frame _centerScrollFrame;
    private Frame _centerContentPage;
    private Label _summaryTitle;
    private TextArea _summaryLabel;
    private Label _detailLabel;

    private Frame _scaleFrame;
    private Label _scaleLabel;

    // Animation Stuff
    private Frame _leftSlideFrame;
    private Frame _rightSlideFrame;
    private Frame _centerSlideFrame;
    private Frame _topSlideFrame;
    private Frame _bottomSlideFrame;
    private Frame _separatorSlideFrame;

    private IBiologProgress _biologProgress;
    private Dictionary<BiologEntry, Button> _buttonsByEntry = new Dictionary<BiologEntry, Button>();
    private Control _selectedBiologEntryControl;

    private float _animationTime = 0.4f;
    private bool _animating = false;

    private RenderTexture _3dPreviewTexture;

    private GameObject _3dPreviewScene;

	public bool IsShowing
	{
		get;
		private set;
	}

    public BiologView(Desktop biologDesktop, IBiologProgress biologData)
    {
        biologDesktop.Dock = DockStyle.Fill;
        Controls.Add(biologDesktop);

        _biologProgress = biologData;
        _biologProgress.BiologEntryScanned += _biologProgress_BiologEntryUnlocked;
		
		_backgroundTintFrame = (Frame)GetControl("TintFrame");
		
        // Animation stuff
        _leftSlideFrame = (Frame)GetControl("LeftSlideFrame");
        _rightSlideFrame = (Frame)GetControl("RightSlideFrame");
        _centerSlideFrame = (Frame)GetControl("CenterSlideFrame");
        _topSlideFrame = (Frame)GetControl("TopSlideFrame");
        _bottomSlideFrame = (Frame)GetControl("BottomSlideFrame");
        _separatorSlideFrame = (Frame)GetControl("SeparatorSlideFrame");

        // Database stuff
        _databaseList = (FlowLayoutFrame)GetControl("Database List");
        _databaseList.Controls.Clear();
        _databaseList.ForceFlowLayout();
        _databaseScrollFrame = (Frame)GetControl("DatabaseScrollFrame");
        _databaseScrollFrame.Update += _databaseList_Update;
        _databaseSlider = (Slider)GetControl("DatabaseSlider");

        // Center content pane stuff
        _centerSlider = (Slider)GetControl("CenterSlider");
        _centerSlider.Ease = true;
        _centerScrollFrame = (Frame)GetControl("Scroll Page");
        _centerContentPage = (Frame)GetControl("CenterContent");
        _centerScrollFrame.Update += new VoidEvent(_centerScrollPage_Update);

        _summaryTitle = (Label)GetControl("SummaryHeader");
        _summaryLabel = (TextArea)GetControl("SummaryInfo");
        _detailLabel = (Label)GetControl("DetailInfo");

        // Tags stuff
        _tagsList = (FlowLayoutFrame)GetControl("Tags");
        _tagsList.Controls.Clear();

        _scaleLabel = (Label)GetControl("Scale Number");
        _scaleFrame = (Frame)GetControl("Scalebar");


        // Gallery stuff
        _galleryImage = (ImageControl)GetControl("Entry");
        _navigatorPreviewFrame = (Frame)GetControl("NavigatorPreviewFrame");
        _navigatorPreviewFrame.Controls.Clear();

        (GetControl("Exit") as Button).MouseClick += (c, s) => { if (ExitButtonPressed != null) ExitButtonPressed(); };

        _3dPreviewTexture = ResourcesExt.Load<RenderTexture>("Biolog/3DPreviews/Biolog3DPreview");

        Initialize();
    }
	
	public void SetTint(float tint)
	{
		_backgroundTintFrame.Opacity = tint;
	}
	
    public void Show(Action onCompleted)
    {
        if (_animating)
        {
            return;
        }
		IsShowing = true;
        GameState.Instance.PauseLevel = PauseLevel.Menu;
        _animating = true;
        Enabled = true;
        this.Visible = true;
        // Make sure it's hidden initially.
        _leftSlideFrame.Position = new Point(-_leftSlideFrame.Size.x, _leftSlideFrame.Position.y);
        _rightSlideFrame.Position = new Point(_rightSlideFrame.Size.x, _rightSlideFrame.Position.y);
        _separatorSlideFrame.Position = new Point(_separatorSlideFrame.Size.x, _separatorSlideFrame.Position.y);
        _centerSlideFrame.Position = new Point(_centerSlideFrame.Position.x, _centerSlideFrame.Size.y);
        _topSlideFrame.Position = new Point(_topSlideFrame.Position.x, -_topSlideFrame.Size.y);

        Animation.Custom(SlideIn(_animationTime, () => {_animating = false; onCompleted(); }));
    }

    public void Hide(Action onCompleted)
    {
        if (_animating)
        {
            return;
        }
        if (_3dPreviewScene)
        {
            GameObject.Destroy(_3dPreviewScene);
        }
		IsShowing = false;
        GameState.Instance.PauseLevel = PauseLevel.Unpaused;
        _animating = true;
        // Make sure it's visible initially.
        _leftSlideFrame.Position = new Point(0, _leftSlideFrame.Position.y);
        _rightSlideFrame.Position = new Point(0, _rightSlideFrame.Position.y);
        _separatorSlideFrame.Position = new Point(0, _separatorSlideFrame.Position.y);
        _centerSlideFrame.Position = new Point(_centerSlideFrame.Position.x, 0);
        _topSlideFrame.Position = new Point(_topSlideFrame.Position.x, 0);

        Animation.Custom(SlideOut(_animationTime, () => { _animating = false; Enabled = false; onCompleted(); }));
    }

    public void SetActiveEntry(BiologEntry entry)
    {
        _selectedBiologEntryControl = _buttonsByEntry[entry];
        _summaryTitle.Text = entry.EntryName;
        _scaleLabel.Text = entry.Scale;
        if (string.IsNullOrEmpty(_scaleLabel.Text))
        {
            _scaleFrame.Visible = false;
        }
        else
        {
            _scaleFrame.Visible = true;
        }
    }

    protected override void OnLateUpdate()
    {
        base.OnLateUpdate();

        if (_selectedBiologEntryControl != null)
        {
            _selectedBiologEntryControl.State = ControlState.Pressed;
        }
    }

    private void Initialize()
    {
        _databaseList.Controls.Clear();
        _buttonsByEntry.Clear();
        foreach (BiologEntry entry in _biologProgress.UnlockedEntries)
        {
            Button entryButton = CreateBiologEntryButton(entry);
            _buttonsByEntry.Add(entry, entryButton);
            entryButton.MouseClick += new MouseEvent(entryButton_MouseClick);
            _databaseList.Controls.Add(entryButton);
        }

        _databaseSlider.Maximum = 1;
        _databaseSlider.Steps = _biologProgress.UnlockedEntries.Count;

        _centerSlider.Maximum = 1;
        _centerSlider.Steps = 500;
    }

    IEnumerator SlideIn(float duration, Action onCompleted)
    {
        this.Visible = true;
        float startTime = Time.time;

        Point leftStart = _leftSlideFrame.Position;
        Point rightStart = _rightSlideFrame.Position;
        Point centerStart = _centerSlideFrame.Position;
        Point topStart = _topSlideFrame.Position;
        Point bottomStart = _bottomSlideFrame.Position;
        Point separatorStart = _separatorSlideFrame.Position;

        Point leftEnd = new Point(0, leftStart.y);
        Point rightEnd = new Point(0, rightStart.y);
        Point separatorEnd = new Point(0, separatorStart.y);
        Point centerEnd = new Point(centerStart.x, 0);
        Point topEnd = new Point(topStart.x, 0);
        Point bottomEnd = new Point(bottomStart.x, 0);

        Vignetting vignetting = ControlServices.Instance.ControlledCamera.Camera.GetComponents<Vignetting>()[1];
        BlurEffect blur = ControlServices.Instance.ControlledCamera.Camera.GetComponent<BlurEffect>();
        blur.enabled = true;
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            
            _leftSlideFrame.Position = MathExt.SmoothStepPoint(leftStart, leftEnd, t);
            _rightSlideFrame.Position = MathExt.SmoothStepPoint(rightStart, rightEnd, t);
            _centerSlideFrame.Position = MathExt.SmoothStepPoint(centerStart, centerEnd, t);
            _topSlideFrame.Position = MathExt.SmoothStepPoint(topStart, topEnd, t);
            _bottomSlideFrame.Position = MathExt.SmoothStepPoint(bottomStart, bottomEnd, t);
            _separatorSlideFrame.Position = MathExt.SmoothStepPoint(separatorStart, separatorEnd, t);

            vignetting.intensity = Mathf.Lerp(0, 2.3f, t);
            vignetting.blur = Mathf.Lerp(0, 2f, t);
            vignetting.chromaticAberration = Mathf.Lerp(0, 25f, t);
            vignetting.blurSpread = Mathf.Lerp(0, 3f, t);
            yield return null;
        }
        

        _leftSlideFrame.Position = leftEnd;
        _rightSlideFrame.Position = rightEnd;
        _centerSlideFrame.Position = centerEnd;
        _topSlideFrame.Position = topEnd;
        _bottomSlideFrame.Position = bottomEnd;
        _separatorSlideFrame.Position = separatorEnd;
        vignetting.intensity = 2.3f;
        vignetting.blur = 2f;
        vignetting.chromaticAberration = 25f;
        vignetting.blurSpread = 3f;

        onCompleted();
    }

    IEnumerator SlideOut(float duration, Action onCompleted)
    {
        float startTime = Time.time;

        Point leftStart = _leftSlideFrame.Position;
        Point rightStart = _rightSlideFrame.Position;
        Point centerStart = _centerSlideFrame.Position;
        Point topStart = _topSlideFrame.Position;
        Point bottomStart = _bottomSlideFrame.Position;
        Point separatorStart = _separatorSlideFrame.Position;

        Point leftEnd = new Point(-_leftSlideFrame.Size.x, leftStart.y);
        Point rightEnd = new Point(_rightSlideFrame.Size.x, rightStart.y);
        Point separatorEnd = new Point(_separatorSlideFrame.Size.x, separatorStart.y);
        Point centerEnd = new Point(centerStart.x, _centerSlideFrame.Size.y);
        Point topEnd = new Point(topStart.x, -_topSlideFrame.Size.y);
        Point bottomEnd = new Point(bottomStart.x, _bottomSlideFrame.Size.y);

        Vignetting vignetting = ControlServices.Instance.ControlledCamera.Camera.GetComponents<Vignetting>()[1];
        BlurEffect blur = ControlServices.Instance.ControlledCamera.Camera.GetComponent<BlurEffect>();
        blur.enabled = false;
        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;

            _leftSlideFrame.Position = MathExt.SmoothStepPoint(leftStart, leftEnd, t);
            _rightSlideFrame.Position = MathExt.SmoothStepPoint(rightStart, rightEnd, t);
            _centerSlideFrame.Position = MathExt.SmoothStepPoint(centerStart, centerEnd, t);
            _topSlideFrame.Position = MathExt.SmoothStepPoint(topStart, topEnd, t);
            _bottomSlideFrame.Position = MathExt.SmoothStepPoint(bottomStart, bottomEnd, t);
            _separatorSlideFrame.Position = MathExt.SmoothStepPoint(separatorStart, separatorEnd, t);
            vignetting.intensity = Mathf.Lerp(2.3f, 0, t);
            vignetting.blur = Mathf.Lerp(2f, 0, t);
            vignetting.chromaticAberration = Mathf.Lerp(25f, 0, t);
            vignetting.blurSpread = Mathf.Lerp(3f, 0, t);
            yield return null;
        }
        

        _leftSlideFrame.Position = leftEnd;
        _rightSlideFrame.Position = rightEnd;
        _centerSlideFrame.Position = centerEnd;
        _topSlideFrame.Position = topEnd;
        _bottomSlideFrame.Position = bottomEnd;
        _separatorSlideFrame.Position = separatorEnd;
        vignetting.intensity = 0f;
        vignetting.blur = 0f;
        vignetting.chromaticAberration = 0f;
        vignetting.blurSpread = 0f;

        this.Visible = false;

        onCompleted();
    }

    void _biologProgress_BiologEntryUnlocked(BiologEntry entry, bool notify)
    {
        Initialize();
        SetActiveEntry(entry);
        InitializeNavigatorThumbnails(entry);
    }

    void _centerScrollPage_Update(Control sender)
    {
        _centerContentPage.Size = new Point(_centerScrollFrame.Size.x, _centerContentPage.Size.y);

        if (_centerContentPage.Size.y < _centerScrollFrame.Size.y)
        {
            _centerSlider.Visible = false;
            _centerContentPage.Position = new Point(_centerContentPage.Position.x, 0);//_centerScrollFrame.Size.y - _centerContentPage.Size.y);
        }
        else
        {
            _centerSlider.Visible = true;
            _centerContentPage.Position = new Point(_centerContentPage.Position.x, (int)((_centerScrollFrame.Size.y - _centerContentPage.Size.y) * _centerSlider.EasedValue));
        }

        if (GuiHost.MouseScroll != 0 && sender.Desktop.HotControl != null && _centerScrollFrame.Hit(GuiHost.MousePosition.x, GuiHost.MousePosition.y))
        {
            _centerSlider.Value += 25 * GuiHost.MouseScroll / _centerSlider.Steps;
        }
    }

    void _databaseList_Update(Control sender)
    {
        _databaseList.Size = new Point(_databaseScrollFrame.Size.x, _databaseList.Size.y);

        if (_databaseList.Size.y < _databaseScrollFrame.Size.y)
        {
            _databaseSlider.Visible = false;
            _databaseList.Position = new Point(_databaseList.Position.x, 0);
        }
        else
        {
            _databaseSlider.Visible = true;
            _databaseList.Position = new Point(_databaseList.Position.x, (int)((_databaseScrollFrame.Size.y - _databaseList.Size.y) * _databaseSlider.EasedValue));
        }

        if (GuiHost.MouseScroll != 0 && sender.Desktop.HotControl != null && _databaseScrollFrame.Hit(GuiHost.MousePosition.x, GuiHost.MousePosition.y))
        {
            float realNumSteps = _biologProgress.UnlockedEntries.Count * (1 - (float)_databaseScrollFrame.Size.y / _databaseList.Size.y);
            _databaseSlider.Value += GuiHost.MouseScroll / realNumSteps;
        }
    }

    void entryButton_MouseClick(Control sender, MouseEventArgs args)
    {
        if (sender != _selectedBiologEntryControl)
        {
            AnalyticsLogger.Instance.AddLogEntry(new BiologEntrySelectedLogEntry(GameContext.Instance.Player.UserGuid, (sender.Tag as BiologEntry)));
        }
        _selectedBiologEntryControl = sender;

        BiologEntry entry = (sender.Tag as BiologEntry);
        InitializeNavigatorThumbnails(entry);
    }

    private void InitializeNavigatorThumbnails(BiologEntry entry)
    {
        _summaryTitle.Text = entry.EntryName;
        _scaleLabel.Text = entry.Scale;
        if (string.IsNullOrEmpty(_scaleLabel.Text))
        {
            _scaleFrame.Visible = false;
        }
        else
        {
            _scaleFrame.Visible = true;
        }

        if (entry.GalleryItems.Count == 0 || entry.GalleryItems[0].GalleryImage == null)
        {
            return;
        }
        _centerSlider.Value = 0;


        
        _navigatorPreviewFrame.Controls.Clear();

        //if (entry.Preview3D)
        //{
        //    _navigatorPreviewFrame.Controls.Add(Create3DPreviewButton(entry));
        //}

        foreach (BiologEntryGalleryItem galleryItem in entry.GalleryItems)
        {
            _navigatorPreviewFrame.Controls.Add(CreateNavigatorThumbnailButton(galleryItem));
        }

		if (_3dPreviewScene)
		{
			GameObject.Destroy(_3dPreviewScene);
		}
        //if (entry.Preview3D)
        //{
        //    _3dPreviewScene = (GameObject)GameObject.Instantiate(entry.Preview3D);
        //    (GuiHost.Renderer as UnityRenderer).InsertTexture(_3dPreviewTexture, "_3dPreviewTexture");
        //    _galleryImage.Texture = "_3dPreviewTexture";
        //    _galleryImage.TextureRect = new Rectangle(0, 0, _3dPreviewTexture.width, _3dPreviewTexture.height);
        //}
        //else
        //{
        //    //Texture texture = entry.GalleryItems[0].GalleryImage;
        //    //_galleryImage.Texture = entry.GalleryItems[0].GalleryImageResourcePath;
        //    //_galleryImage.TextureRect = new Rectangle(0, 0, texture.width, texture.height);
        //}



        _summaryLabel.Text = entry.DescriptionText;
        _summaryLabel.PerformLayout();
        _centerContentPage.PerformLayout();
    }

    private Button CreateBiologEntryButton(BiologEntry entry)
    {
        Button button = new Button();
        button.Tag = entry;

        button.Style = "Button - Transparent";
        //button.Tint = ColorInt.RGB(134, 226, 111);
        button.Dock = DockStyle.Top;
        button.Text = entry.EntryName;

        return button;
    }

    private Frame CreateNavigatorThumbnailButton(BiologEntryGalleryItem galleryItem)
    {
        Frame navigatorFrame = new Frame();
        navigatorFrame.Dock = DockStyle.Left;
        navigatorFrame.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        navigatorFrame.Margin = new Margin(0, 0, 0, 0);
        navigatorFrame.NoEvents = true;
        navigatorFrame.Size = new Point(100, 96);

        ImageControl backgroundImage = new ImageControl();
        //backgroundImage.Style = "Inventory - Grid";// "Image - Gallery Unselected Frame";
        backgroundImage.Tiling = TextureMode.Stretch;
        backgroundImage.Dock = DockStyle.Fill;
        backgroundImage.NoEvents = true;

        ImageControl previewImage = new ImageControl();
        previewImage.Tiling = TextureMode.Stretch;
        previewImage.Dock = DockStyle.Center;
        //previewImage.Margin = new Margin(4, 2, 4, 2);
        //previewImage.Texture = galleryItem.GalleryPreviewResourcePath;
        previewImage.NoEvents = true;

        Texture previewTexture = null;// galleryItem.GalleryPreview;
        if (previewTexture)
        {
            float scaleFactor;

            // Just assume we're always previewing the entire texture. Just need to figure out how big it should be:

            if (previewTexture.width > previewTexture.height)
            {
                scaleFactor = ((float)92) / previewTexture.width;
            }
            else
            {
                scaleFactor = ((float)92) / previewTexture.height;
            }

            previewImage.Size = new Point((int)(scaleFactor * previewTexture.width), (int)(scaleFactor * previewTexture.height));
        }
        

        Button previewButton = new Button();
        previewButton.Dock = DockStyle.Fill;
        previewButton.Style = "Inventory - Grid"; //"Image - Gallery Unselected Image";
        previewButton.Tint = ColorInt.ARGB(0.0f, 1.0f, 1.0f, 1.0f);

        previewButton.MouseClick +=
            (s, a) =>
            {
                //Texture texture = galleryItem.GalleryImage;
                //_galleryImage.Texture = galleryItem.GalleryImageResourcePath;
                //_galleryImage.TextureRect = new Rectangle(0, 0, texture.width, texture.height);
            };
        navigatorFrame.Controls.Add(backgroundImage);
        navigatorFrame.Controls.Add(previewImage);
        navigatorFrame.Controls.Add(previewButton);

        return navigatorFrame;
    }

    private Frame Create3DPreviewButton(BiologEntry entry)
    {
        Frame navigatorFrame = new Frame();
        navigatorFrame.Dock = DockStyle.Left;
        navigatorFrame.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        navigatorFrame.Margin = new Margin(0, 0, 0, 0);
        navigatorFrame.NoEvents = true;
        navigatorFrame.Size = new Point(100, 96);

        ImageControl backgroundImage = new ImageControl();
        backgroundImage.Style = "Image - Gallery Unselected Frame";
        backgroundImage.Tiling = TextureMode.Stretch;
        backgroundImage.Dock = DockStyle.Fill;
        backgroundImage.NoEvents = true;

        ImageControl previewImage = new ImageControl();
        previewImage.Tiling = TextureMode.Stretch;
        previewImage.Dock = DockStyle.Fill;
        previewImage.Margin = new Margin(1, 2, 1, 2);

        if (entry.GalleryItems.Count > 0)
        {
            Texture previewTexture = null;// entry.GalleryItems[0].GalleryPreview;
            if (previewTexture)
            {
                //previewImage.Texture = entry.GalleryItems[0].GalleryPreviewResourcePath;

                float scaleFactor;

                // Just assume we're always previewing the entire texture. Just need to figure out how big it should be:

                if (previewTexture.width > previewTexture.height)
                {
                    scaleFactor = ((float)92) / previewTexture.width;
                }
                else
                {
                    scaleFactor = ((float)92) / previewTexture.height;
                }

                previewImage.Size = new Point((int)(scaleFactor * previewTexture.width), (int)(scaleFactor * previewTexture.height));
            }
            
        }
        previewImage.NoEvents = true;

        Button previewButton = new Button();
        previewButton.Dock = DockStyle.Fill;
        previewButton.Style = "Inventory - Grid";
        previewButton.Tint = ColorInt.ARGB(0.0f, 1.0f, 1.0f, 1.0f);

        Label label3D = new Label();
        label3D.Dock = DockStyle.Fill;
        label3D.Text = "3D";
        label3D.TextAlign = Alignment.MiddleCenter;
        label3D.Style = "Label - Blurb";
        label3D.NoEvents = true;

        previewButton.MouseClick +=
            (s, a) =>
            {
                if (_3dPreviewScene)
                {
                    GameObject.Destroy(_3dPreviewScene);
                }
                //_3dPreviewScene = (GameObject)GameObject.Instantiate(entry.Preview3D);
                (GuiHost.Renderer as UnityRenderer).InsertTexture(_3dPreviewTexture, "_3dPreviewTexture");
                _galleryImage.Texture = "_3dPreviewTexture";
                _galleryImage.TextureRect = new Rectangle(0, 0, _3dPreviewTexture.width, _3dPreviewTexture.height);
            };

        navigatorFrame.Controls.Add(backgroundImage);
        navigatorFrame.Controls.Add(previewImage);
        navigatorFrame.Controls.Add(previewButton);
        navigatorFrame.Controls.Add(label3D);

        return navigatorFrame;
    }
}
