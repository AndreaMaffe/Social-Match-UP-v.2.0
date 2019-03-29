using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

	public void DestroyGameManager()
    {
        PhotonView pw = GameObject.Find("GameManager(Clone)").GetComponent<PhotonView>();
        pw.RPC("Autodestroy", PhotonTargets.All, null);
    }

}
