using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{  
    void Start()
    {
        //disable the camera for all players except the one who owns the Helper (otherwise other players may 
        //see the game from the wrong camera
        if (GetComponent<PhotonView>().isMine == false)
        {
            this.GetComponent<Camera>().enabled = false;
            this.transform.Find("GvrReticlePointer").gameObject.SetActive(false);
        }

        else
        {
            //substitutes the base avatar with the one chosen by the user in the ChangingRoom (its index is saved in PlayerPrefs)
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
