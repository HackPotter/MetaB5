
public interface IPersistentDataStorage : IDataStorage
{
    void ReadData();
    void WriteData();
    void ClearData();
}
