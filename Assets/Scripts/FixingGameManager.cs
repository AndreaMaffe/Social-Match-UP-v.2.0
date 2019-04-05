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

    private void Start()
    {
        AudioManager.instance.PlayBackgroundMusic();
        StartCoroutine("SetUpGame");        
    }

    //initial setup, called at task launch
    IEnumerator SetUpGame()
    {
        yield return new WaitForSeconds(6); //wait a bit to give to each client the time to instantiate their player (otherwise only one player is found)

        pieces = GameObject.Find("Pieces").transform;
        brokenPieces = GameObject.Find("BrokenPieces").transform;
        
        //find the players in the scene
        players = GameObject.FindGameObjectsWithTag("MainCamera");
        foreach (GameObject player in players)
            if (player.GetPhotonView().isMine)
                thisPlayer = player.gameObject;

        Debug.Log(players.Length + " players");

        //only the main GameManager (not its PhotonViews) must spawn the images
        if (this.gameObject.GetPhotonView().isMine)
            ;//SpawnRandomImages();
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
        yield return new WaitForSeconds(1);
        //AudioManager.instance.PlayFireworksSound();
        Instantiate(Resources.Load<GameObject>("Fireworks"), Vector3.up * 30, Quaternion.identity);
        yield return new WaitForSeconds(1);
        AudioManager.instance.StopBackgroundMusic();
        yield return new WaitForSeconds(3);
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

        Debug.Log("Player starts looking at " + pieceName);
    }

    [PunRPC]
    public void OnPieceExitGaze(string pieceName)
    {
        this.pieceGazed = "<nothing>";
        StopAllCoroutines();

        Debug.Log("Player stops looking at " + pieceName);
    }

    [PunRPC]
    public void OnBrokenPieceEnterGaze(string brokenPieceName)
    {
        this.brokenPieceGazed = brokenPieceName;

        if (this.pieceGazed == this.brokenPieceGazed)
            StartCoroutine(Fix(brokenPieceName));

        Debug.Log("Player starts looking at " + brokenPieceName);
    }

    [PunRPC]
    public void OnBrokenPieceExitGaze(string brokenPieceName)
    {
        this.brokenPieceGazed = "<nothing>";
        StopAllCoroutines();

        Debug.Log("Player stops looking at " + brokenPieceName);

    }

    IEnumerator Fix(string pieceName)
    {
        Debug.Log("I start fixing " + pieceName + "!");

        yield return new WaitForSeconds(5);

        Debug.Log("Fixing completed!");

        PhotonNetwork.Destroy(pieces.Find(pieceName).gameObject.GetPhotonView());
        brokenPieces.Find(pieceName).gameObject.GetPhotonView().RPC("Fix", PhotonTargets.All);

        AudioManager.instance.PlayDingSound();
    }



}
