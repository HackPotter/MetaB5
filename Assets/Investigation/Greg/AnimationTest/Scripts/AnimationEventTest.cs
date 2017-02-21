using UnityEngine;
using System.Collections;

public class AnimationEventTest : MonoBehaviour
{
    public void Test(float val)
    {
        DebugFormatter.Log(this, "" + val);
    }
}
