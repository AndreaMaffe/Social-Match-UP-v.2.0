using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadsetPlayer : MonoBehaviour {  

    void Start()
    {
        //GameObject photonVoiceManager = PhotonNetwork.Instantiate("PhotonVoice", Vector3.zero, Quaternion.identity, 0);

        if (!GetComponent<PhotonView>().isMine)
        {
            //transform.Find("Camera").GetComponent<Canvas>().enabled = false;
            this.GetComponent<Camera>().enabled = false;
            this.enabled = false;
            this.transform.Find("GvrReticlePointer").gameObject.SetActive(false);
        }
    }

    void Update()
    {
            if (Input.GetKey(KeyCode.W))
                transform.Rotate(Vector3.right, -1f);
            if (Input.GetKey(KeyCode.A))
                transform.Rotate(Vector3.up, -1);
            if (Input.GetKey(KeyCode.S))
                transform.Rotate(Vector3.right, 1f);
            if (Input.GetKey(KeyCode.D))
                transform.Rotate(Vector3.up, 1f);
    }

}
