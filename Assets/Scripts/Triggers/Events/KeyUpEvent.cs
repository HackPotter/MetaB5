using UnityEngine;

[Trigger(Description = "Invoked when the player has released the given key.\nThis event will only be invoked the first frame the key has been released.", DisplayPath = "Input")]
[AddComponentMenu("Metablast/Triggers/Events/Input/Key Up Event")]
public class KeyUpEvent : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The key for which this event will be invoked when released.")]
    private KeyCode _inputKey;
#pragma warning restore 0067, 0649

    void Update()
    {
        if (Input.GetKeyUp(_inputKey))
        {
            TriggerEvent();
        }
    }
}

