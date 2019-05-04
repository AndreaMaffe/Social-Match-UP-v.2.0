using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorAvatar : MonoBehaviour
{
    public GameObject camera; //main camera to follow
	
	void Update ()
    {
        this.transform.rotation = Quaternion.Euler(-camera.transform.rotation.eulerAngles.x, - camera.transform.rotation.eulerAngles.y, 0);
	}

    public void UpdateAvatar()
    {
        Destroy(transform.GetChild(0).gameObject);
        GameObject newAvatar = Instantiate(Resources.Load<GameObject>("Avatars/Avatar_" + PlayerPrefs.GetInt("Avatar")), this.transform.position, this.transform.rotation);
        newAvatar.transform.Rotate(Vector3.up, 180);
        newAvatar.transform.parent = this.transform;
    }


}
