using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class PersistentPlayerDataProvider : IPlayerDataProvider
{
    public IPlayer PlayerData
    {
        get { throw new NotImplementedException(); }
    }

    public IGameData GameData
    {
        get { throw new NotImplementedException(); }
    }
}

