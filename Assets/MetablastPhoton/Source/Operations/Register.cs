using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MetablastServer.Codes;

public class Register : BaseRequest
{
    private string _email;
    private string _pw;
    //private _sex;
    private int _age;
    //private _education;
    private bool _analyticsEnabled;

    public Register(string email, string pw, int age, bool analyticsEnabled)
    {
        _email = email;
        _pw = pw;
        _age = age;
        _analyticsEnabled = analyticsEnabled;
    }

    protected override void PopulateParameters(Dictionary<byte, object> D)
    {
        D[(byte)RegisterUserParameters.Email] = _email;
        D[(byte)RegisterUserParameters.Password] = _pw;
        D[(byte)RegisterUserParameters.Age] = _age;
        D[(byte)RegisterUserParameters.AnalyticsEnabled] = _analyticsEnabled;
    }

    protected override RequestOpCode RequestOpCode
    {
        get { return RequestOpCode.Register; }
    }

    public override bool IsReliable
    {
        get { return true; }
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

