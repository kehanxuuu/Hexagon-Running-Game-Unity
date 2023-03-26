using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WrongDirection : MonoBehaviour
{
    public Text wrongDir;
    bool visible;
    int count = 0;

    void Awake()
    {
        wrongDir.gameObject.SetActive(false);
        visible = false;
    }

    void Update()
    {
        if (visible)
        {
            if (count%100<50)
                wrongDir.gameObject.SetActive(true);
            else wrongDir.gameObject.SetActive(false);
            count += 1;
        }
    }
    
    public void ShowWarning()
    {
        visible = true;
        count = 0;
    }

    public void StopWarning()
    {
        visible = false;
        count = 0;
    }
}
