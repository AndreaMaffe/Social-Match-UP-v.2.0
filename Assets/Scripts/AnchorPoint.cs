using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnchorPoint : MonoBehaviour
{
    public int Index { get; private set; }

    [PunRPC]
	public void SetIndex(int index)
    {
        Index = index;
    }
}
