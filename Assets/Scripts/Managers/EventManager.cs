using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

//"Support" manager: provides methods to be called when some events in the game occur.
public class EventManager : MonoBehaviour
{
    public void OnBackToMainMenuButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
        if (PhotonNetwork.inRoom)
            PhotonNetwork.LeaveRoom();
    }

    public void OnOpenGamesButtonClicked()
    {
        SceneManager.LoadScene("JoinGameMenu");
    }

    public void OnNewgameButtonClicked()
    {
        SceneManager.LoadScene("NewGameMenu");
    }

    public void OnChangingRoomButtonClicked()
    {
        SceneManager.LoadScene("ChangingRoom");
    }


}
