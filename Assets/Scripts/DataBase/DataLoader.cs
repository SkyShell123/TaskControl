//using UnityEngine;
//using System.IO;
//using UnityEngine.UI;
//using TMPro;

//public class DataLoader : MonoBehaviour
//{
//    public TextMeshProUGUI displayText;
//    public TextMeshProUGUI displayText2;

//    void Start()
//    {
//        LoadData();
//    }

//    public void LoadData()
//    {
//        string filePath = "playerData.json";

//        if (File.Exists(filePath))
//        {
//            string jsonData = File.ReadAllText(filePath);
//            FormData loadedData = JsonUtility.FromJson<FormData>(jsonData);

//            // Используйте загруженные данные
//            displayText.text = loadedData.name;
//            displayText2.text = loadedData.duration;
//        }
//        else
//        {
//            Debug.LogWarning("File not found: " + filePath);
//        }
//    }
//}
