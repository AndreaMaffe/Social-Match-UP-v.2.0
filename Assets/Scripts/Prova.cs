using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prova : MonoBehaviour
{
	void Start ()
    {
        //GameObject p = Instantiate(Resources.Load<GameObject>("PhotonVoice"));
        //p.AddComponent<PhotonVoiceRecorder>();

        try
        {
            PhotonNetwork.ConnectUsingSettings("1");
            Debug.Log("Trying to connnect...");

        }
        catch (System.Net.Sockets.SocketException e) { Debug.LogError("Connection failed!"); }
    }

    private void Update()
    {
        if (PhotonNetwork.connected && Input.GetKeyDown(KeyCode.C))
            OnConnectedToMaster();
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("Connected to the server");
        //PhotonNetwork.JoinLobby(new TypedLobby("MyLobby", LobbyType.SqlLobby));
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        PhotonNetwork.JoinOrCreateRoom("ciao", roomOptions, TypedLobby.Default);
    }

    public void OnJoinedRoom()
    {
        Debug.Log("Numero di giocatori: " + PhotonNetwork.room.PlayerCount);
        PhotonNetwork.Instantiate("NetworkCube", new Vector3(Random.Range(-3,3), 3, 0), Quaternion.identity, 0);
    }
}
