using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
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

        else
        {
            //substitutes the base avatar with the one chosen by the user in the ChangingRoom
            int chosenAvatar = PlayerPrefs.GetInt("Avatar");
            GetComponent<PhotonView>().RPC("LoadAvatar", PhotonTargets.All, chosenAvatar);
        }
    }
     
    [PunRPC]
    public void LoadAvatar(int avatarIndex)
    {
        GameObject oldAvatar = transform.Find("Avatar").gameObject;
        Vector3 oldAvatarPosition = oldAvatar.transform.position;
        Quaternion oldAvatarRotation = oldAvatar.transform.rotation;
        Destroy(oldAvatar);

        GameObject newAvatar = Instantiate(Resources.Load<GameObject>("Avatars/Avatar_" + avatarIndex));
        newAvatar.transform.position = oldAvatarPosition;
        newAvatar.transform.rotation = oldAvatarRotation;
        newAvatar.transform.parent = this.transform;
    }
}
