using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PhotonManager : Photon.MonoBehaviour
{
    public static PhotonManager instance = null;

    public int NumberOfImages { get; set; }
    public string ImageType { get; set; }

    private int playerCount;

    public bool OpenRooms;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this);
    }

    void Start ()
    {
        DontDestroyOnLoad(this.gameObject);
        PhotonNetwork.ConnectUsingSettings("1");

    }

    public void OnConnectedToMaster()
    {
        Debug.Log("Connected to the server");
        PhotonNetwork.JoinLobby(new TypedLobby("MyLobby", LobbyType.SqlLobby));
    }

    public void CreateRoom(string roomName)
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        
        PhotonNetwork.CreateRoom(roomName, roomOptions, PhotonNetwork.lobby);
    }

    public void JoinRoom(string RoomName)
    {
        PhotonNetwork.JoinRoom(RoomName);
    }

    public void OnJoinedLobby()
    {
        Debug.Log("Connected to the lobby " + PhotonNetwork.lobby.Name);
        Debug.Log("Number of rooms: " + PhotonNetwork.countOfRooms);
    }

    public void OnJoinedRoom()
    {
        SceneManager.LoadScene("Gameplay2");
        PhotonNetwork.room.IsVisible = true;
        Debug.Log("Connected to the room");

        if (PhotonNetwork.room.PlayerCount == 2)  //se sei il secondo giocatore
        {
            StartCoroutine(StartGame());
        }
    }

    void OnPhotonJoinRoomFailed()
    {
        Debug.Log("Can't join any room!");
    }

    void OnReceivedRoomListUpdate()
    {
        if (OpenRooms)
            foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList())
            {
                GameObject joinRoomButton = Instantiate(Resources.Load<GameObject>("JoinRoomButton"), GameObject.Find("Canvas").transform);
                joinRoomButton.transform.Find("Text").GetComponent<Text>().text = roomInfo.Name;
            }
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer) //se sei il primo giocatore
    {
        if (PhotonNetwork.room.PlayerCount == 2)
        {
            StartCoroutine(StartGame2());
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.Instantiate("HeadsetPlayer", new Vector3(0, 3, -4), Quaternion.identity, 0);
    }

    IEnumerator StartGame2() //crea un GameManager al primo giocatore e una sua View in tutti i mondi
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.Instantiate("HeadsetPlayer", new Vector3(0, 3, 4), new Quaternion(0,1,0,0), 0);
        PhotonNetwork.Instantiate("GameManager", Vector3.zero, Quaternion.identity, 0);
    }

}
