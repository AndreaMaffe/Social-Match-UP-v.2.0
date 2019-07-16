using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for the "missing pieces" in the Fixing game mode. Provides methods to notifiy the SortingGameManager wether the 
//player is looking at the object or not. Symmetrical to "BrokenPiece" class.
public class Piece : MonoBehaviour
{ 
    private PhotonView gameManagerView; //view of the FixingGameManager, needed to call its methods remotely

    //the view of the FixingGameManager may not have been initializated yet trought the network when the scene is 
    //created, so it's necessary to manually check it until it's found
    private void Update()
    {
        if (gameManagerView == null)
            try
            {
                gameManagerView = GameObject.Find("FixingGameManager(Clone)").GetPhotonView();
            } catch (System.NullReferenceException e) { Debug.Log("Manager not found!"); }
    }

    //called when the player starts looking at the object
    public void OnEnterGaze()
    {
        gameManagerView.RPC("OnPieceEnterGaze", gameManagerView.owner, this.gameObject.name);
    }

    //called when the player stops looking at the object
    public void OnExitGaze()
    {
        gameManagerView.RPC("OnPieceExitGaze", gameManagerView.owner, this.gameObject.name);
    }

}
