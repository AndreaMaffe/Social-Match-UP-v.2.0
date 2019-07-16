using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class for each avatar available in the ChangingRoom, provides methods to check when the player is looking at it 
//and to save the player selection
public class ChoosableAvatar : MonoBehaviour
{
    private ChangingRomManager manager;
    public bool Active { get; set; } //true if the avatar can be chosen, set to false when the avatar is chosen

    private void Start()
    {
        manager = GameObject.Find("ChangingRoomManager").GetComponent<ChangingRomManager>();
        Active = true;
    }

    //called when the player starts looking at the object
    public void OnGazeEnter()
    {
        if (Active)
            StartCoroutine("ChooseAvatar");
    }

    //called when the player stops looking at the object
    public void OnGazeExit()
    {
        StopAllCoroutines();
    }

    //wait a few seconds, then notifies the ChangingRoomManager of the choice and substitutes the avatar with the base avatar
    IEnumerator ChooseAvatar()
    {
        yield return new WaitForSeconds(2);
        AudioManager.instance.PlayDingSound();
        manager.OnAvatarChosen(this.gameObject);

        yield return new WaitForSeconds(0.1f);

        //destroy the avatar and substitute it with the base avatar
        Destroy(transform.GetChild(0).gameObject);
        GameObject newAvatar = Instantiate(Resources.Load<GameObject>("Avatars/BaseAvatar"), this.transform.position, this.transform.rotation);
        newAvatar.transform.rotation = Quaternion.LookRotation(-newAvatar.transform.position);
        newAvatar.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        newAvatar.transform.parent = this.transform;

        Active = false; //now the avatar cannot be chosen again
    }


}
