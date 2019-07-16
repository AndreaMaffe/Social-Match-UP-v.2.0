using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//class for the object used in Sorting game mode. Provides methods to drag and drop the object and to notify the 
// SortingGameManager wether the object is placed on a AnchorPoint
public class DraggableObject : MonoBehaviour
{
    private Transform camTransform;
    private Rigidbody rb;
    private bool dragging;
    private bool droppable;
    private Vector3 lastFramePosition; //used to check if the player is moving the object
    private Vector3 lastAnchorPointPosition;
    private int index; //ID of the object, to distinguish it between the others
    private GameObject gameManager; //reference to the manager, needed to send info on the network e check wether the player is dragging an object

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

            //if the player stops moving, drops the object
            if (Mathf.Abs((transform.position - lastFramePosition).magnitude) < 0.06f)
                StartCoroutine("Drop");
            else StopAllCoroutines();

            lastFramePosition = transform.position;
        }


        if (gameManager == null)
            try
            {
                gameManager = GameObject.Find("SortingGameManager(Clone)");
            }
            catch (System.NullReferenceException e) { Debug.Log("Manager not found!"); }

        if (Vector3.Distance(this.transform.position, lastAnchorPointPosition) > 1)
            droppable = true;
            
    }

    //called when the player starts looking at the object
    public void OnGazeEnter()
    {
        if (gameManager.GetComponent<SortingGameManager>().IsPlayerDragging == false) //if the player is not already dragging an object
            StartCoroutine("Drag");
    }

    //called when the player stops looking at the object
    public void OnGazeExit()
    {
        StopAllCoroutines();
    }

    //wait a few seconds, then drag the object and notifies the SortingGameManager
    private IEnumerator Drag()
    {
        yield return new WaitForSeconds(2.5f);
        AudioManager.instance.PlayPopSound();
        dragging = true;
        gameManager.GetComponent<SortingGameManager>().IsPlayerDragging = true;
        light.SetActive(true); 

        if (anchorPoint != null && gameObject.GetPhotonView().isMine)
        {
            gameManager.GetPhotonView().RPC("OnObjectRemoved", gameManager.GetPhotonView().owner, PhotonNetwork.player.ID, anchorPoint.GetComponent<AnchorPoint>().Index);
            anchorPoint.GetComponent<AnchorPoint>().anchoredObject = null;
            anchorPoint = null;
        }
    }

    private IEnumerator Drop()
    {
        yield return new WaitForSeconds(5);
        dragging = false;
        light.SetActive(false);
        gameManager.GetComponent<SortingGameManager>().IsPlayerDragging = false; //now the player is free to pick up a new object
    }

    private void OnTriggerEnter(Collider other)
    {
        //if collides with the AnchorPoint, stick it to it and notifies the SortingGameManager
        if (other.tag == "AnchorPoint" && gameObject.GetPhotonView().isMine && other.GetComponent<AnchorPoint>().anchoredObject == null && droppable)
        {
            dragging = false;
            light.SetActive(false);
            AudioManager.instance.PlayDingSound();
            other.GetComponent<AnchorPoint>().anchoredObject = this.gameObject;
            this.anchorPoint = other.gameObject;
            this.lastAnchorPointPosition = other.transform.position;
            droppable = false;
            gameManager.GetComponent<SortingGameManager>().IsPlayerDragging = false; //now the player is free to pick up a new object

            //notifies the SortingGameManager
            gameManager.GetPhotonView().RPC("OnObjectPositioned", PhotonTargets.All, PhotonNetwork.player.ID, this.index, other.gameObject.GetComponent<AnchorPoint>().Index);
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
