using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

//Assigned to the Exit door in the ChangingRoom. Simply go back to the MainMenu when gazed for a few seconds
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
        SceneManager.LoadScene("MainMenu");
    }

}
