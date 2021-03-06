﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Parent class of all different GameManagers. Provides the basic methods to setup the game correctly when both players are
//ready and correctly instantiated over the network, and to close the game correctly when the match is over.
public abstract class GameManager : MonoBehaviour
{
    protected GameObject[] players;
    protected GameObject thisPlayer;

    private void Update()
    {
        if (players == null || players.Length < 2)
        {
            players = GameObject.FindGameObjectsWithTag("MainCamera");

            if (players.Length == 2)
            {
                Debug.Log("Giocatori trovati");

                foreach (GameObject player in players)
                    if (player.GetPhotonView().isMine)
                        thisPlayer = player.gameObject;

                SetUpGame();
            }
        }
    }

    protected abstract void SetUpGame(); 

    [PunRPC]
    public void StartVictoryAnimations()
    {
        StartCoroutine(OnVictory());
    }

    protected IEnumerator OnVictory()
    {
        yield return new WaitForSeconds(1);
        AudioManager.instance.PlayHurraySound();
        yield return new WaitForSeconds(3);
        AudioManager.instance.StopBackgroundMusic();
        yield return new WaitForSeconds(2);
        AudioManager.instance.PlayVictorySound();

        SpriteRenderer endGamePanel = thisPlayer.transform.Find("BlackPanel").GetComponent<SpriteRenderer>();
        endGamePanel.color = Color.black;

        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("MainMenu");

        PhotonNetwork.LeaveRoom();
    }

}
