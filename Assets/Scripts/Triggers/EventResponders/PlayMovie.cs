using System;
using UnityEngine;

[Trigger(DisplayPath = "UI")]
public class PlayMovie : EventResponder
{
#pragma warning disable 0067, 0649
    [SerializeField]
    private MoviePlayerSettings _moviePlayerSettings;
#pragma warning restore 0067, 0649

    private MoviePlayer _moviePlayer;

    public event Action OnMovieComplete;

    public override void OnEvent(ExecutionContext context)
    {
        _moviePlayer = MoviePlayer.CreateMoviePlayer(_moviePlayerSettings);
        _moviePlayer.OnMovieFinishedPlaying += new Action(moviePlayer_OnMovieFinishedPlaying);
        _moviePlayer.StartMovie();
    }

    void moviePlayer_OnMovieFinishedPlaying()
    {
        if (OnMovieComplete != null)
        {
            OnMovieComplete();
        }

        GameObject.Destroy(_moviePlayer.gameObject);
    }
}

