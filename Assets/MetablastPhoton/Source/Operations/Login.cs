using System.Collections.Generic;
using MetablastServer.Codes;

public class Login : BaseRequest
{
    private string _email;
    private string _pw;

    public Login(string email, string pw)
    {
        _email = email;
        _pw = pw;
    }


    protected override void PopulateParameters(Dictionary<byte, object> D)
    {
        D[(byte)LoginRequestParameters.Email] = _email;
        D[(byte)LoginRequestParameters.Password] = _pw;
    }

    public override bool IsReliable
    {
        get { return true; }
    }
    protected override RequestOpCode RequestOpCode
    {
        get { return RequestOpCode.Login; }
    }

    public override bool Encrypt
    {
        get { return true; }
    }

    public override byte ChannelId
    {
        get { return 0; }
    }
}

