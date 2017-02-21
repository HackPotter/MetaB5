using ExitGames.Client.Photon;
using MetablastServer.Codes;

public enum RegisterResponse
{
    Success,
    InternalError,
    UserAlreadyRegistered,
}
public delegate void RegisterResponseReceivedHandler(RegisterResponse code);

public class RegisterResponseHandler : IResponseHandler
{

    public event RegisterResponseReceivedHandler ResponseReceived;


    public RequestOpCode OperationCode
    {
        get { return RequestOpCode.Register; }
    }

    public void HandleResponse(OperationResponse response)
    {
        switch ((ResponseReturnCode)response.ReturnCode)
        {
            case ResponseReturnCode.OK:
                ResponseReceived(RegisterResponse.Success);
                break;
            case ResponseReturnCode.InvalidParameters:
                ResponseReceived(RegisterResponse.InternalError);
                break;
            case ResponseReturnCode.Error:
                ResponseReceived(RegisterResponse.UserAlreadyRegistered);
                break;

        }
    }
}

