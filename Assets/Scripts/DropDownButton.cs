using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropDownButton : MonoBehaviour {

    public TextMeshProUGUI parentButtonLabel;
    private TextMeshProUGUI thisText;

    private void Start()
    {
        thisText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }

    public void ChangeParentButtonLabel()
    {
        parentButtonLabel.text = thisText.text;
    }
}
