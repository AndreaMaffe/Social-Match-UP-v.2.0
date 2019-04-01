using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomDisplayer : MonoBehaviour
{
    private float time;

    private void Start()
    {
        time = 0;
        DisplayRooms();
    }

    private void Update()
    {
        time += Time.deltaTime;
        if(time >= 2f)
        {
            DisplayRooms();
            time = 0;
        }
    }

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
            i -= 70;
        }
    }
}
