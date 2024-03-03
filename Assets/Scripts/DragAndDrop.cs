using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Image upArrow;
    public Image downArrow;

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
