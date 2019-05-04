using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoosableAvatar : MonoBehaviour
{
    private ChangingRomManager manager;
    public bool Active { get; set; }

    private void Start()
    {
        manager = GameObject.Find("ChangingRoomManager").GetComponent<ChangingRomManager>();
        Active = true;
    }

    public void OnGazeEnter()
    {
        if (Active)
            StartCoroutine("ChooseAvatar");
    }

    public void OnGazeExit()
    {
        StopAllCoroutines();
    }

    IEnumerator ChooseAvatar()
    {
        yield return new WaitForSeconds(2);
        AudioManager.instance.PlayDingSound();
        manager.OnAvatarChosen(this.gameObject);

        yield return new WaitForSeconds(0.1f);

        Destroy(transform.GetChild(0).gameObject);
        GameObject newAvatar = Instantiate(Resources.Load<GameObject>("Avatars/BaseAvatar"), this.transform.position, this.transform.rotation);
        newAvatar.transform.rotation = Quaternion.LookRotation(-newAvatar.transform.position);
        newAvatar.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        newAvatar.transform.parent = this.transform;

        Active = false;
    }


}
