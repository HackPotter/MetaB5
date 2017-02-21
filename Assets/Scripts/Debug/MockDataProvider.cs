using System.IO;

public class MockDataProvider : IPlayerDataProvider
{
    private BiologLoadingProcess _biologLoadingProcess;

    public MockDataProvider(IGameData gameData, BiologLoadingProcess biologLoadingProcess)
    {
        _biologLoadingProcess = biologLoadingProcess;
        GameData = gameData;
        PlayerData = GetPlayerData();
    }

    private IPlayer GetPlayerData()
    {
        Player player = new Player();
        player.PlayerName = "Mock Player";
        player.CurrentObjectives = new UserObjectives();
        player.PersistentStorage = new PersistentDataStorage("SaveData.txt");
        player.PersistentStorage.ReadData();
        player.SessionStorage = new SessionDataStorage();
        player.Tools = new EquippedTools();
        player.BiologProgress = new BiologProgress(GameData, _biologLoadingProcess);
        player.QuestionProgress = new PlayerQuestionProgress();
        player.GameplayObjectManager = new GameplayObjectManager();

        return player;
    }

    public IGameData GameData
    {
        get;
        private set;
    }

    public IPlayer PlayerData
    {
        get;
        private set;
    }
}