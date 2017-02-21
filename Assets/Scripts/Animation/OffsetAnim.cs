using UnityEngine;

public class OffsetAnim : MonoBehaviour
{
	public string ClipName;

    void Start()
    {
        GetComponent<Animation>()[ClipName].time = Random.Range(0.0f, GetComponent<Animation>()[ClipName].length * 2);
    }
}