﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Photon.MonoBehaviour
{
    private HeadsetPlayer[] players;
    private GameObject thisPlayer;
    private int numberOfImages = 5;

    private GameObject goldParticles;

    private Dictionary<int, GameObject[]> images;

    private void Start()
    {
        //find the players in the scene
        players = FindObjectsOfType<HeadsetPlayer>(); //ATTENZIONE! questo metodo deve essere chiamato solo dopo che tutti i giocatori sono stai istanziati

        Debug.Log(players.Length);

        //only the main GameManager (not its PhotonViews) must spawn the images
        if (this.gameObject.GetPhotonView().isMine)
            SpawnRandomImages();
    }

    //generate a number of images equal to variable numberOfImages for both player in randomic position around them.
    //The images are chosen randomly in the set of multiple sprites.
    private void SpawnRandomImages()
    {
        images = new Dictionary<int, GameObject[]>();

        int numberOfPossibleImages = Resources.LoadAll<Sprite>("Pokemons").Length;

        //generate a list with random indexes of the sprites
        List<int> chosenImages = new List<int>();
        while(chosenImages.Count < numberOfImages)
        {
            //generate random index
            int randomIndex = Random.Range(0, numberOfPossibleImages);
            //check it's not already taken, otherwise add it
            if (!chosenImages.Contains(randomIndex))
                chosenImages.Add(randomIndex);
        }

        foreach (HeadsetPlayer player in players)
        {
            images.Add(player.gameObject.GetPhotonView().ownerId, new GameObject[numberOfImages]);

            for (int i = 0; i < numberOfImages; i++)
            {
                Vector3 imagePosition = player.transform.position + Random.insideUnitSphere * 3;
                Quaternion imageRotation = Quaternion.LookRotation(player.transform.position - imagePosition);
                GameObject image = PhotonNetwork.Instantiate("Image", imagePosition, imageRotation, 0);
                image.GetPhotonView().RPC("SetSprite", PhotonTargets.All, "Pokemons", chosenImages[i]);
                image.GetPhotonView().RPC("SetIndex", PhotonTargets.All, i);

                if (player.gameObject.GetPhotonView().isMine)
                    image.GetPhotonView().RPC("ChangeCircleColor", PhotonTargets.All, "blue");
                else image.GetPhotonView().RPC("ChangeCircleColor", PhotonTargets.All, "red");

                images[player.gameObject.GetPhotonView().ownerId][i] = image;
            }
        }


    }

    //called when a player starts looking at a image
    [PunRPC] 
    public void OnImageEnterGaze(int imageIndex, int playerId)
    {
        Debug.Log("Player " + playerId + " starts looking at image " + imageIndex);
        bool sameImageGazed = true;

        images[playerId][imageIndex].GetComponent<Image>().IsGazed = true;

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

        Debug.Log("STOP!");

        StopAllCoroutines();
        //PhotonNetwork.Destroy(goldParticles);
    }

    IEnumerator DestroyImage(int imageIndex)
    {
        Debug.Log("Destroying image " + imageIndex + "...");

        foreach (int player in images.Keys)
            goldParticles = PhotonNetwork.Instantiate("GoldParticles", images[player][imageIndex].transform.position, Quaternion.identity, 0);

        yield return new WaitForSeconds(5);

        foreach (int player in images.Keys)
            images[player][imageIndex].GetPhotonView().RPC("AutoDestroy", PhotonTargets.All);
    }
}
