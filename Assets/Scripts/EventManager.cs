using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{

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
