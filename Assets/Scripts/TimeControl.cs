using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    private readonly int bTime=8;
    private readonly int eTime=23;

    public int currentTime;

    public static TimeControl Instance;
    // Start is called before the first frame update

    public List<GameObject> tasks;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
        UpdateTime();
    }

    public void UpdateTime()
    {
        currentTime = bTime * 60;

        tasks = LeftDynamicContentScript.Instance.panels;

        foreach (GameObject task in tasks)
        {
            PanelData panelData = task.GetComponent<PanelData>();

            panelData.BTime.text = $"{currentTime/60}.{currentTime % 60}";

            currentTime += int.Parse(panelData.panelDuration.text);

            panelData.ETime.text = $"{currentTime / 60}.{currentTime % 60}";
        }
    }
}
