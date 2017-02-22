using Squid;
using UnityEngine;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class MetablastMenuUI : GuiRenderer {
#pragma warning disable 0067, 0649
    [SerializeField]
    private SquidLayout _launchScreen;
    [SerializeField]
    private SquidLayout _loginScreen;
    [SerializeField]
    private SquidLayout _registerScreen;
#pragma warning restore 0067, 0649

    private bool _sendRegistration = false;

    private MenuView _menuView;
    private LoginView _loginView;
    private RegisterView _registerView;

    public IMenuView MenuView {
        get { return _menuView; }
    }

    public ILoginView LoginView {
        get { return _loginView; }
    }

    public IRegisterView RegisterView {
        get { return _registerView; }
    }

    protected override void Awake() {
        base.Awake();

        _menuView = new MenuView(_launchScreen);
        _menuView.Dock = DockStyle.Fill;

        _loginView = new LoginView(_loginScreen);
        _loginView.Dock = DockStyle.Fill;
        _loginView.Visible = false;

        _registerView = new RegisterView(_registerScreen);
        _registerView.Dock = DockStyle.Fill;
        _registerView.Visible = false;

        Desktop.Controls.Add(_menuView);
        Desktop.Controls.Add(_loginView);
        Desktop.Controls.Add(_registerView);

        MenuView.ExitButtonPressed += new System.Action(MenuView_ExitButtonPressed);
        MenuView.LoginButtonPressed += new System.Action(MenuView_LoginButtonPressed);
        MenuView.PlayButtonPressed += new System.Action(MenuView_PlayButtonPressed);

        LoginView.LoginButtonPressed += new System.Action(LoginView_LoginButtonPressed);
        LoginView.RegisterButtonPressed += new System.Action(LoginView_RegisterButtonPressed);
        LoginView.BackButtonPressed += new System.Action(LoginView_BackButtonPressed);
        LoginView.ResetPasswordButtonPressed += new System.Action(LoginView_ResetPasswordButtonPressed);

        RegisterView.BackButtonPressed += new System.Action(RegisterView_BackButtonPressed);
        RegisterView.SubmitButtonPressed += new System.Action(RegisterView_SubmitButtonPressed);

        NetworkManager.Instance.Connected += Instance_Connected;
        NetworkManager.Instance.Disconnected += Instance_Disconnected;
        NetworkManager.Instance.EncryptionEstablished += Instance_EncryptionEstablished;
        NetworkManager.Instance.GetHandler<RegisterResponseHandler>().ResponseReceived += MetablastMenuUI_ResponseReceived;

    }

    void RegisterView_SubmitButtonPressed() {
        _sendRegistration = true;
        if (NetworkManager.Instance.ConnectionStatus == PeerStateValue.Disconnected) {
            //NetworkManager.Instance.Connect("10.25.109.228:5055", "MetablastServer");
        }
    }

    void Instance_Connected(StatusCode statusCode) {
        NetworkManager.Instance.EstablishEncryption();
    }

    void Instance_EncryptionEstablished(StatusCode statusCode) {
        if (_sendRegistration) {
            NetworkManager.Instance.SendRequest(new Register(RegisterView.Email, RegisterView.Password, RegisterView.Age, RegisterView.CollectData));
        }
    }

    void Instance_Disconnected(StatusCode statusCode) {
        //Debug.Log("Disconnected!");
    }

    void MetablastMenuUI_ResponseReceived(RegisterResponse code) {
        Debug.Log(code.ToString());

        if (code == RegisterResponse.Success) {
            _registerView.Visible = false;
            _loginView.Visible = true;
        }
        NetworkManager.Instance.Disconnect();
    }



    void RegisterView_BackButtonPressed() {
        _registerView.Visible = false;
        _loginView.Visible = false;
        _menuView.Visible = true;
    }

    void LoginView_ResetPasswordButtonPressed() {
        // TODO not really a way to do this??
    }

    void LoginView_LoginButtonPressed() {
        // TODO attempt to login.
    }

    void LoginView_RegisterButtonPressed() {
        _loginView.Visible = false;
        _registerView.Visible = true;
    }

    void LoginView_BackButtonPressed() {
        _loginView.Visible = false;
        _menuView.Visible = true;
    }

    void MenuView_LoginButtonPressed() {
        _menuView.Visible = false;
        _loginView.Visible = true;
    }

    void MenuView_PlayButtonPressed() {
        //Debug.Log("Play Button Pressed");
        SceneManager.LoadScene("SceneLoader");
    }

    void MenuView_ExitButtonPressed() {
        Application.Quit();
    }
}