using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Image : MonoBehaviour {

    //from 0 to numberOfImages: used to check with image the player is looking at
    private int index;
    private Color originalColor;
    public bool IsGazed { get; set; }
    private PhotonView gameManagerView;


    private void Start()
    {
        IsGazed = false;
        gameManagerView = GameObject.Find("GameManager(Clone)").GetPhotonView();
    }

    [PunRPC]
	public void SetSprite(string multipleSpriteName, int index)
    {
        Sprite[] imageSprites = Resources.LoadAll<Sprite>(multipleSpriteName);
        GetComponent<SpriteRenderer>().sprite = imageSprites[index];
    }

    [PunRPC]
    public void ChangeCircleColor(string color)
    {
        if (color == "blue")
        transform.Find("Circle").GetComponent<SpriteRenderer>().color = Color.blue;
        if (color == "red")
            transform.Find("Circle").GetComponent<SpriteRenderer>().color = Color.red;
    }

    [PunRPC]
    public void SetIndex(int index)
    {
        this.index = index;
    }

    [PunRPC]
    public void AutoDestroy()
    {
        Destroy(this.gameObject);
    }

    public void OnEnterGaze()
    {
        gameManagerView.RPC("OnImageEnterGaze", this.gameObject.GetPhotonView().owner, index, PhotonNetwork.player.ID);
    }

    public void OnExitGaze()
    {
        gameManagerView.RPC("OnImageExitGaze", this.gameObject.GetPhotonView().owner, index, PhotonNetwork.player.ID);
    }
}
