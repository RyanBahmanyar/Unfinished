using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] string UITag = "MainUI";
    [SerializeField] float modifier = 1000;

    private GameObject mainUI;

    private void Start()
    {
        mainUI = GameObject.FindWithTag(UITag);
        SetChildrenActive(false);
    }

    private void SetChildrenActive (bool active)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(active);
        }
    }

    public void Open ()
    {
        GameManager.instance.Freeze();
        if (mainUI != null)
        {
            mainUI.SetActive(false);
        }
        if (mainUI != null)
        {
            text.text = "$" + (GameManager.instance.CurrentHighestScore * modifier).ToString();
        }
        SetChildrenActive(true);
    }

    public void Close ()
    {
        GameManager.instance.Unfreeze();
        if (mainUI != null)
        {
            mainUI.SetActive(true);
        }
        SetChildrenActive(false);
    }
}
