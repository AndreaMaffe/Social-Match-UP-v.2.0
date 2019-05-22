using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPoint : MonoBehaviour
{
    public int Index { get; private set; } //index used by the OrderingGameManager to keep trace of the position

    [SerializeField]
    public GameObject anchoredObject; //current object attached to the anchorPoint

    private void Update()
    {
        //keep the object freezed in the same position (a little above the table surface)
        if (anchoredObject != null)
            anchoredObject.transform.position = this.transform.position + new Vector3(0f, 0.35f, 0f);
    }

    [PunRPC]
    public void SetIndex(int index)
    {
        Index = index;
    }
}
