using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DropDownButton : MonoBehaviour {

    private GameObject parentButton;
    private GameObject parentPanel;
    private TextMeshProUGUI thisText;
    private GameObject tick;

    private void Start()
    {
        parentButton = transform.parent.parent.gameObject;
        parentPanel = transform.parent.gameObject;
        thisText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        tick = transform.Find("Tick").gameObject;
    }

    public void OnClick()
    {
        parentButton.transform.Find("Label").GetComponent<TextMeshProUGUI>().text = thisText.text;
        parentButton.transform.Find("ArrowDown").gameObject.SetActive(true);
        parentPanel.SetActive(false);

        foreach (Transform button in this.transform.parent)
                button.Find("Tick").gameObject.SetActive(false);

        tick.SetActive(true);
    }


}
