using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private Transform camTransform;
    private Rigidbody rb;
    private bool dragging;
    private Vector3 lastFramePosition;

    private Color[] colors = new Color[] { Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.magenta, Color.white };

    void Start ()
    {
        camTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
	}	

	void Update ()
    {
        if (dragging)
        {
            transform.position = camTransform.position + camTransform.forward * (3+ Mathf.Abs(transform.position.x/2f));

            if (Mathf.Abs((transform.position - lastFramePosition).magnitude) < 0.05f)
                StartCoroutine("Drop");

            else StopAllCoroutines();

            lastFramePosition = transform.position;
        }
    }

    public void OnGazeEnter()
    {
        StartCoroutine("Drag");
    }

    public void OnGazeExit()
    {
        StopAllCoroutines();
    }

    private IEnumerator Drag()
    {
        yield return new WaitForSeconds(3);
        dragging = true;
    }

    private IEnumerator Drop()
    {
        yield return new WaitForSeconds(3);
        dragging = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AnchorPoint")
        {
            dragging = false;
            transform.position = other.transform.position;
            AudioManager.instance.PlayDingSound();
        }
    }
}
