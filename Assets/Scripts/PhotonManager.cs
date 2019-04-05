using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class PhotonManager : Photon.MonoBehaviour
{
    public static PhotonManager instance = null;

    public RoomInfo[] RoomInfo { get; set; }

    public string Task;
    public string Location;
    public string ImageType;
    public int NumberOfImages;
    public byte NumberOfPlayers;


    private int order;

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
        order = 0;
        PhotonNetwork.automaticallySyncScene = true;
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
        roomOptions.MaxPlayers = NumberOfPlayers;
        
        PhotonNetwork.CreateRoom(roomName, roomOptions, PhotonNetwork.lobby);
    }

    public void JoinRoom(string RoomName)
    {
        PhotonNetwork.JoinRoom(RoomName);
    }

    public void OnJoinedRoom()
    {
        order = PhotonNetwork.room.PlayerCount;
        OnPhotonPlayerConnected(PhotonNetwork.player);
    }

    void OnReceivedRoomList()
    {
        RoomInfo = PhotonNetwork.GetRoomList();
    }

    void OnReceivedRoomListUpdate()
    {
        RoomInfo = PhotonNetwork.GetRoomList();
    }

    //called by every client when a player joins the room
    void OnPhotonPlayerConnected(PhotonPlayer newPlayer) 
    {
        if (PhotonNetwork.room.PlayerCount == PhotonNetwork.room.MaxPlayers) //if all the players are connected
        {
            switch(order)
            {
                case 1: PhotonNetwork.SetMasterClient(PhotonNetwork.player);
                        PhotonNetwork.LoadLevel(Task + "Gameplay" + Location);
                        StartCoroutine(StartGameAndInstantiateGameManager());
                        break;
                case 2: StartCoroutine(StartGameAsPlayer());
                        break;
                case 3: StartCoroutine(StartGameAsHelper());
                        break;
            }

            StartCoroutine(SwitchVROn());
        }
    }

    IEnumerator StartGameAsPlayer()
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.Instantiate("HeadsetPlayer", new Vector3(0, 3, -4), Quaternion.identity, 0);
    }

    IEnumerator StartGameAsHelper()
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.Instantiate("Helper", new Vector3(-4, 3, 0), new Quaternion(0, 0.707f, 0, 0.707f), 0);
    }

    IEnumerator StartGameAndInstantiateGameManager() //crea un GameManager al primo giocatore e una sua View in tutti i mondi
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.Instantiate("HeadsetPlayer", new Vector3(0, 3, 4), new Quaternion(0,1,0,0), 0);
        PhotonNetwork.Instantiate(Task + "GameManager", Vector3.zero, Quaternion.identity, 0);
    }

    private IEnumerator SwitchVROn()
    {
        XRSettings.LoadDeviceByName("cardboard");
        yield return null;
        XRSettings.enabled = true;
    }

}
