using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class MainMenu : MonoBehaviour
{
    private Button newGameButton;
    private Button joinGameButton;

    private void Start()
    {
        AudioManager.instance.PlayMainMenuMusic();
        StartCoroutine("SwitchVROff");

        newGameButton = GameObject.Find("NewGameButton").GetComponent<Button>();
        joinGameButton = GameObject.Find("JoinGameButton").GetComponent<Button>();
    }

    private IEnumerator SwitchVROff()
    {
        XRSettings.LoadDeviceByName("None");
        yield return null;
        XRSettings.enabled = false;
    }

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
