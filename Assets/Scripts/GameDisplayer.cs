using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDisplayer : MonoBehaviour {

	void Start ()
    {
        PhotonManager.instance.OpenRooms = true;
	}

    private void OnDestroy()
    {
        PhotonManager.instance.OpenRooms = false;
    }


}
