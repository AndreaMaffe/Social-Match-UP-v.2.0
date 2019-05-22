using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkElement : MonoBehaviour
{
    private void Update()
    {
        if (PhotonNetwork.connected)
            this.gameObject.SetActive(false);
        else
            this.gameObject.SetActive(true);
    }
}
