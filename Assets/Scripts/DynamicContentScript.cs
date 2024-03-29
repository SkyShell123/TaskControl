using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using static UnityEditor.Progress;
using UnityEngine.UIElements;

[System.Serializable]
public class FormDataList
{
    public List<FormData> items;
}

public class DynamicContentScript : MonoBehaviour
{
    public GameObject itemPrefab;  // ������ ��������, ������� ����� �����������

    public List<FormData> items = new();

    public List<GameObject> panels = new();

    public static DynamicContentScript Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadData();
        PopulateScrollView();
    }

    public void DeliteTask(int _id) 
    {
        FormData formData = new();
        foreach (FormData item in items)
        {
            if (item.id == _id)
            {
                formData = item;

            }
        }
        items.Remove(formData);
        UpdateData();
    }

    public void UpdateData() 
    {
        //for (int i = 0; i < items.Count; i++)
        //{
        //    items[i].id = i;
        //}

        

        FormDataList formDataList = new()
        {
            items = items
        };

        string json = JsonUtility.ToJson(formDataList);
        File.WriteAllText("playerData.json", json);
    }

    public void AddTask(FormData _items)
    {
        items.Add(_items);
        UpdateData();

        Transform contentTransform = GetComponentInChildren<VerticalLayoutGroup>().transform;

        GameObject newItem = Instantiate(itemPrefab, contentTransform);
        newItem.GetComponent<PanelData>().LoadData(_items.id, _items.id_order, _items.name, _items.duration,"");
    }

    public void LoadData()
    {
        string filePath = "playerData.json";

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            FormDataList formDataList = JsonUtility.FromJson<FormDataList>(jsonData);

            items = formDataList.items;
        }
        else
        {
            Debug.LogWarning("File not found");
        }
    }

    public void PopulateScrollView()
    {
        foreach (var item in panels) 
        {
            Destroy(item);
        }
        panels.Clear();


        // �������� ������ �� �������
        Transform contentTransform = GetComponentInChildren<VerticalLayoutGroup>().transform;

        // ������� ������ ���������� ���������
        for (int i = 0; i < items.Count; i++)
        {
            // ������� ��������� �������� �� �������
            GameObject newItem = Instantiate(itemPrefab, contentTransform);
            newItem.GetComponent<PanelData>().LoadData(items[i].id, items[i].id_order, items[i].name, items[i].duration, "");
            panels.Add(newItem);
        }
    }
}
