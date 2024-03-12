[System.Serializable]
public class FormData
{
    public int id;

    public int id_order;

    public string name;

    public float duration;

    public float startTime; // Время начала выполнения задачи в пределах дня (0 - начало дня, 1 - конец дня)
    public float endTime;
}
