using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Image upArrow;
    public Image downArrow;

    private BoxCollider2D boxCollider; // ���������� BoxCollider2D, �����������, ��� ��������� �������� �������������

    private void Start()
    {
        // �������� ��������� BoxCollider2D
        boxCollider = GetComponent<BoxCollider2D>();

        // ���������, ���� �� ���������
        if (boxCollider == null)
        {
            Debug.LogError("��������� �� ������ �� �������: " + gameObject.name);
            return;
        }

        // �������� ����� ��� �������������� ��������� �������� ����������
        UpdateColliderSize();
    }

    private void Update()
    {
        // ������������ �������� ����� ��� ���������� �������� ����������
        UpdateColliderSize();
    }

    private void UpdateColliderSize()
    {
        // �������� ������� ������� RectTransform �������� ����������
        Vector2 size = GetComponent<RectTransform>().sizeDelta;

        // ��������� ������� ����������
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
