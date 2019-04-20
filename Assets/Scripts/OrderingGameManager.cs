using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderingGameManager : MonoBehaviour
{
    private GameObject[] players;
    private GameObject thisPlayer;
    private int numberOfObjects;

    private void Start()
    {
        AudioManager.instance.PlayBackgroundMusic();
        StartCoroutine("SetUpGame");
        numberOfObjects = PhotonManager.instance.NumberOfImages;
        Debug.Log("Number: " + numberOfObjects);
    }

    //initial setup, called at task launch
    IEnumerator SetUpGame()
    {
        yield return new WaitForSeconds(8); //wait a bit to give to each client the time to instantiate their player (otherwise only one player is found)

        //find the players in the scene
        players = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (GameObject player in players)
            if (player.GetPhotonView().isMine)
                thisPlayer = player.gameObject;

        gameObject.GetPhotonView().RPC("SpawnObjects", PhotonTargets.All, numberOfObjects);
        SpawnAnchorPoints();
    }

    [PunRPC]
    public void StartVictoryAnimations()
    {
        StartCoroutine(OnVictory());
    }

    IEnumerator OnVictory()
    {
        yield return new WaitForSeconds(1);
        AudioManager.instance.PlayHurraySound();
        yield return new WaitForSeconds(3);
        AudioManager.instance.StopBackgroundMusic();
        yield return new WaitForSeconds(2);
        AudioManager.instance.PlayVictorySound();

        SpriteRenderer endGamePanel = thisPlayer.transform.Find("BlackPanel").GetComponent<SpriteRenderer>();
        endGamePanel.color = Color.black;
    }

    void SpawnAnchorPoints()
    {
        for (int i=0; i<numberOfObjects; i++)
        {
            PhotonNetwork.Instantiate("AnchorPoint", new Vector3(numberOfObjects * -0.75f + 0.75f + 1.5f * i, 1.7f, 7), Quaternion.identity, 0);
            PhotonNetwork.Instantiate("AnchorPoint", new Vector3(numberOfObjects * -0.75f + 0.75f + 1.5f * i, 1.7f, -7), Quaternion.identity, 0);
        }
    }

    [PunRPC]
    void SpawnObjects(int numberOfObjects)
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            PhotonNetwork.Instantiate("Balloons/Balloon" + i, thisPlayer.transform.position + Random.onUnitSphere, Quaternion.identity, 0);
        }
    }
}
