using System;
using ExitGames.Client.Photon;
using MetablastServer.Codes;

public enum WriteDataLogResponse
{
    Success,
    InternalError,
}
   
public delegate void WriteDataLogResponseReceivedHandler(WriteDataLogResponse code);

	public class WriteDataLogResponseHandler : IResponseHandler
	{
        public event WriteDataLogResponseReceivedHandler DataLogReceived;




	
public RequestOpCode OperationCode
{
	get { return RequestOpCode.WriteLogEntry; }
}

public void  HandleResponse(OperationResponse response)
{
    switch ((ResponseReturnCode)response.ReturnCode)
    {
        case ResponseReturnCode.OK:
            DataLogReceived(WriteDataLogResponse.Success);
            break;
        case ResponseReturnCode.InvalidParameters:
        case ResponseReturnCode.Error:
            DataLogReceived(WriteDataLogResponse.InternalError);
            break;
    }

}
}
