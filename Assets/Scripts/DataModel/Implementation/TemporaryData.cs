using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class TemporaryData : ITemporaryData
{
    private Dictionary<string, object> _data = new Dictionary<string, object>();

    public Dictionary<string, object> Data
    {
        get { return _data; }
    }
}

