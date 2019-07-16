using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingRoomButton : MonoBehaviour
{
    public int direction;
    public Animator animator;
    private ChangingRomManager manager;

    private void Start()
    {
        manager = GameObject.Find("ChangingRoomManager").GetComponent<ChangingRomManager>();
    }

    public void OnGazeEnter()
    {
        StartCoroutine("Clicked");
    }

    IEnumerator Clicked()
    {
        animator.SetTrigger("Clicked");
        yield return new WaitForSeconds(0.4f);
        AudioManager.instance.PlayPopSound();
        manager.OnArrowButtonClicked(direction);
    }
}
