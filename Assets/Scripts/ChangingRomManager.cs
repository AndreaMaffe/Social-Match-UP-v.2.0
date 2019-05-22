using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class ChangingRomManager : MonoBehaviour
{
    public GameObject[] choosableAvatars;
    public MirrorAvatar mirrorAvatar;

    private int page; //0 for the first set of avatars (0 -> 7), 8 for the second set (8 -> 15), ecc... 
    private int numberOfAvatars; //overall number of available avatars in Resources/Avatars folder
    private int chosenAvatarIndex;

	void Start ()
    {
        StartCoroutine(SwitchVROn());

        page = 0;
        numberOfAvatars = CountNumberOfAvatars();
        chosenAvatarIndex = -1;

        SetAvatars();
    }

    public void OnAvatarChosen(GameObject chosenAvatar)
    {
        chosenAvatarIndex = Array.IndexOf(choosableAvatars, chosenAvatar) + page;
        PlayerPrefs.SetInt("Avatar", chosenAvatarIndex);
        mirrorAvatar.UpdateAvatar();
        SetAvatars();
    }

    //called when one of the two arrows in the game is clicked, increments the value of page and loads the new set of avatars
    public void OnArrowButtonClicked(int direction)
    {
        page += direction * 8;

        if (page < 0)
            page = (numberOfAvatars/8 ) * 8;

        if (page > numberOfAvatars)
            page = 0;

        SetAvatars();
    }

    //generate the avatars on the stations
    void SetAvatars()
    {
        for (int i = 0; i < choosableAvatars.Length; i++)
        {
            if (choosableAvatars[i].transform.childCount != 0)
                Destroy(choosableAvatars[i].transform.GetChild(0).gameObject);

            try
            {
                GameObject avatar;

                if (i+page != chosenAvatarIndex)
                    avatar = Instantiate(Resources.Load<GameObject>("Avatars/Avatar_" + (i + page)), choosableAvatars[i].transform);
                else
                    avatar = Instantiate(Resources.Load<GameObject>("Avatars/BaseAvatar"), choosableAvatars[i].transform);

                avatar.transform.position = choosableAvatars[i].transform.position;
                Quaternion avatarRotation = Quaternion.LookRotation(-avatar.transform.position);
                avatar.transform.rotation = avatarRotation;
                avatar.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);

                choosableAvatars[i].GetComponent<ChoosableAvatar>().Active = true;
            }
            catch (ArgumentException e) { choosableAvatars[i].GetComponent<ChoosableAvatar>().Active = false; }

        }
    }

    //counts the number of Prefabs called "Avatar_[x]" in the Resources/Avatars folder
    int CountNumberOfAvatars()
    {
        int number = 0;

        while(Resources.Load<GameObject>("Avatars/Avatar_" + number) != null)
        {
            number++;
        }

        return number;
    }

    private IEnumerator SwitchVROn()
    {
        XRSettings.LoadDeviceByName("cardboard");
        yield return null;
        XRSettings.enabled = true;
    }

    private IEnumerator SwitchVROff()
    {
        XRSettings.LoadDeviceByName("None");
        yield return null;
        XRSettings.enabled = false;
    }


}
