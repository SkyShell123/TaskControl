using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using static UnityEditor.Progress;

[System.Serializable]
public class FormDataList
{
    public List<FormData> items;
}

public class DynamicContentScript : MonoBehaviour
{
    public GameObject itemPrefab;  // Префаб элемента, который будет повторяться

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
        items.RemoveAt(_id);
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

        Transform contentTransform = GetComponentInChildren<GridLayoutGroup>().transform;

        GameObject newItem = Instantiate(itemPrefab, contentTransform);
        newItem.GetComponent<PanelData>().LoadData(_items.id, _items.name, _items.duration,"");
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


        // Получаем ссылку на контент
        Transform contentTransform = GetComponentInChildren<GridLayoutGroup>().transform;

        // Создаем нужное количество элементов
        for (int i = 0; i < items.Count; i++)
        {
            // Создаем экземпляр элемента из префаба
            GameObject newItem = Instantiate(itemPrefab, contentTransform);
            newItem.GetComponent<PanelData>().LoadData(items[i].id, items[i].name, items[i].duration, "");
            panels.Add(newItem);
        }
    }
}
