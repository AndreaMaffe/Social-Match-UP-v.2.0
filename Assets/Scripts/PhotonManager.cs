﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;

public class PhotonManager : Photon.MonoBehaviour
{
    public static PhotonManager instance = null;

    public int NumberOfImages { get; set; }
    public string ImageType { get; set; }
    public RoomInfo[] RoomInfo { get; set; }

    private int playerCount;

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
        PhotonNetwork.room.IsVisible = true;
        Debug.Log("Connected to the room");
        SceneManager.LoadScene("Gameplay");

        if (PhotonNetwork.room.PlayerCount == 2)  //se sei il secondo giocatore
        {
            StartCoroutine(StartGame());
            StartCoroutine(SwitchVROn());
        }
    }

    void OnReceivedRoomList()
    {
        RoomInfo = PhotonNetwork.GetRoomList();
    }

    void OnReceivedRoomListUpdate()
    {
        RoomInfo = PhotonNetwork.GetRoomList();
    }

    void OnPhotonPlayerConnected(PhotonPlayer newPlayer) //se sei il primo giocatore
    {
        if (PhotonNetwork.room.PlayerCount == 2)
        {
            StartCoroutine(StartGameAndInstantiateGameManager());
            StartCoroutine(SwitchVROn());
        }
    }

    IEnumerator StartGame()
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.Instantiate("HeadsetPlayer", new Vector3(0, 3, -4), Quaternion.identity, 0);
    }

    IEnumerator StartGameAndInstantiateGameManager() //crea un GameManager al primo giocatore e una sua View in tutti i mondi
    {
        yield return new WaitForSeconds(5f);
        PhotonNetwork.Instantiate("HeadsetPlayer", new Vector3(0, 3, 4), new Quaternion(0,1,0,0), 0);
        PhotonNetwork.Instantiate("GameManager", Vector3.zero, Quaternion.identity, 0);
    }

    private IEnumerator SwitchVROn()
    {
        XRSettings.LoadDeviceByName("cardboard");
        yield return null;
        XRSettings.enabled = true;
    }

}
