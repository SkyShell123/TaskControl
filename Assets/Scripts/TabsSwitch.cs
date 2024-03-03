using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TabsSwitch : MonoBehaviour
{
    public GameObject[] Tabs;

    public void Switch()
    {
        if (Tabs[0].activeSelf)
        {
            Tabs[0].SetActive(false);
            Tabs[1].SetActive(true);
        }
        else
        {
            Tabs[1].SetActive(false);
            Tabs[0].SetActive(true);
        }
    }

    public void Done()
    {

    }

    public void SwitchFlag()
    {
        //GameObject flagObject = gameObject.transform.GetChild(0).gameObject;

        //if (flagObject.activeSelf)
        //{
        //    flagObject.SetActive(false);
        //}
        //else
        //{
        //    flagObject.SetActive(true);
        //}
        
    }
}
