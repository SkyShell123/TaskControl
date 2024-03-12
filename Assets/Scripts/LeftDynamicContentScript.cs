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
        FormData formData = new();
        foreach (FormData item in itemsLeft)
        {
            if (item.id== _id)
            {
                formData= item;
                
            }
        }
        itemsLeft.Remove(formData);
        UpdateData();

    }

    public void UpdateOrder(int indexToMove, int newIndex)
    {
        if (true/*indexToMove >= 0 && indexToMove < itemsLeft.Count && newIndex >= 0 && newIndex < itemsLeft.Count*/)
        {
            FormData itemToMove = itemsLeft[indexToMove];
            itemsLeft.RemoveAt(indexToMove);

            if (newIndex > indexToMove)
            {
                newIndex--; // Коррекция индекса, если новая позиция находится после удаленного элемента
            }

            itemsLeft.Insert(newIndex, itemToMove);
            Debug.Log(indexToMove + "  " + newIndex);

            // Теперь элемент переместился на новую позицию
        }

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

        Transform contentTransform = GetComponentInChildren<VerticalLayoutGroup>().transform;

        GameObject newItem = Instantiate(itemPrefab, contentTransform);
        newItem.GetComponent<PanelData>().LoadData(_items.id, _items.id_order, _items.name, _items.duration, "left");
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
        Transform contentTransform = GetComponentInChildren<VerticalLayoutGroup>().transform;

        // Создаем нужное количество элементов
        for (int i = 0; i < itemsLeft.Count; i++)
        {
            // Создаем экземпляр элемента из префаба
            GameObject newItem = Instantiate(itemPrefab, contentTransform);
            newItem.GetComponent<PanelData>().LoadData(itemsLeft[i].id, itemsLeft[i].id_order, itemsLeft[i].name, itemsLeft[i].duration, "left");
            panels.Add(newItem);
        }
    }
}
