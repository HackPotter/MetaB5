
using UnityEngine;
public interface IOrderable
{
    int Ordinal { get; set; }

    GameObject gameObject { get; }
}

