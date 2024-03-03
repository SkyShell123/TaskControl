using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Progress;

public class PanelData : MonoBehaviour
{
    public TextMeshProUGUI panelName;
    public TextMeshProUGUI panelDuration;

    public int id;

    public int id_order;

    public string type="";

    public void LoadData(int _id, string _name, string _duration, string _type)
    {
        id = _id;
        panelName.text = _name;
        panelDuration.text = _duration;
        type=_type;
    }

    public void DeliteTask()
    {
        if (type == "")
        {
            DynamicContentScript.Instance.DeliteTask(id);
            Destroy(gameObject);
        }
        else if (type == "left") 
        {
            LeftDynamicContentScript.Instance.DeliteTask(id);
            Destroy(gameObject);
        }
    }
}
