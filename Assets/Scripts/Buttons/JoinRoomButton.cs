using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//encapsulates logic of the JoinRoomButton in the JoinGameMenu. Once clicked, it connects to that specific room
//and create the waiting animation
public class JoinRoomButton : MonoBehaviour
{
    private Text text;

	void Start ()
    {
        text = transform.Find("Text").GetComponent<Text>();
	}
	
	public void JoinRoom()
    {
        PhotonManager.instance.JoinRoom(text.text);

        GameObject.Find("RoomDisplayer").SetActive(false);
        GameObject.Find("OpenGamesText").SetActive(false);
        Instantiate(Resources.Load<GameObject>("WaitingAnimation"), GameObject.Find("Canvas").transform);

        foreach (GameObject roomButton in GameObject.FindGameObjectsWithTag("JoinRoomButton"))
            Destroy(roomButton);
    }
}
