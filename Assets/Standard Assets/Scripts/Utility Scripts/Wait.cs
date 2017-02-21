using UnityEngine;
using System.Collections;

public class Wait : MonoBehaviour
{

    public static IEnumerator ForSecondsIgnoreTimescale(float time)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < time)
        {
            yield return null;
        }
    }

}
