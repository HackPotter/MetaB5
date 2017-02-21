using ExitGames.Client.Photon;
using MetablastServer.Codes;
public enum LoginResponse
{
    Success,
    InvalidCredentials,
    InternalError,
    UserAlreadyLoggedIn,
}
public delegate void LoginResponseReceivedHandler(LoginResponse code);
public class LoginResponseHandler : IResponseHandler
{
    public event LoginResponseReceivedHandler ResponseReceived;


    public RequestOpCode OperationCode
    {
        get { return RequestOpCode.Login; }
    }

    public void HandleResponse(OperationResponse response)
    {
        switch ((ResponseReturnCode)response.ReturnCode)
        {
            case ResponseReturnCode.OK:
                ResponseReceived(LoginResponse.Success);
                break;
            case ResponseReturnCode.InvalidParameters:
                ResponseReceived(LoginResponse.InternalError);
                break;
            case ResponseReturnCode.Error:
                if ((LoginUserErrorDetail)response.Parameters[(byte)CommonResponseParameters.ErrorDetailCode] == LoginUserErrorDetail.InvalidCredentials)
                {
                    ResponseReceived(LoginResponse.InvalidCredentials);
                }
                if ((LoginUserErrorDetail)response.Parameters[(byte)CommonResponseParameters.ErrorDetailCode] == LoginUserErrorDetail.UserAlreadyLoggedIn)
                {
                    ResponseReceived(LoginResponse.UserAlreadyLoggedIn);
                }
                break;
        }
    }


}

