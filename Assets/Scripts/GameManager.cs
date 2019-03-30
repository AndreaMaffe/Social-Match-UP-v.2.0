using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Photon.MonoBehaviour
{
    private HeadsetPlayer[] players;
    private GameObject thisPlayer;

    private Dictionary<int, GameObject[]> images;

    private void Start()
    {
        
        Debug.Log("Tipo di immagini: " + PhotonManager.instance.ImageType + " - Numero di immagini: " + PhotonManager.instance.NumberOfImages);
        StartCoroutine("SetUpGame");
        
    }

    //generate a number of images equal to variable NumberOfImages for both player in randomic position around them.
    //The images are chosen randomly in the set of multiple sprites.
    private void SpawnRandomImages()
    {
        images = new Dictionary<int, GameObject[]>();
        
        int numberOfPossibleImages = Resources.LoadAll<Sprite>(PhotonManager.instance.ImageType).Length;

        //generate a list with random indexes of the sprites
        List<int> chosenImages = new List<int>();

        while(chosenImages.Count < PhotonManager.instance.NumberOfImages)
        {
            //generate random index
            int randomIndex = Random.Range(0, numberOfPossibleImages);
            //check it's not already taken, otherwise add it
            if (!chosenImages.Contains(randomIndex))
                chosenImages.Add(randomIndex);
        }
        
        foreach (HeadsetPlayer player in players)
        {
            images.Add(player.gameObject.GetPhotonView().ownerId, new GameObject[PhotonManager.instance.NumberOfImages]);

            for (int i = 0; i < PhotonManager.instance.NumberOfImages; i++)
            {
                Vector3 imagePosition = player.transform.position + Random.insideUnitSphere * 3;
                Quaternion imageRotation = Quaternion.LookRotation(player.transform.position - imagePosition);
                GameObject image = PhotonNetwork.Instantiate("Image", imagePosition, imageRotation, 0);
                image.GetPhotonView().RPC("SetSprite", PhotonTargets.All, PhotonManager.instance.ImageType, chosenImages[i]);
                image.GetPhotonView().RPC("SetIndex", PhotonTargets.All, i);

                if (player.gameObject.GetPhotonView().isMine)
                    image.GetPhotonView().RPC("ChangeCircleColor", PhotonTargets.All, 0f, 0f, 255f);
                else image.GetPhotonView().RPC("ChangeCircleColor", PhotonTargets.All, 255f, 0f, 0f);

                images[player.gameObject.GetPhotonView().ownerId][i] = image;
            }
        }    

    }

    //called when a player starts looking at a image
    [PunRPC] 
    public void OnImageEnterGaze(int imageIndex, int playerId)
    {
        Debug.Log("Player " + playerId + " starts looking at image " + imageIndex);

        images[playerId][imageIndex].GetComponent<Image>().IsGazed = true;


        bool sameImageGazed = true;
        foreach (int player in images.Keys)
            if (images[player][imageIndex].GetComponent<Image>().IsGazed == false)
                sameImageGazed = false;

        if (sameImageGazed)
            StartCoroutine("DestroyImage", imageIndex);
    }

    //called when a player stops looking at a image
    [PunRPC]
    public void OnImageExitGaze(int imageIndex, int playerId)
    {
        try
        {
            Debug.Log("Player " + playerId + " stops looking at image " + imageIndex);
            images[playerId][imageIndex].GetComponent<Image>().IsGazed = false;
        } catch (System.NullReferenceException e) { }

        StopAllCoroutines();

        foreach (int player in images.Keys)
            images[player][imageIndex].GetPhotonView().RPC("StopAnimation", PhotonTargets.All);
    }

    IEnumerator DestroyImage(int imageIndex)
    {
        Debug.Log("Destroying image " + imageIndex + "...");

        foreach (int player in images.Keys)
            images[player][imageIndex].GetPhotonView().RPC("StartAnimation", PhotonTargets.All);

        yield return new WaitForSeconds(5);

        foreach (int player in images.Keys)
            images[player][imageIndex].GetPhotonView().RPC("AutoDestroy", PhotonTargets.All);
    }

    IEnumerator SetUpGame()
    {
        yield return new WaitForSeconds(3);

        //find the players in the scene
        players = FindObjectsOfType<HeadsetPlayer>(); //ATTENZIONE! questo metodo deve essere chiamato solo dopo che tutti i giocatori sono stai istanziati

        //only the main GameManager (not its PhotonViews) must spawn the images
        if (this.gameObject.GetPhotonView().isMine)
            SpawnRandomImages();
           
    }
}
