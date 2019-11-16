using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//VERY IMPORTANT class: used for the "Let's Go" button in the new game menu. Once the button is clicked, it recollects 
//game parameters by reading the field in the menu buttons and send this data to the PhotonManager to create a new game room.
public class LetsGoButton : MonoBehaviour
{
    private string playerName, task, location, imagesType, numberOfImages;
    private byte numberOfPlayers;
    private bool audioChat;

    public void StartGame()
    {
        playerName = GameObject.Find("NameInputField").GetComponent<TMP_InputField>().text;
        if (playerName == "")
            playerName = "PLAYER";

        task = GameObject.Find("TaskButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;
        location = GameObject.Find("LocationButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;

        if (task == "Classic" || task == "Sorting")
            numberOfImages = GameObject.Find("NumberOfImagesButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;

        if (task == "Classic")
            imagesType = GameObject.Find("ImagesButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;

        if (GameObject.Find("HelperToggle").GetComponent<Toggle>().isOn)
            numberOfPlayers = 3;
        else numberOfPlayers = 2;

        audioChat = GameObject.Find("AudioChatToggle").GetComponent<Toggle>().isOn;

        PhotonManager.instance.NumberOfPlayers = numberOfPlayers;

        if (task == "Classic" || task == "Sorting")
            PhotonManager.instance.NumberOfImages = System.Convert.ToInt32(numberOfImages);

        if (task == "Classic")
            PhotonManager.instance.ImageType = imagesType;
     
        PhotonManager.instance.Location = location;
        PhotonManager.instance.Task = task;
        PhotonManager.instance.AudioChat = audioChat;

        PhotonManager.instance.CreateRoom(playerName + " (" + task + " - " + location + " - " + imagesType + ")");
    }
}
