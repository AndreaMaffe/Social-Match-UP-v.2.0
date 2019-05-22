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
        SceneManager.LoadScene("MainMenu");
    }

}
