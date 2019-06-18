using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClassicGameManager : GameManager
{

    private Dictionary<int, GameObject[]> images;     //Dictionary that pairs each playerId with the array of images of that player
    private int numberOfDestroyedImages;              //  e.g:        <ID Player1> | [img0, img1, img2, img3]
                                                      //              <ID Player2> | [img0, img1, img2, img3]
                                                      // used to check if the players are looking at the same image
    private void Start()
    {
        numberOfDestroyedImages = 0;
        AudioManager.instance.PlayBackgroundMusic();      
    }

        //generate a number of images equal to variable NumberOfImages for both player in randomic position around them.
        //The images are chosen randomly from the set of multiple sprites decided before launching the task.
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
        
        foreach (GameObject player in players)
        {
            images.Add(player.gameObject.GetPhotonView().ownerId, new GameObject[PhotonManager.instance.NumberOfImages]);

            for (int i = 0; i < PhotonManager.instance.NumberOfImages; i++)
            {
                Vector3 imagePosition = player.transform.position + Random.onUnitSphere*2;
                Quaternion imageRotation = Quaternion.LookRotation(player.transform.position - imagePosition);
                GameObject image = PhotonNetwork.Instantiate("Image", imagePosition, imageRotation, 0);
                image.GetPhotonView().RPC("SetSprite", PhotonTargets.All, PhotonManager.instance.ImageType, chosenImages[i]);
                image.GetPhotonView().RPC("SetIndex", PhotonTargets.All, i);

                if (player.GetPhotonView().isMine)
                    image.GetPhotonView().RPC("ChangeCircleColor", PhotonTargets.All, 0f, 0f, 255f);
                else
                {
                    image.GetPhotonView().RPC("ChangeCircleColor", PhotonTargets.All, 255f, 0f, 0f);
                    image.GetPhotonView().TransferOwnership(player.GetPhotonView().owner);
                }

                images[player.GetPhotonView().ownerId][i] = image;
            }
        }    

    }

    //called when a player starts looking at a image
    [PunRPC] 
    public void OnImageEnterGaze(int imageIndex, int playerId)
    {
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
            images[player][imageIndex].GetPhotonView().RPC("StopDestroyAnimation", PhotonTargets.All);
    }

    //start the image animation, wait 5 secs and destroy the image. It is stopped by OnImageExitgaze
    IEnumerator DestroyImage(int imageIndex)
    {
        foreach (int player in images.Keys)
            images[player][imageIndex].GetPhotonView().RPC("StartDestroyAnimation", PhotonTargets.All);

        yield return new WaitForSeconds(5);

        foreach (int player in images.Keys)
            images[player][imageIndex].GetPhotonView().RPC("AutoDestroy", PhotonTargets.All);

        numberOfDestroyedImages++;

        if (numberOfDestroyedImages == PhotonManager.instance.NumberOfImages) 
            this.gameObject.GetPhotonView().RPC("StartVictoryAnimations", PhotonTargets.All);
    }

    [PunRPC]
    public void StartVictoryAnimations()
    {
        StartCoroutine(OnVictory());
    }

    IEnumerator OnVictory()
    {
        yield return new WaitForSeconds(1);
        AudioManager.instance.PlayHurraySound();
        yield return new WaitForSeconds(1);
        //AudioManager.instance.PlayFireworksSound();
        //Instantiate(Resources.Load<GameObject>("Fireworks"), Vector3.up * 30, Quaternion.identity);
        yield return new WaitForSeconds(3);
        AudioManager.instance.StopBackgroundMusic();
        yield return new WaitForSeconds(1);
        AudioManager.instance.PlayVictorySound();

        SpriteRenderer endGamePanel = thisPlayer.transform.Find("BlackPanel").GetComponent<SpriteRenderer>();
        endGamePanel.color = Color.black;
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene("MainMenu");
    }

    protected override void SetUpGame()
    {
        if (this.gameObject.GetPhotonView().isMine) //only the main GameManager must Spawn the Images
        {
            SpawnRandomImages();
        }
    }
}
