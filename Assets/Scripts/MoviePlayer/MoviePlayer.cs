#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.

using UnityEngine;
using System;

public class MoviePlayer : MonoBehaviour
{
    [SerializeField]
    private MoviePlayerSettings _moviePlayerSettings;

    private float alpha;
    private bool _hasStarted = false;
    private bool _hasStopped = false;

    public event Action OnMovieFinishedPlaying;

    public static MoviePlayer CreateMoviePlayer(MoviePlayerSettings moviePlayerSettings)
    {
        GameObject moviePlayerGO = new GameObject("MoviePlayer");
        MoviePlayer moviePlayer = moviePlayerGO.AddComponent<MoviePlayer>();
        moviePlayer._moviePlayerSettings = moviePlayerSettings;
        return moviePlayer;
    }

    public void StartMovie()
    {
        alpha = 0.0f;
        //iTween.ValueTo(gameObject, iTween.Hash("from", audio.volume, "to", 1.0f, "delay", 0.0f, "time", 1.0f, "easetype", iTween.EaseType.easeInExpo, "onupdate", "updateVolume"));
        //audio.Play();

        _moviePlayerSettings.Movie.Play();

        alpha = 1;
        //iTween.ValueTo(gameObject, iTween.Hash("from", alpha, "to", 1.0f, "delay", 0.0f, "time", 0.0f, "easetype", iTween.EaseType.linear, "onupdate", "updateAlpha"));
        //iTween.ValueTo(gameObject, iTween.Hash("from", audio.volume, "to", 1.0f, "delay", 0.0f, "time", 2.0f, "easetype", iTween.EaseType.linear, "onupdate", "updateVolume"));

        _hasStarted = true;
    }

    public void StopMovie()
    {
        _moviePlayerSettings.Movie.Stop();
    }

    void OnGUI()
    {
        if (_hasStarted)
        {
            //GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
            GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(Screen.width / (1280.0f), Screen.height / (720.0f), 1));
            GUI.DrawTexture(new Rect(0, 0, 1280, 720), _moviePlayerSettings.Movie);
            //if (GUI.Button(new Rect(15, 660, 134, 45), "Skip Movie", "SkipButton"))
            //{
            //    startFadeOut();
            //}

            if (!_moviePlayerSettings.Movie.isPlaying)
            {
                _hasStopped = true;
            }
        }
    }

    private void startFadeOut()
    {
        alpha = 0;
        //iTween.ValueTo(gameObject, iTween.Hash("from", alpha, "to", 0.0f, "delay", 0.0f, "time", 0.0f, "easetype", iTween.EaseType.linear, "onupdate", "updateAlpha", "oncomplete", "OnComplete"));
        //iTween.ValueTo(gameObject, iTween.Hash("from", audio.volume, "to", 0.0f, "delay", 0.0f, "time", 2.0f, "easetype", iTween.EaseType.linear, "onupdate", "updateVolume"));
    }

    private void updateAlpha(float input)
    {
        alpha = input;
    }

    private void updateVolume(float input)
    {
        //audio.volume = input;
    }

    void Update()
    {
        if (_hasStarted && _hasStopped)
        {
            OnComplete();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StopMovie();
        }
    }

    void OnComplete()
    {
        if (OnMovieFinishedPlaying != null)
        {
            OnMovieFinishedPlaying();
        }
    }
}