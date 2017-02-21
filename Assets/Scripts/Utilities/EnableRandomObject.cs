using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class EnableRandomObject : MonoBehaviour
{
    public GameObject[] GameObjects;


    void Awake()
    {
        int index = UnityEngine.Random.Range(0, GameObjects.Length);

        foreach (var gameObject in GameObjects)
        {
            gameObject.SetActive(false);
        }
        GameObjects[index].SetActive(true);
    }
}

