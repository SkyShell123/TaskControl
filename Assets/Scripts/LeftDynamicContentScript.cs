using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using static UnityEditor.Progress;
using UnityEngine.UIElements;

[System.Serializable]
public class FormDataListLeft
{
    public List<FormData> itemsLeft;
}

public class LeftDynamicContentScript : MonoBehaviour
{
    public GameObject itemPrefab;  // Префаб элемента, который будет повторяться

    public List<FormData> itemsLeft = new();

    public static LeftDynamicContentScript Instance;

    private List<GameObject> panels = new();

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
        itemsLeft.RemoveAt(_id);
        UpdateData();
    }

    public void UpdateData()
    {
        //for (int i = 0; i < items.Count; i++)
        //{
        //    items[i].id = i;
        //}

        FormDataListLeft formDataList = new()
        {
            itemsLeft = itemsLeft
        };

        string json = JsonUtility.ToJson(formDataList);
        File.WriteAllText("playerDataLeft.json", json);
    }

    public void AddTask(FormData _items)
    {
        itemsLeft.Add(_items);
        UpdateData();

        Transform contentTransform = GetComponentInChildren<GridLayoutGroup>().transform;

        GameObject newItem = Instantiate(itemPrefab, contentTransform);
        newItem.GetComponent<PanelData>().LoadData(_items.id, _items.name, _items.duration, "left");
    }

    public void LoadData()
    {
        string filePath = "playerDataLeft.json";

        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            FormDataListLeft formDataList = JsonUtility.FromJson<FormDataListLeft>(jsonData);

            itemsLeft = formDataList.itemsLeft;
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
        for (int i = 0; i < itemsLeft.Count; i++)
        {
            // Создаем экземпляр элемента из префаба
            GameObject newItem = Instantiate(itemPrefab, contentTransform);
            newItem.GetComponent<PanelData>().LoadData(itemsLeft[i].id, itemsLeft[i].name, itemsLeft[i].duration, "left");
            panels.Add(newItem);
        }
    }
}
