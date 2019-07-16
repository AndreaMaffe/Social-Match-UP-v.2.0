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
    public bool AudioChat;

    private int order;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(this.gameObject);
    }

    void Start ()
    {
        DontDestroyOnLoad(this.gameObject);
        PhotonNetwork.automaticallySyncScene = true;
        Connect();
    }

    
    public void OnConnectedToMaster()
    {
        Debug.Log("Connected to the server");
        PhotonNetwork.JoinLobby(new TypedLobby("MyLobby", LobbyType.SqlLobby));
    }
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");

            if (PhotonNetwork.inRoom)
                PhotonNetwork.LeaveRoom();
        }
    }

    private void Connect()
    {
        try
        {
            PhotonNetwork.ConnectUsingSettings("1");
            Debug.Log("Trying to connnect...");
        }
        catch (System.Net.Sockets.SocketException e) { Debug.LogError("Connection failed!");  }
    }

    //Called if a connect call to the Photon server failed (before the connection was established), followed by a call to OnDisconnectedFromPhoton().
    public void OnFailedToConnectToPhoton()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
            SceneManager.LoadScene("MainMenu");
        Debug.Log("Connection failed!");
    }

    //Called when something causes the connection to fail (after it was established), followed by a call to OnDisconnectedFromPhoton().
    public void OnConnectionFail()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
            SceneManager.LoadScene("MainMenu");
        Debug.Log("Disconnected from server!");
    }

    public void OnDisconnectedFromPhoton()
    {
        Connect();
        Debug.Log("Trying to connnect...");
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
        this.gameObject.AddComponent<PhotonView>();
        gameObject.GetPhotonView().viewID = PhotonNetwork.room.GetHashCode();
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
                        gameObject.GetPhotonView().RPC("SetGameParameters", PhotonTargets.Others, Task, Location, NumberOfImages, AudioChat);
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
        GameObject player = PhotonNetwork.Instantiate("HeadsetPlayer", new Vector3(0, 3, -4), Quaternion.identity, 0);

        if (AudioChat)
        {
            player.GetComponent<PhotonVoiceRecorder>().enabled = true;
            PhotonVoiceSettings.Instance.VoiceDetection = true;
        }

        else
        {
            player.GetComponent<PhotonVoiceRecorder>().enabled = false;
            PhotonVoiceSettings.Instance.VoiceDetection = false;
        }
    }

    IEnumerator StartGameAsHelper()
    {
        yield return new WaitForSeconds(5f);

        GameObject helper = PhotonNetwork.Instantiate("Helper", new Vector3(-4, 3, 0), new Quaternion(0, 0.707f, 0, 0.707f), 0);

        if (AudioChat)
        {
            helper.GetComponent<PhotonVoiceRecorder>().enabled = true;
            PhotonVoiceSettings.Instance.VoiceDetection = true;
        }

        else
        {
            helper.GetComponent<PhotonVoiceRecorder>().enabled = false;
            PhotonVoiceSettings.Instance.VoiceDetection = false;
        }

    }

    IEnumerator StartGameAndInstantiateGameManager() //crea un GameManager al primo giocatore e una sua View in tutti i mondi
    {
        yield return new WaitForSeconds(5f);
        GameObject player = PhotonNetwork.Instantiate("HeadsetPlayer", new Vector3(0, 3, 4), new Quaternion(0, 1, 0, 0), 0);

        if (AudioChat)
        {
            player.GetComponent<PhotonVoiceRecorder>().enabled = true;
            PhotonVoiceSettings.Instance.VoiceDetection = true;
        }

        else
        {
            player.GetComponent<PhotonVoiceRecorder>().enabled = false;
            PhotonVoiceSettings.Instance.VoiceDetection = false;
        }

        PhotonNetwork.Instantiate("Managers/" + Task + "GameManager", Vector3.zero, Quaternion.identity, 0);
    }

    private IEnumerator SwitchVROn()
    {
        XRSettings.LoadDeviceByName("cardboard");
        yield return null;
        XRSettings.enabled = true;
    }

    [PunRPC]
    public void SetGameParameters(string task, string location, int numberOfElements, bool audioChat)
    {
        this.Task = task;
        this.Location = location;
        this.NumberOfImages = numberOfElements;
        this.AudioChat = audioChat;
    }

}
