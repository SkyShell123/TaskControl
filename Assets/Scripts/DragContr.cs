using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragContr : MonoBehaviour
{
    public int dragIdOrder;

    private GameObject dragObject;

    public bool isDragging = false;

    public static DragContr instance = null;

    public ScrollRect scrollView1;
    public GameObject dragPreview;

    public RectTransform rectTransformParent;
    public CanvasGroup canvasGroup;

    private GameObject _dragPreview;
    private RectTransform dragPreviewRect;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void StartDragging(PanelData _panelData)
    {
        isDragging=true;
        canvasGroup.blocksRaycasts = false;
        _dragPreview = Instantiate(dragPreview, transform.parent);
        _dragPreview.transform.SetParent(rectTransformParent);
        dragPreviewRect = _dragPreview.GetComponent<RectTransform>();

        DragPreview dragPreviewIn = _dragPreview.GetComponent<DragPreview>();

        dragPreviewIn.id = _panelData.id;
        dragPreviewIn.name = _panelData.panelName.text;
        dragPreviewIn.duration = _panelData.panelDuration.text;
    }

    public void StopDragging()
    {
        isDragging = false;
        canvasGroup.blocksRaycasts = true;

        AddElement();

        Destroy(_dragPreview);
    }

    private void AddElement()
    {
        if (CheckIntersectionWithScrollView())
        {
            DragPreview dragPreviewOut = _dragPreview.GetComponent<DragPreview>();
            FormData newItem = new()
            {
                id = dragPreviewOut.id,
                id_order = dragIdOrder,
                name = dragPreviewOut.name,
                duration = dragPreviewOut.duration,
            };

            LeftDynamicContentScript.Instance.AddTask(newItem);

        }
    }

    public void UpdateDragPreviewPosition(PointerEventData eventData)
    {

        //Преобразование координат мыши в локальные координаты RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransformParent, eventData.position, eventData.pressEventCamera, out Vector2 localPointerPosition);

        //Обновление позиции объекта
        _dragPreview.transform.localPosition = localPointerPosition;

        CheckPlaneView();
    }

    private bool CheckIntersectionWithScrollView()
    {
        if (_dragPreview != null)
        {
            // Получаем прямоугольник Scroll View
            RectTransform scrollRectTransform = scrollView1.content;

            // Получаем прямоугольники элемента Scroll View и целевого элемента
            Rect scrollRectRect = scrollRectTransform.rect;
            Rect targetRect = dragPreviewRect.rect;

            // Получаем позиции элемента Scroll View и целевого элемента
            Vector2 scrollRectPosition = scrollRectTransform.anchoredPosition;
            Vector2 targetPosition = dragPreviewRect.anchoredPosition;

            // Проверяем, виден ли целевой элемент внутри Scroll View

            bool isVisible =
                scrollRectPosition.x + scrollRectRect.width / 100 > targetPosition.x &&
                scrollRectPosition.x - scrollRectRect.width / 1.3 < targetPosition.x + targetRect.width &&
                scrollRectPosition.y + scrollRectRect.height / 2 > targetPosition.y &&
                scrollRectPosition.y - scrollRectRect.height / 2 < targetPosition.y + targetRect.height;

            // Делаем что-то, если элемент виден
            if (isVisible)
            {
                return true;
            }

        }
        return false;
    }

    public void CheckPlaneView()
    {
        // Получаем позицию мыши в мировых координатах
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Определяем луч из позиции мыши в направлении (0, 0, -1)
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        // Проверяем, попал ли луч в какой-то объект
        if (hit.collider != null)
        {
            
            Vector2 localPosition;
            RectTransform rectTransform = hit.collider.GetComponent<RectTransform>();
            DragAndDrop dragAndDrop = hit.collider.GetComponent<DragAndDrop>();
            PanelData panelData = hit.collider.GetComponent<PanelData>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, null, out localPosition);
            //GameObject upArrow= dragAndDrop.downArrow.gameObject;
            //GameObject downArrow= dragAndDrop.upArrow.gameObject;


            bool isCursorOnTopHalf = localPosition.y > 0f;

            if (isCursorOnTopHalf)
            {
                dragIdOrder = panelData.id_order - 1;
            }
            else 
            {
                dragIdOrder = panelData.id_order + 1;
            }

            SwitchArrows(dragAndDrop.upArrow.gameObject, dragAndDrop.downArrow.gameObject, isCursorOnTopHalf);

            //Invoke(nameof(DelayDel), 1f);

            //if (dragObject== hit.collider.gameObject)
            //{

            //}
            //else 
            //{

            //}
        }
    }

    private void SwitchArrows(GameObject arrow1, GameObject arrow2, bool state)
    {
        arrow1.SetActive(state);
        arrow2.SetActive(!state);
    }
}
