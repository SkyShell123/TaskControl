using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    private Time bTime;
    private Time eTime;
    // Start is called before the first frame update
    
    public List<FormData> tasks;

    void Start()
    {
        tasks=LeftDynamicContentScript.Instance.itemsLeft;
        // ���������� ����� �� ������� ������
        tasks.Sort((a, b) => a.startTime.CompareTo(b.startTime));

        // ������������� ������� ������ � ����� ��� ������ ������, ������ �� �������
        float currentTime = 0f;

        foreach (FormData task in tasks)
        {
            task.startTime = currentTime;
            task.endTime = currentTime + task.duration / (24 * 60); // �������������� ����� � ���� ���
            currentTime = task.endTime;
        }

        // ������ ������ ������� ������ � ��������� ����� � �������
        foreach (FormData task in tasks)
        {
            Debug.Log($"Task: {task.name}, Start: {task.startTime}, End: {task.endTime}");
        }
    }
}
