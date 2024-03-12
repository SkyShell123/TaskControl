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
        // —ортировка задач по времени начала
        tasks.Sort((a, b) => a.startTime.CompareTo(b.startTime));

        // »нициализаци€ времени начала и конца дл€ каждой задачи, исход€ из пор€дка
        float currentTime = 0f;

        foreach (FormData task in tasks)
        {
            task.startTime = currentTime;
            task.endTime = currentTime + task.duration / (24 * 60); // ѕреобразование минут в доли дн€
            currentTime = task.endTime;
        }

        // ѕример вывода времени начала и окончани€ задач в консоль
        foreach (FormData task in tasks)
        {
            Debug.Log($"Task: {task.name}, Start: {task.startTime}, End: {task.endTime}");
        }
    }
}
