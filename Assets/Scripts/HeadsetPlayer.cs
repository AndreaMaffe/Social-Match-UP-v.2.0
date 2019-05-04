using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadsetPlayer : MonoBehaviour
{  

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

        int chosenAvatar = PlayerPrefs.GetInt("Avatar");
        Debug.Log(chosenAvatar);
        GetComponent<PhotonView>().RPC("LoadChosenAvatar", PhotonTargets.All, chosenAvatar);

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

    //substitutes the base avatar with the one chosen by the user in the ChangingRoom 
    [PunRPC]
    public void LoadChosenAvatar(int avatarIndex)
    {
        GameObject oldAvatar = transform.Find("Avatar").gameObject;
        Vector3 oldAvatarPosition = oldAvatar.transform.position;
        Destroy(oldAvatar);

        GameObject newAvatar = Instantiate(Resources.Load<GameObject>("Avatars/Avatar_" + PlayerPrefs.GetInt("Avatar")));
        newAvatar.transform.position = oldAvatarPosition;
        oldAvatar.transform.parent = this.transform;
    }
}
