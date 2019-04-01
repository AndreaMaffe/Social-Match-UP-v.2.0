using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetsGoButton : MonoBehaviour {

    private string playerName, task, location, imagesType, numberOfImages;

    public void StartGame()
    {
        playerName = GameObject.Find("NameInputField").GetComponent<TMP_InputField>().text;
        if (playerName == "")
            playerName = "PLAYER";

        task = GameObject.Find("TaskButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;
        location = GameObject.Find("LocationButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;
        imagesType = GameObject.Find("ImagesButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;
        numberOfImages = GameObject.Find("NumberOfImagesButton").transform.Find("Label").GetComponent<TextMeshProUGUI>().text;

        PhotonManager.instance.ImageType = imagesType;
        PhotonManager.instance.NumberOfImages = System.Convert.ToInt32(numberOfImages);
        PhotonManager.instance.CreateRoom(playerName + " (" + task + " - " + location + " - " + imagesType + ")");
    }
}
