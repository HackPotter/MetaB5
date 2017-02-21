using UnityEngine;

[Trigger(Description = "Plays the given audio clip at the given audio position at the given volume.", DisplayPath = "Audio")]
public class PlayAudioClipOneShot : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The 3d world position at which the audio clip will be played.")]
    private Vector3 _audioPosition;

    [SerializeField]
    [Infobox("The clip that will be played.")]
    private AudioClip _audioClip;

    [SerializeField]
    [Infobox("The volume that the clip will be played at.")]
    [Range(0, 1)]
    private float _volume = 1.0f;
#pragma warning restore 0067, 0649

    public override void OnEvent(ExecutionContext context)
    {
        AudioSource.PlayClipAtPoint(_audioClip, _audioPosition, _volume);
    }
}

