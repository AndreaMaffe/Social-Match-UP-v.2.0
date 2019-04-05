using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Image : MonoBehaviour {

    private int index; //from 0 to numberOfImages: used to check with image the player is looking at
    private Color originalColor;
    private Transform circle;
    private GameObject goldenParticle;
    private PhotonView gameManagerView;
    public Animator animator;

    public bool IsGazed { get; set; }


    private void Start()
    {
        IsGazed = false;
        goldenParticle = transform.Find("GoldenParticles").gameObject;
        gameManagerView = GameObject.Find("ClassicGameManager(Clone)").GetPhotonView();
    }

    [PunRPC]
	public void SetSprite(string multipleSpriteName, int index)
    {
        Debug.Log("Carico " + multipleSpriteName + "numero " + index);
        Sprite[] imageSprites = Resources.LoadAll<Sprite>(multipleSpriteName);
        GetComponent<SpriteRenderer>().sprite = imageSprites[index];
    }

    [PunRPC]
    public void ChangeCircleColor(float r, float g, float b)
    {
        this.originalColor = new Color(r, g, b);
        transform.Find("Circle").GetComponent<SpriteRenderer>().color = this.originalColor;
    }

    [PunRPC]
    public void SetIndex(int index)
    {
        this.index = index;
    }

    [PunRPC]
    public void StartDestroyAnimation()
    {
        transform.Find("Circle").GetComponent<SpriteRenderer>().color = Color.yellow;
        goldenParticle.SetActive(true);
    }

    [PunRPC]
    public void StopDestroyAnimation()
    {
        transform.Find("Circle").GetComponent<SpriteRenderer>().color = this.originalColor;
        goldenParticle.SetActive(false);
    }

    [PunRPC]
    public void AutoDestroy()
    {
        AudioManager.instance.PlayDingSound();
        Destroy(this.gameObject);
    }

    public void OnEnterGaze()
    {
        animator.SetBool("Gazed", true);
        gameManagerView.RPC("OnImageEnterGaze", gameManagerView.owner, index, PhotonNetwork.player.ID);
    }

    public void OnExitGaze()
    {
        animator.SetBool("Gazed", false);
        gameManagerView.RPC("OnImageExitGaze", gameManagerView.owner, index, PhotonNetwork.player.ID);
    }
}
