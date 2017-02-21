
public class GameData : IGameData
{
    public BiologData BiologData
    {
        get;
        private set;
    }

    public ItemDatabase ItemDatabase
    {
        get;
        private set;
    }

    public QuestionDatabase QuestionDatabase
    {
        get;
        private set;
    }

    public GameData()
    {
        BiologData = ResourcesExt.Load<BiologData>("Biolog/Database/BiologData_Converted");
        ItemDatabase = ResourcesExt.Load<ItemDatabase>("Inventory/Database/InventoryDatabase");
        QuestionDatabase = ResourcesExt.Load<QuestionDatabase>("Questions/Database/QuestionDatabase");
    }
}

