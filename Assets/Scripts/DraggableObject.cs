using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : MonoBehaviour
{
    private Transform camTransform;
    private Rigidbody rb;
    private bool dragging;
    private Vector3 lastFramePosition;

    [SerializeField]
    private int index;

    private PhotonView gameManagerView;

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
            //follow the camera movements
            transform.position = camTransform.position + camTransform.forward * (3+ Mathf.Abs(transform.position.x/2f));

            //avoid going under y = 0.7 (otherwise it will go under the pavement)
            if (transform.position.y < 0.7f)
                transform.position = new Vector3(transform.position.x, 0.7f, transform.position.z);

            if (Mathf.Abs((transform.position - lastFramePosition).magnitude) < 0.05f)
                StartCoroutine("Drop");

            else StopAllCoroutines();

            lastFramePosition = transform.position;
        }


        if (gameManagerView == null)
            try
            {
                gameManagerView = GameObject.Find("OrderingGameManager(Clone)").GetPhotonView();
            }
            catch (System.NullReferenceException e) { Debug.Log("Manager not found!"); }

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
        if (other.tag == "AnchorPoint" && gameObject.GetPhotonView().isMine)
        {
            dragging = false;
            transform.position = other.transform.position;
            AudioManager.instance.PlayDingSound();
            gameManagerView.RPC("OnObjectPositioned", gameManagerView.owner, PhotonNetwork.player.ID, this.index, other.gameObject.GetComponent<AnchorPoint>().Index);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "AnchorPoint" && gameObject.GetPhotonView().isMine)
        {
            gameManagerView.RPC("OnObjectRemoved", gameManagerView.owner, PhotonNetwork.player.ID, other.gameObject.GetComponent<AnchorPoint>().Index);
        }
    }

    [PunRPC]
    public void SetIndex(int index)
    {
        this.index = index;
        GetComponent<MeshRenderer>().material.color = colors[index];
        transform.Find("Tip").gameObject.GetComponent<MeshRenderer>().material.color = colors[index];
    }
}
