using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

//General script for MainMenu functionalities
public class MainMenu : MonoBehaviour
{
    private Button newGameButton;
    private Button joinGameButton;

    private void Start()
    {
        //creates a PhotonManager
        if (PhotonManager.instance == null)
            Instantiate(Resources.Load<GameObject>("Managers/PhotonManager"));           

        AudioManager.instance.PlayMainMenuMusic();
        StartCoroutine("SwitchVROff");

        newGameButton = GameObject.Find("NewGameButton").GetComponent<Button>();
        joinGameButton = GameObject.Find("JoinGameButton").GetComponent<Button>();
    }

    //Switch the game back to 2D mode
    private IEnumerator SwitchVROff()
    {
        XRSettings.LoadDeviceByName("None");
        yield return null;
        XRSettings.enabled = false;
    }

    //disable the NewGame / JoinGame buttons wether the connection is not available
    private void Update()
    {
        if (PhotonNetwork.connected)
        {
            newGameButton.interactable = true;
            joinGameButton.interactable = true;
        }

        else
        {
            newGameButton.interactable = false;
            joinGameButton.interactable = false;
        }
    }


}
