using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface INumericalData
{
    void InitVariable(string variableName, int value);
    void IncrementVariable(string variableName);
    void DecrementVariable(string variableName);
    
}

