using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{

    void Start()
    {
        //GameObject photonVoiceManager = PhotonNetwork.Instantiate("PhotonVoice", Vector3.zero, Quaternion.identity, 0);

        if (GetComponent<PhotonView>().isMine == false)
        {
            //transform.Find("Camera").GetComponent<Canvas>().enabled = false;
            this.GetComponent<Camera>().enabled = false;
            this.transform.Find("GvrReticlePointer").gameObject.SetActive(false);
        }
    }
}
