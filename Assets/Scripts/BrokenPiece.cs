using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for the "broken" objects in the Fixing game mode. Provides methods to notifiy the FixingGameManager when
//the object is being gazed or not and to "fix" the object. Symmetrical to "Piece" class.
public class BrokenPiece : MonoBehaviour
{
    private PhotonView gameManagerView; //view of the FixingGameManager, needed to call its methods remotely
    private GameObject effectiveObject; //the final "fixed" object, which is initially disabled
    private GameObject collider; //collider of the object. It is disabled once the object is fixed

    void Start()
    {
        effectiveObject = transform.Find("Object").gameObject;
        collider = transform.Find("Collider").gameObject;
    }

    private void Update()
    {
        //the view of the FixingGameManager may not have been initializated yet trought the network when the scene is 
        //created, so it's necessary to manually check it until it's found
        if (gameManagerView == null)
            try
            {
                gameManagerView = GameObject.Find("FixingGameManager(Clone)").GetPhotonView();
            } catch (System.NullReferenceException e) { Debug.Log("Manager not found!"); }
    }

    //called when the player starts looking at the object
    public void OnEnterGaze()
    {
        gameManagerView.RPC("OnBrokenPieceEnterGaze", gameManagerView.owner, this.gameObject.name);
    }

    //called when the player stops looking at the object
    public void OnExitGaze()
    {
        gameManagerView.RPC("OnBrokenPieceExitGaze", gameManagerView.owner, this.gameObject.name);
    }

    [PunRPC]
    public void Fix()
    {
        effectiveObject.SetActive(true);
        collider.SetActive(false);
    }
}
