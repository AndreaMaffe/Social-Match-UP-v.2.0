using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixingGameManager : Photon.MonoBehaviour
{
    private GameObject[] players;
    private GameObject thisPlayer;

    [SerializeField]
    private string pieceGazed;
    [SerializeField]
    private string brokenPieceGazed;

    Transform pieces;
    Transform brokenPieces;

    int numberOfFixedPieces;

    private void Start()
    {
        AudioManager.instance.PlayBackgroundMusic();
        StartCoroutine("SetUpGame");
        numberOfFixedPieces = 0;
        pieces = GameObject.Find("Pieces").transform;
        brokenPieces = GameObject.Find("BrokenPieces").transform;
    }

    //initial setup, called at task launch
    IEnumerator SetUpGame()
    {
        yield return new WaitForSeconds(10); //wait a bit to give to each client the time to instantiate their player (otherwise only one player is found)

        //find the players in the scene
        players = GameObject.FindGameObjectsWithTag("MainCamera");

        foreach (GameObject player in players)
            if (player.GetPhotonView().isMine)
                thisPlayer = player.gameObject;
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

    [PunRPC]
    public void OnPieceEnterGaze(string pieceName)
    {
        this.pieceGazed = pieceName;

        if (this.pieceGazed == this.brokenPieceGazed)
            StartCoroutine(Fix(pieceName));
    }

    [PunRPC]
    public void OnPieceExitGaze(string pieceName)
    {
        this.pieceGazed = "<nothing>";
        StopAllCoroutines();
    }

    [PunRPC]
    public void OnBrokenPieceEnterGaze(string brokenPieceName)
    {
        this.brokenPieceGazed = brokenPieceName;

        if (this.pieceGazed == this.brokenPieceGazed)
            StartCoroutine(Fix(brokenPieceName));
    }

    [PunRPC]
    public void OnBrokenPieceExitGaze(string brokenPieceName)
    {
        this.brokenPieceGazed = "<nothing>";
        StopAllCoroutines();
    }

    IEnumerator Fix(string pieceName)
    {
        yield return new WaitForSeconds(5);

        PhotonNetwork.Destroy(pieces.Find(pieceName).gameObject.GetPhotonView()); //make the fixer piece disappear
        brokenPieces.Find(pieceName).gameObject.GetPhotonView().RPC("Fix", PhotonTargets.All); //fix the object

        AudioManager.instance.PlayDingSound();

        numberOfFixedPieces++;

        if (numberOfFixedPieces == 4)
            this.gameObject.GetPhotonView().RPC("StartVictoryAnimations", PhotonTargets.All); //spostare sul player
    }



}
