using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class Exit : MonoBehaviour
{
    public void OnGazeEnter()
    {
        StartCoroutine("BackToMainMenu");
    }

    public void OnGazeExit()
    {
        StopAllCoroutines();
    }

    IEnumerator BackToMainMenu()
    {
        yield return new WaitForSeconds(3);
        //XRSettings.LoadDeviceByName("None");
        //yield return null;
        //XRSettings.enabled = false;
        SceneManager.LoadScene("MainMenu");
    }

}
