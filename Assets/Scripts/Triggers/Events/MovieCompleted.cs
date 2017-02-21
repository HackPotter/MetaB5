using System;
using UnityEngine;

[Trigger(Description = "Plays a movie.")]
public class MovieCompleted : EventSender
{
#pragma warning disable 0067, 0649
    [SerializeField]
    [Infobox("The movie that will be played.")]
    private PlayMovie _playMovie;
#pragma warning restore 0067, 0649

    protected override void OnAwake()
    {
        base.OnAwake();

        _playMovie.OnMovieComplete += new Action(_playMovie_OnMovieComplete);
    }

    void _playMovie_OnMovieComplete()
    {
        TriggerEvent();
    }
}

