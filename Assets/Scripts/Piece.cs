using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour {

    private PhotonView gameManagerView;

    void Start ()
    {
    }

    private void Update()
    {
        if (gameManagerView == null)
            try
            {
                gameManagerView = GameObject.Find("FixingGameManager(Clone)").GetPhotonView();
            } catch (System.NullReferenceException e) { Debug.Log("Manager not found!"); }
    }

    public void OnEnterGaze()
    {
        gameManagerView.RPC("OnPieceEnterGaze", gameManagerView.owner, this.gameObject.name);
    }

    public void OnExitGaze()
    {
        gameManagerView.RPC("OnPieceExitGaze", gameManagerView.owner, this.gameObject.name);
    }

}
