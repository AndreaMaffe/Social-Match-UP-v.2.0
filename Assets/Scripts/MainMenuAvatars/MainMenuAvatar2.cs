using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAvatar2 : MonoBehaviour
{
    private int direction;
    private float screenWidth, screenHeight;
    private float angle;

    private void Start()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;

        float randomYPosition = Random.Range(-screenHeight / 2 + 2, +screenHeight / 2 - 2);

        this.transform.position = new Vector3(transform.position.x, randomYPosition, 85);
        direction = 1;
        angle = 0;
    }

    private void Update()
    {
        this.transform.position += new Vector3(direction * 0.02f, Mathf.Sin(angle)*0.075f, 0);
        angle+=0.09f;

        if (transform.position.x > screenWidth / 2 +5)
        {
            direction = -1;
            transform.rotation = Quaternion.Euler(0, -90, 0);
            float randomYPosition = Random.Range(-screenHeight / 2, +screenHeight / 2);
            this.transform.position = new Vector3(this.transform.position.x, randomYPosition, 85);

        }
        else if (transform.position.x < -screenWidth / 2 -5)
        {
            direction = +1;
            transform.rotation = Quaternion.Euler(0, 90, 0);
            float randomYPosition = Random.Range(-screenHeight / 2, +screenHeight / 2);
            this.transform.position = new Vector3(this.transform.position.x, randomYPosition, 85);
        }
    }

}
