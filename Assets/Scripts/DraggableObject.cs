using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableObject : MonoBehaviour
{
    private Transform camTransform;
    private Rigidbody rb;
    private bool dragging;
    private bool droppable;
    private Vector3 lastFramePosition;
    private Vector3 lastAnchorPointPosition;
    private int index;
    private PhotonView gameManagerView;

    private GameObject anchorPoint;
    public GameObject light;
    private Color[] colors = new Color[] { Color.red, Color.green, Color.blue, Color.yellow, Color.cyan, Color.magenta, Color.white };

    void Start ()
    {
        camTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        droppable = true;
        lastAnchorPointPosition = Vector3.zero;

        //disable the script and the event trigger if the object is not mine (in this way the other player cannot drag it)
        if (GetComponent<PhotonView>().isMine == false)
        {
            this.GetComponent<EventTrigger>().enabled = false;
            this.enabled = false;
        }
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

            if (Mathf.Abs((transform.position - lastFramePosition).magnitude) < 0.06f)
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

        if (Vector3.Distance(this.transform.position, lastAnchorPointPosition) > 1.3f)
            droppable = true;
            
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
        AudioManager.instance.PlayPopSound();
        dragging = true;
        light.SetActive(true);

        if (anchorPoint != null && gameObject.GetPhotonView().isMine)
        {
            gameManagerView.RPC("OnObjectRemoved", gameManagerView.owner, PhotonNetwork.player.ID, anchorPoint.GetComponent<AnchorPoint>().Index);
            anchorPoint.GetComponent<AnchorPoint>().anchoredObject = null;
            anchorPoint = null;
        }

    }

    private IEnumerator Drop()
    {
        yield return new WaitForSeconds(5);
        dragging = false;
        light.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "AnchorPoint" && gameObject.GetPhotonView().isMine && other.GetComponent<AnchorPoint>().anchoredObject == null && droppable)
        {
            dragging = false;
            light.SetActive(false);
            AudioManager.instance.PlayDingSound();
            other.GetComponent<AnchorPoint>().anchoredObject = this.gameObject;
            this.anchorPoint = other.gameObject;
            this.lastAnchorPointPosition = other.transform.position;
            gameManagerView.RPC("OnObjectPositioned", PhotonTargets.All, PhotonNetwork.player.ID, this.index, other.gameObject.GetComponent<AnchorPoint>().Index);
            droppable = false;
        }
    }

    [PunRPC]
    public void SetIndex(int index)
    {
        this.index = index;
        GetComponent<MeshRenderer>().material.color = colors[index];
        transform.Find("Tip").gameObject.GetComponent<MeshRenderer>().material.color = colors[index];
        light.SetActive(true);
        light.GetComponent<Light>().color = colors[index];
        light.SetActive(false);
    }
}
