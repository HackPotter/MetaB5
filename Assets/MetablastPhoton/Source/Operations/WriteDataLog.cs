using System.Collections.Generic;
using MetablastServer.Codes;

class WriteDataLog : BaseRequest
{
    private  string _data; 
    private int _type;

    public WriteDataLog(string data, int type)
    {
        _data = data;
        _type = type;
    }
       
    public override bool  IsReliable 
    {
	    get { return true; }
    }

    public override bool  Encrypt
    {
	    get { return true; }
    }

    public override byte  ChannelId
    {
	    get { return 0; }
    }

    protected override RequestOpCode  RequestOpCode
    {
	    get { return RequestOpCode.WriteLogEntry; }
    }

    protected override void  PopulateParameters(Dictionary<byte,object> parameters)
    {
        parameters[(byte)WriteLogEntryParameters.Data] = _data;
        parameters[(byte)WriteLogEntryParameters.LogEntryType] = _type;
    }
}
