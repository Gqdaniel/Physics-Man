using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusTextManager : MonoBehaviour
{
    public static StatusTextManager instance;

    public Text statusText;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }
    }

    private void Start()
    {
        toggleStatusText(false);
    }

    public void updateStatusText(string message)
    {
        statusText.text = message;
        toggleStatusText(true);
    }

    public void toggleStatusText(bool status)
    {
        statusText.enabled = status;
    }
}
