using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetsGoButton : MonoBehaviour {

    public TextMeshProUGUI imagesButtonLabel;
    public TextMeshProUGUI numberOfImagesButtonLabel;

    private void Start()
    {
        imagesButtonLabel = GameObject.Find("ImagesButton").transform.Find("Label").GetComponent<TextMeshProUGUI>();
        numberOfImagesButtonLabel = GameObject.Find("NumberOfImagesButton").transform.Find("Label").GetComponent<TextMeshProUGUI>();

    }


    public void StartGame()
    {
        PhotonManager.instance.ImageType = imagesButtonLabel.text;
        PhotonManager.instance.NumberOfImages = System.Convert.ToInt32(numberOfImagesButtonLabel.text);

        PhotonManager.instance.CreateRoom(GameObject.Find("NameInputField").GetComponent<TMP_InputField>().text);
    }
}
