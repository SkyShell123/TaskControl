using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using static UnityEditor.Progress;
using System.Linq;
using Unity.VisualScripting;

public class FormController : MonoBehaviour
{
    public TMP_InputField inputField1;
    public TMP_InputField inputField2;

    private int _id;

    public void SaveData()
    {
        float.TryParse(inputField2.text, out float floatValue);

        if (DynamicContentScript.Instance.items.Count>0)
        {
            _id = DynamicContentScript.Instance.items.Last().id++;
        }
        else 
        { 
            _id = 0;
        }

        FormData newItem = new()
        {
            id = _id,
            name = inputField1.text,

            duration = floatValue,
        };

        DynamicContentScript.Instance.AddTask(newItem);

        Destroy(gameObject);
    }
}
