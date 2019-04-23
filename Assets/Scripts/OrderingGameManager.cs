using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderingGameManager : MonoBehaviour
{
    private GameObject[] players;
    private GameObject thisPlayer;
    private int numberOfObjects;
    private int[] combinationPlayer1;
    private int[] combinationPlayer2;

    private void Start()
    {
        AudioManager.instance.PlayBackgroundMusic();

        //synchronize the value of numberOfObjects between all clients (otherwise only the player who creates the room has it)
        if (gameObject.GetPhotonView().isMine)
            gameObject.GetPhotonView().RPC("SetNumberOfObjects", PhotonTargets.All, PhotonManager.instance.NumberOfImages);
    }

    private void Update()
    {
        if ( (players==null || players.Length <2) && numberOfObjects != 0)
        {
            players = GameObject.FindGameObjectsWithTag("MainCamera");

            if (players.Length==2)
            {
                Debug.Log("Giocatori trovati");

                foreach (GameObject player in players)
                    if (player.GetPhotonView().isMine)
                        thisPlayer = player.gameObject;

                SpawnObjects();

                if (this.gameObject.GetPhotonView().isMine)
                {
                    combinationPlayer1 = new int[numberOfObjects];
                    combinationPlayer2 = new int[numberOfObjects];

                    SpawnAnchorPoints();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("PLAYER 1 : " + combinationPlayer1[0] + ", " + combinationPlayer1[1] + ", " + combinationPlayer1[2]);
            Debug.Log("PLAYER 2 : " + combinationPlayer2[0] + ", " + combinationPlayer2[1] + ", " + combinationPlayer2[2]);
        }
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

    void SpawnObjects()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            GameObject draggableObject = PhotonNetwork.Instantiate("Balloon", thisPlayer.transform.position + Random.onUnitSphere, Quaternion.identity, 0);
            draggableObject.GetPhotonView().RPC("SetIndex", PhotonTargets.All, i);
        }
    }

    void SpawnAnchorPoints()
    {
        for (int i=0; i<numberOfObjects; i++) //creates the Anchor Point
        {
            GameObject anchorPointPlayer1 = PhotonNetwork.Instantiate("AnchorPoint", new Vector3(numberOfObjects * -0.75f + 0.75f + 1.5f * i, 1.7f, 7), Quaternion.identity, 0);
            anchorPointPlayer1.GetPhotonView().RPC("SetIndex", PhotonTargets.All, i);
            GameObject anchorPointPlayer2 = PhotonNetwork.Instantiate("AnchorPoint", new Vector3(numberOfObjects * -0.75f + 0.75f + 1.5f * i, 1.7f, -7), Quaternion.identity, 0);
            anchorPointPlayer2.GetPhotonView().RPC("SetIndex", PhotonTargets.All, numberOfObjects - i -1);
        }
    }

    [PunRPC]
    public void OnObjectPositioned(int playerId, int objectIndex, int anchorPointIndex)
    {
        if (playerId == this.gameObject.GetPhotonView().ownerId)
            combinationPlayer1[anchorPointIndex] = objectIndex+1;
        else combinationPlayer2[anchorPointIndex] = objectIndex+1;

        Debug.Log("PLAYER 1 : " + combinationPlayer1[0] + ", " + combinationPlayer1[1] + ", " + combinationPlayer1[2]);
        Debug.Log("PLAYER 2 : " + combinationPlayer2[0] + ", " + combinationPlayer2[1] + ", " + combinationPlayer2[2]);

        bool sameCombination = true;

        for (int i = 0; i < numberOfObjects; i++)
            if (combinationPlayer1[i] != combinationPlayer2[i] || combinationPlayer1[i]==0)
                sameCombination = false;

        if (sameCombination)
            this.gameObject.GetPhotonView().RPC("StartVictoryAnimations", PhotonTargets.All);
    }

    [PunRPC]
    public void OnObjectRemoved(int playerId, int anchorPointIndex)
    {
        if (playerId == this.gameObject.GetPhotonView().ownerId)
            combinationPlayer1[anchorPointIndex] = 0;
        else combinationPlayer2[anchorPointIndex] = 0;

        Debug.Log("PLAYER 1 : " + combinationPlayer1[0] + ", " + combinationPlayer1[1] + ", " + combinationPlayer1[2]);
        Debug.Log("PLAYER 2 : " + combinationPlayer2[0] + ", " + combinationPlayer2[1] + ", " + combinationPlayer2[2]);
    }

    [PunRPC]
    void SetNumberOfObjects(int number)
    {
        numberOfObjects = number;
    }

}
