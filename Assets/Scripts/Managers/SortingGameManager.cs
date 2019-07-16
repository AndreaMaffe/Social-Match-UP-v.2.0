using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SortingGameManager : GameManager
{
    private int numberOfObjects;
    private int[] combinationPlayer1;
    private int[] combinationPlayer2;
    private GameObject[] scores;

    public bool IsPlayerDragging { get; set; } //true if the player is dragging an object, false otherwise

    private void Start()
    {
        AudioManager.instance.PlayBackgroundMusic();

        //synchronize the value of numberOfObjects between all clients (otherwise only the player who creates the room will have it)
        /*
        if (gameObject.GetPhotonView().isMine)
            gameObject.GetPhotonView().RPC("SetNumberOfObjects", PhotonTargets.All, PhotonManager.instance.NumberOfImages);
        */

        numberOfObjects = PhotonManager.instance.NumberOfImages;
        scores = GameObject.FindGameObjectsWithTag("Score");
        IsPlayerDragging = false;
    }

    void SpawnObjects()
    {
        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 objectPosition = new Vector3(Random.Range(-numberOfObjects / 2f, numberOfObjects / 2f), 1, thisPlayer.transform.position.z / 1.7f);
            GameObject draggableObject = PhotonNetwork.Instantiate("Balloon", objectPosition, Quaternion.identity, 0);
            draggableObject.GetPhotonView().RPC("SetIndex", PhotonTargets.All, i);
        }

        Instantiate(Resources.Load<GameObject>("WaitingAnimation"), Vector3.zero, Quaternion.identity);
    }

    void SpawnAnchorPoints()
    {
        for (int i=0; i<numberOfObjects; i++)
        {
            GameObject anchorPointPlayer1 = PhotonNetwork.Instantiate("AnchorPoint", new Vector3(numberOfObjects * -0.75f + 0.75f + 1.5f * i, 1.7f, 7), Quaternion.identity, 0);
            anchorPointPlayer1.GetPhotonView().RPC("SetIndex", PhotonTargets.All, i);
            GameObject anchorPointPlayer2 = PhotonNetwork.Instantiate("AnchorPoint", new Vector3(numberOfObjects * -0.75f + 0.75f + 1.5f * i, 1.7f, -7), Quaternion.identity, 0);
            anchorPointPlayer2.GetPhotonView().RPC("SetIndex", PhotonTargets.All, numberOfObjects - i -1);
        }
    }

    [PunRPC]
    void UpdateScore(string scoreString)
    {
        foreach (GameObject score in scores)
            score.GetComponent<TextMeshPro>().text = scoreString;
    }

    [PunRPC]
    public void OnObjectPositioned(int playerId, int objectIndex, int anchorPointIndex)
    {
        if (this.gameObject.GetPhotonView().isMine)
        {
            if (playerId == this.gameObject.GetPhotonView().ownerId)
                combinationPlayer1[anchorPointIndex] = objectIndex + 1;
            else combinationPlayer2[anchorPointIndex] = objectIndex + 1;
        }

        int numberOfCorrectObjects = 0;

        for (int i = 0; i < numberOfObjects; i++)
            if (combinationPlayer1[i] == combinationPlayer2[i] && combinationPlayer1[i] != 0)
                numberOfCorrectObjects++;

        this.gameObject.GetPhotonView().RPC("UpdateScore", PhotonTargets.All, numberOfCorrectObjects + "/" + numberOfObjects);

        if (numberOfCorrectObjects == numberOfObjects)
            this.gameObject.GetPhotonView().RPC("StartVictoryAnimations", PhotonTargets.All);
    }

    [PunRPC]
    public void OnObjectRemoved(int playerId, int anchorPointIndex)
    {

        if (playerId == this.gameObject.GetPhotonView().ownerId)
            combinationPlayer1[anchorPointIndex] = 0;
        else combinationPlayer2[anchorPointIndex] = 0;

        int numberOfCorrectObjects = 0;

        for (int i = 0; i < numberOfObjects; i++)
            if (combinationPlayer1[i] == combinationPlayer2[i] && combinationPlayer1[i] != 0)
                numberOfCorrectObjects++;

        this.gameObject.GetPhotonView().RPC("UpdateScore", PhotonTargets.All, numberOfCorrectObjects + "/" + numberOfObjects);

        if (numberOfCorrectObjects == numberOfObjects)
            this.gameObject.GetPhotonView().RPC("StartVictoryAnimations", PhotonTargets.All);
    }

    /*
    [PunRPC]
    void SetNumberOfObjects(int number)
    {
        numberOfObjects = number;
    }
    */

    protected override void SetUpGame()
    {
        SpawnObjects();

        if (this.gameObject.GetPhotonView().isMine)
        {
            combinationPlayer1 = new int[numberOfObjects];
            combinationPlayer2 = new int[numberOfObjects];

            this.gameObject.GetPhotonView().RPC("UpdateScore", PhotonTargets.All, "0/" + numberOfObjects);
            SpawnAnchorPoints();
        }
    }
}
