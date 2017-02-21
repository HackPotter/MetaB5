using System;
using Squid;
using UnityEngine;
using Metablast.UI;

public class MetablastUI : GuiRenderer
{
    public static MetablastUI Instance
    {
        get;
        private set;
    }

    public NewBiologUI BiologUi;
    public HudView HudViewUI;

    // TODO
    public DialogueView DialogueUI;
    public Metablast.UI.QuestionView QuestionUI;
    
    //private Desktop _questionDesktop;
    
    //private QuestionView _questionView;
    

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null)
        {
            DebugFormatter.LogError(this, "MetablastUI is a singleton, but multiple instances exist.");
            Destroy(this);
            return;
        }
        Instance = this;
        
        //_questionDesktop = QuestionLayout.GetInstance();
        //_questionView = new QuestionView(_questionDesktop);
        //_questionView.Visible = false;
        //_questionView.Dock = DockStyle.Fill;


        BiologUi.ExitButtonPressed += BiologView_ExitButtonPressed;
        HudView.BiologButtonPressed += HudView_BiologButtonPressed;
        
        //Desktop.Controls.Add(_questionView);
    }
    
    void OnEnable()
    {
        GameContext.Instance.Player.BiologProgress.BiologEntryScanned += BiologProgress_BiologEntryScanned;
    }
	
	void OnDisable()
	{
		GameContext.Instance.Player.BiologProgress.BiologEntryScanned -= BiologProgress_BiologEntryScanned;
	}
	

    void BiologProgress_BiologEntryScanned(BiologEntry unlockedEntry, bool notify)
    {
        if (notify)
        {
            HudView.Hide(() => BiologView.Show(() => { }));
        }
    }

    void HudView_BiologButtonPressed()
    {
        HudViewUI.Hide(() => BiologView.Show(() => { }));
    }

    void BiologView_ExitButtonPressed()
    {
        BiologView.Hide(() => HudViewUI.Show(() => { }));
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && BiologView.IsShowing)
        {
            BiologView.Hide(() => HudViewUI.Show(() => { }));
        }
    }

    public IHudView HudView
    {
        get { return HudViewUI; }
    }

    public IBiologView BiologView
    {
        get { return BiologUi; }
    }

    public IDialogueView DialogueFrameView
    {
        get { return DialogueUI; }
    }

    public IQuestionView QuestionView
    {
        get { return QuestionUI; }
    }

    public void Destroy()
    {
        GameObject.Destroy(this.gameObject);
        Instance = null;
    }


}

