﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LetsGoButton : MonoBehaviour {

    private string playerName, task, location, imagesType, numberOfImages;
    private byte numberOfPlayers;

    public void StartGame()
    {
        playerName = GameObject.Find("NameInputField").GetComponent<TMP_InputField>().text;
        if (playerName == "")
            playerName = "PLAYER";

        task = GameObject.Find("TaskButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;
        location = GameObject.Find("LocationButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;

        if (task == "Classic" || task == "Ordering")
            numberOfImages = GameObject.Find("NumberOfImagesButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;

        if (task == "Classic")
            imagesType = GameObject.Find("ImagesButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;

        if (GameObject.Find("HelperToggle").GetComponent<Toggle>().isOn)
            numberOfPlayers = 3;
        else numberOfPlayers = 2;

        PhotonManager.instance.NumberOfPlayers = numberOfPlayers;

        if (task == "Classic" || task == "Ordering")
            PhotonManager.instance.NumberOfImages = System.Convert.ToInt32(numberOfImages);

        if (task == "Classic")
            PhotonManager.instance.ImageType = imagesType;
     
        PhotonManager.instance.Location = location;
        PhotonManager.instance.Task = task;

        PhotonManager.instance.CreateRoom(playerName + " (" + task + " - " + location + " - " + imagesType + ")");
    }
}
