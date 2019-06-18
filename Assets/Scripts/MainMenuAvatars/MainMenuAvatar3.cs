using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAvatar3 : MonoBehaviour
{
    private float timer;
    public float angle;
    private float screenWidth, screenHeight;

    private void Start()
    {
        screenHeight = Camera.main.orthographicSize * 2;
        screenWidth = screenHeight * Camera.main.aspect;

        timer = Random.Range(6, 9);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            angle += Random.Range(-90, 90);
            transform.rotation = Quaternion.LookRotation(new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle) * 0.02f, Mathf.Sin(Mathf.Deg2Rad * angle) * 0.02f, 0));
            timer = Random.Range(6,9);
        }

        transform.position += new Vector3(Mathf.Cos(Mathf.Deg2Rad*angle)*0.02f, Mathf.Sin(Mathf.Deg2Rad*angle)*0.02f, 0);

        if (transform.position.x > screenWidth / 2 + 2)
            transform.position = new Vector3(-screenWidth / 2 - 2, transform.position.y, transform.position.z);
        if (transform.position.x < -screenWidth / 2 - 2)
            transform.position = new Vector3(screenWidth / 2 + 2, transform.position.y, transform.position.z);
        if (transform.position.y > screenHeight / 2 + 2)
            transform.position = new Vector3(transform.position.x, -screenHeight / 2 - 2, transform.position.z);
        if (transform.position.y < -screenHeight / 2 - 2)
            transform.position = new Vector3(transform.position.x, screenHeight / 2 + 2, transform.position.z);
    }
}
