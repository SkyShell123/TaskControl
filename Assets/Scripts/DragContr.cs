using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragContr : MonoBehaviour
{
    private GameObject activeUpArrow;
    private GameObject activeDownArrow;

    public int dragIdOrder;

    public string dragObjectType;
    public DragPreview dragObject;

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
        float floatValue;
        float.TryParse(_panelData.panelDuration.text, out floatValue);

        dragPreviewIn.id = _panelData.id;
        dragPreviewIn.id_order = _panelData.id_order;
        dragPreviewIn.name = _panelData.panelName.text;
        dragPreviewIn.duration = floatValue;
        dragPreviewIn.panel= _panelData.gameObject;

        dragObjectType = _panelData.type;
        dragObject = dragPreviewIn;
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
        if (CheckView())
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

    private void ChangeOrder()
    {
        int index = dragObject.panel.transform.GetSiblingIndex();
        
        

        LeftDynamicContentScript.Instance.UpdateOrder(index, dragIdOrder);

        dragObject.panel.transform.SetSiblingIndex(dragIdOrder);
    }

    public void UpdateDragPreviewPosition(PointerEventData eventData)
    {

        //Преобразование координат мыши в локальные координаты RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransformParent, eventData.position, eventData.pressEventCamera, out Vector2 localPointerPosition);

        //Обновление позиции объекта
        _dragPreview.transform.localPosition = localPointerPosition;

        CheckPlaneView();
    }

    private bool CheckView()
    {
        if (_dragPreview != null)
        {
            // Получаем позицию мыши в мировых координатах
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Определяем луч из позиции мыши в направлении (0, 0, -1)
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePosition, Vector2.zero);

            foreach (RaycastHit2D view in hits)
            {
                if (view.collider != null && view.collider.gameObject.name == "Viewport")
                {
                    foreach (RaycastHit2D hit in hits)
                    {
                        if (hit.collider != null && hit.collider.gameObject.name == "Panel(Clone)")
                        {
                            Vector2 localPosition;
                            RectTransform rectTransform = hit.collider.GetComponent<RectTransform>();
                            //DragPreview dragPreview = hit.collider.GetComponent<DragPreview>();
                            //PanelData panelData = hit.collider.GetComponent<PanelData>();
                            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, null, out localPosition);
                            if (dragObject != null && dragObjectType == "left")
                            {
                                ChangeOrder();
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else if (dragObject != null && dragObjectType == "")
                        {
                            return true;
                        }

                    }
                }

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
        if (hit.collider != null && hit.collider.gameObject.name== "Panel(Clone)")
        {
            
            Vector2 localPosition;
            RectTransform rectTransform = hit.collider.GetComponent<RectTransform>();
            PanelData panelData = hit.collider.GetComponent<PanelData>();
            DragAndDrop dragAndDrop = hit.collider.GetComponent<DragAndDrop>();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, mousePosition, null, out localPosition);


            bool isCursorOnTopHalf = localPosition.y > 0f;
            int index = panelData.transform.GetSiblingIndex();

            if (isCursorOnTopHalf)
            {
                if (index > 0)
                {
                    dragIdOrder = index - 1;
                }
                
            }
            else 
            {
                dragIdOrder = index + 1;
            }

            activeUpArrow = dragAndDrop.upArrow.gameObject;
            activeDownArrow = dragAndDrop.downArrow.gameObject;

            SwitchArrows(isCursorOnTopHalf);
        }
        else
        {
            activeUpArrow.SetActive(false);
            activeDownArrow.SetActive(false);
        }
    }

    private void SwitchArrows(bool state)
    {
        activeUpArrow.SetActive(state);
        activeDownArrow.SetActive(!state);
    }
}
