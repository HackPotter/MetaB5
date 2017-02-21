using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public class NumericalData
{
    public Dictionary<string, int> Values { get; private set; }

    public NumericalData()
    {
        Values = new Dictionary<string, int>();
    }
}

