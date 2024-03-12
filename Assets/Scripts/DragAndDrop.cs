using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Image upArrow;
    public Image downArrow;

    private BoxCollider2D boxCollider; // Используем BoxCollider2D, предполагая, что коллайдер является прямоугольным

    private void Start()
    {
        // Получаем компонент BoxCollider2D
        boxCollider = GetComponent<BoxCollider2D>();

        // Проверяем, есть ли коллайдер
        if (boxCollider == null)
        {
            Debug.LogError("Коллайдер не найден на объекте: " + gameObject.name);
            return;
        }

        // Вызываем метод для первоначальной установки размеров коллайдера
        UpdateColliderSize();
    }

    private void Update()
    {
        // Периодически вызываем метод для обновления размеров коллайдера
        UpdateColliderSize();
    }

    private void UpdateColliderSize()
    {
        // Получаем текущие размеры RectTransform элемента интерфейса
        Vector2 size = GetComponent<RectTransform>().sizeDelta;

        // Обновляем размеры коллайдера
        boxCollider.size = size;
    }

    private bool IsDragStarted() 
    {
        return DragContr.instance.isDragging; 
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!IsDragStarted())
        {
            StartDragging();
            DragContr.instance.UpdateDragPreviewPosition(eventData);
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsDragStarted())
        {
            StopDragging();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (IsDragStarted())
        {
            DragContr.instance.UpdateDragPreviewPosition(eventData); 
        }
    }

    public void StartDragging()
    {
        PanelData panelData = GetComponent<PanelData>();
        DragContr.instance.StartDragging(panelData);
    }

    public void StopDragging()
    {
        DragContr.instance.StopDragging();

    }
}
