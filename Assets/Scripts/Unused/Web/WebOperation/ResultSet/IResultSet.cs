using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IResultSet
{
    string RootLabel
    {
        get;
    }

    string[] Keys
    {
        get;
    }

    string GetString(int index);
    string GetString(string key);

    DateTime GetDateTime(int index);
    DateTime GetDateTime(string key);

    int GetInteger(int index);
    int GetInteger(string key);
}

