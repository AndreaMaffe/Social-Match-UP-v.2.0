using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//used in the JoinGame Scene. Simply creates a JoinRoomButton for each open room available, and refreshes every 2 seconds.
public class RoomDisplayer : MonoBehaviour
{
    private float timer;

    private void Start()
    {
        timer = 0;  
        DisplayRooms();
    }

    //Refresh every 2 seconds
    private void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 2f)
        {
            DisplayRooms();
            timer = 0;
        }
    }

    //Gets info about rooms in the PhotonManager and creates a JoinRoomButton prefab for each one of them
    private void DisplayRooms()
    {
        foreach (GameObject roomButton in GameObject.FindGameObjectsWithTag("JoinRoomButton"))
            Destroy(roomButton);

        int i = 0;

        foreach (RoomInfo roomInfo in PhotonManager.instance.RoomInfo)
        {
            GameObject joinRoomButton = Instantiate(Resources.Load<GameObject>("JoinRoomButton"), GameObject.Find("Canvas").transform);
            joinRoomButton.GetComponent<RectTransform>().localPosition = new Vector3(0, i, 0);
            joinRoomButton.transform.Find("Text").GetComponent<Text>().text = roomInfo.Name;
            i -= 80;
        }
    }
}
