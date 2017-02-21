using ExitGames.Client.Photon;
using MetablastServer.Codes;

public interface IResponseHandler
{
    RequestOpCode OperationCode { get; }

    void HandleResponse(OperationResponse response);

}

