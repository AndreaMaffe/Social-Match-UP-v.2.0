using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour {

    public static EventManager instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void OnBackToMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnOpenGamesButtonClicked()
    {
        SceneManager.LoadScene("OpenGames");
    }

    public void OnNewgameButtonClicked()
    {
        SceneManager.LoadScene("GameModeSelection");
    }
}
