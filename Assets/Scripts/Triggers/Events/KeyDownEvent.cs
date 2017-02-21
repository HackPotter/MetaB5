using UnityEngine;

[Trigger(Description = "Invoked when the player has pressed the given key.\nThis event will only be invoked the first frame the key has been pressed.", DisplayPath="Input")]
[AddComponentMenu("Metablast/Triggers/Events/Input/Key Down Event")]
public class KeyDownEvent : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The key for which this event will be invoked when pressed.")]
    private KeyCode _inputKey;
#pragma warning restore 0067, 0649

    void Update()
    {
        if (Input.GetKeyDown(_inputKey))
        {
            TriggerEvent();
        }
    }
}

