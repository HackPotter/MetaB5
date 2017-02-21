using UnityEngine;
using System.Collections;

public class PutObjectAtOrigin : MonoBehaviour
{
    void Awake()
    {
        transform.position = new Vector3(0,0,0);
        transform.rotation = Quaternion.identity;
    }
}
