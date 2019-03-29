using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LetsGoButton : MonoBehaviour {

    public void StartGame()
    {
        PhotonManager.instance.CreateRoom(GameObject.Find("NameInputField").GetComponent<TMP_InputField>().text);
    }
}
