using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Image : MonoBehaviour {

    private int index; //from 0 to numberOfImages: used to check with image the player is looking at
    private Color originalColor;
    private Transform circle;
    private GameObject goldenParticle;
    private PhotonView gameManagerView;

    public bool IsGazed { get; set; }


    private void Start()
    {
        IsGazed = false;

        goldenParticle = transform.Find("GoldenParticles").gameObject;
        gameManagerView = GameObject.Find("GameManager(Clone)").GetPhotonView();
    }

    [PunRPC]
	public void SetSprite(string multipleSpriteName, int index)
    {
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
    public void StartAnimation()
    {
        transform.Find("Circle").GetComponent<SpriteRenderer>().color = Color.yellow;
        goldenParticle.SetActive(true);
    }

    [PunRPC]
    public void StopAnimation()
    {
        transform.Find("Circle").GetComponent<SpriteRenderer>().color = this.originalColor;
        goldenParticle.SetActive(false);
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
