using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause_menu : MonoBehaviour
{
    GameObject inst_Obj;

    public GameObject MainMenuUI;
    public GameObject AllTasksUI;
    public GameObject TodayTasksUI;

    public GameObject NewBlank;
    public GameObject ObjCanvas;

    public static Pause_menu instance = null;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void NewTask()
    {
        inst_Obj = Instantiate(NewBlank, transform.position, transform.rotation);
        inst_Obj.transform.SetParent(ObjCanvas.transform, false);
    }

    public void Quit()
    {
        
    }

    public void TodayTasks()
    {
        MainMenuUI.SetActive(false);
        TodayTasksUI.SetActive(true);
        UpdateLists();
    }

    public void AllTasks()
    {
        MainMenuUI.SetActive(false);
        AllTasksUI.SetActive(true);
        UpdateLists();
    }

    public void Back()
    {
        AllTasksUI.SetActive(false);
        TodayTasksUI.SetActive(false);
        MainMenuUI.SetActive(true);
    }

    private void UpdateLists()
    {
        DynamicContentScript.Instance.PopulateScrollView();
        LeftDynamicContentScript.Instance.PopulateScrollView();
    }
}
