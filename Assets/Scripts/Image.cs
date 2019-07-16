using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//class for the each Image used in Classic game mode. Provides methods to send info to the ClassicGameManager wether the players
//looks at it and to change the sprite accordingly
public class Image : MonoBehaviour
{
    private int index; //from 0 to numberOfImages: used to check with image the player is looking at
    private Color originalColor;
    private Transform circle;
    private GameObject goldenParticle; //golden particles effect, activated when both players look at the same image
    private PhotonView gameManagerView; //view of the ClassicGameManager, needed to call its methods remotely
    public Animator animator;

    public bool IsGazed { get; set; }

    private void Start()
    {
        if (this.gameObject.GetPhotonView().isMine == false) 
            GetComponent<EventTrigger>().enabled = false;  //in this way, the image triggers only when it's gazed by the correct player

        IsGazed = false;
        goldenParticle = transform.Find("GoldenParticles").gameObject;
        gameManagerView = GameObject.Find("ClassicGameManager(Clone)").GetPhotonView();
    }

    //the sprite of the image is remotely set by the ClassicGameManager when the image is created. See ClassicGameManager.SpawnRandomImages()
    [PunRPC]
	public void SetSprite(string multipleSpriteName, int index)
    {
        Sprite[] imageSprites = Resources.LoadAll<Sprite>(multipleSpriteName);
        GetComponent<SpriteRenderer>().sprite = imageSprites[index];
    }

    //the color of the circle is remotely set by the ClassicGameManager when the image is created. See ClassicGameManager.SpawnRandomImages()
    [PunRPC]
    public void ChangeCircleColor(float r, float g, float b)
    {
        this.originalColor = new Color(r, g, b); //cannot directly pass object of type Color with a RPC call over the network
        transform.Find("Circle").GetComponent<SpriteRenderer>().color = this.originalColor;
    }

    //the index of the image is remotely set by the ClassicGameManager when the image is created. See ClassicGameManager.SpawnRandomImages()
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

    //called when the player starts looking at the image
    public void OnEnterGaze()
    {
            animator.SetBool("Gazed", true); //start the animation
            gameManagerView.RPC("OnImageEnterGaze", gameManagerView.owner, index, PhotonNetwork.player.ID);
    }

    //called when the player stops looking at the image
    public void OnExitGaze() 
    {
            animator.SetBool("Gazed", false); //stops the animation
            gameManagerView.RPC("OnImageExitGaze", gameManagerView.owner, index, PhotonNetwork.player.ID);
    }
}
