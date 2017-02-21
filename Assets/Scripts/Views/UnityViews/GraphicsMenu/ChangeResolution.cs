using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeResolution : MonoBehaviour
{
#pragma warning disable 0067, 0649
    [SerializeField]
    GameObject prefab;
    [SerializeField]
    Transform parentObject;
#pragma warning restore 0067, 0649

    public bool showing;
  
    public void ShowResolutions()
    {
        if (!showing)
        {

            for (int i = 0; i < Screen.resolutions.Length; i++)
            {
                GameObject button = (GameObject)Instantiate(prefab);
                button.GetComponentInChildren<Text>().text = ResToString(Screen.resolutions[i]);

                int index = i;
                // CR: So your prefab already has a button component on it.
                //      When this code executes, it just complains that you can't add another Button.
                button.AddComponent<Button>();

                // CR: I'm impressed that you use a lambda expression here and even more
                //          impressed that you avoided capturing the loop variable in a closure
                //          How did you know to do that?
                button.GetComponent<Button>().onClick.AddListener(
                    () => { SetResolution(index); }
                    );
                button.transform.SetParent(parentObject);
            }
        }
    }

    void SetResolution(int index)
    {
        Screen.SetResolution(Screen.resolutions[index].width, Screen.resolutions[index].height, Screen.fullScreen);
    }

    string ResToString(Resolution res)
    {
        return res.width + " x " + res.height;
    }

    public void SetShowing(bool set)
    {
        showing = set;
    }
}
