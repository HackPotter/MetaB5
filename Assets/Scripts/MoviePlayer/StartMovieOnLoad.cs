using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(MoviePlayer))]
public class StartMovieOnLoad : MonoBehaviour {

    // Use this for initialization
    void Start() {
        GetComponent<MoviePlayer>().OnMovieFinishedPlaying += new System.Action(StartMovieOnLoad_OnMovieFinishedPlaying);
        GetComponent<MoviePlayer>().StartMovie();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            GetComponent<MoviePlayer>().StopMovie();
        }
    }

    void StartMovieOnLoad_OnMovieFinishedPlaying() {
        SceneManager.LoadScene("MainMenu");
    }
}
