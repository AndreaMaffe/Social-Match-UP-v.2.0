using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for the avatar in the mirror in the ChangingRoom. Replicates the movements of the camera  with a mirror-like symmetry
public class MirrorAvatar : MonoBehaviour
{
    public GameObject playerCamera; //main camera to follow
	
	void Update ()
    {
        this.transform.rotation = Quaternion.Euler(-playerCamera.transform.rotation.eulerAngles.x, -playerCamera.transform.rotation.eulerAngles.y, 0);
	}

    //destroy the current avatar and substitute it with the one chosen by the player
    public void UpdateAvatar()
    {
        Destroy(transform.GetChild(0).gameObject);
        GameObject newAvatar = Instantiate(Resources.Load<GameObject>("Avatars/Avatar_" + PlayerPrefs.GetInt("Avatar")), this.transform.position, this.transform.rotation);
        newAvatar.transform.Rotate(Vector3.up, 180);
        newAvatar.transform.parent = this.transform;
    }


}
