using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPiece : MonoBehaviour {

    private PhotonView gameManagerView;
    private GameObject effectiveObject;
    private GameObject collider;

    void Start()
    {
        StartCoroutine(Wait()); //short delay to make sure everything has been correctly generated
        effectiveObject = transform.Find("Object").gameObject;
        collider = transform.Find("Collider").gameObject;
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
        gameManagerView.RPC("OnBrokenPieceEnterGaze", gameManagerView.owner, this.gameObject.name);
    }

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

    public IEnumerator Wait()
    {
        yield return new WaitForSeconds(10);
        gameManagerView = GameObject.Find("FixingGameManager(Clone)").GetPhotonView();

    }
}
