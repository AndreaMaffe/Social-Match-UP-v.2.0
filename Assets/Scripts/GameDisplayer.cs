using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDisplayer : MonoBehaviour {

	void Start ()
    {
        PhotonManager.instance.DisplayRooms();
	}
	
	
}
