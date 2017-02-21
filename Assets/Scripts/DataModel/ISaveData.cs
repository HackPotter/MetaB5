using System.Collections.Generic;

public interface ISaveData
{
    Dictionary<string, string> Data { get; }
    void Write();
}

