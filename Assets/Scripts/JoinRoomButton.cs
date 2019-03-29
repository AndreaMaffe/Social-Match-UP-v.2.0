using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomButton : MonoBehaviour {

    private Text text;

	void Start ()
    {
        text = transform.Find("Text").GetComponent<Text>();
	}
	
	public void JoinRoom()
    {
        PhotonManager.instance.JoinRoom(text.text);
    }
}
