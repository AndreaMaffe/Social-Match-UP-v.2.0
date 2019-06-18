using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAvatar1 : MonoBehaviour
{
    private float initialX;
    private float inclination;

    private void Start()
    {
        initialX = this.transform.position.x;
        inclination = Random.Range(-0.01f, 0.01f);
    }

    private void Update()
    {
        transform.Rotate(new Vector3(0, 2, 0));
        transform.position += new Vector3(-0.018f, inclination, 0);

        if (transform.position.x < -initialX)
        {
            transform.position = new Vector3(initialX, transform.position.y, transform.position.z);
            inclination = Random.Range(-0.01f, 0.01f);

            if (transform.position.y > Camera.main.orthographicSize +2|| transform.position.y < -Camera.main.orthographicSize-2)
                transform.position = new Vector3(transform.position.x, 0, 80);

        }


    }

}
