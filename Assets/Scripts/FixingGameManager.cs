using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FixingGameManager : GameManager
{
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
        numberOfFixedPieces = 0;
        pieces = GameObject.Find("Pieces").transform;
        brokenPieces = GameObject.Find("BrokenPieces").transform;
    }

    [PunRPC]
    public void StartVictoryAnimations()
    {
        StartCoroutine(OnVictory());
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

    protected override void SetUpGame()
    {
    }
}
