using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragonDrop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Canvas canvas;

    private bool isDragging = false;

    public RectTransform parentRectTransform;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();
        //parentRectTransform = GetComponentInParent<RectTransform>();

    }

    void Update()
    {
        if (isDragging)
        {
            // �������� ����������� � ������ Scroll View
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        canvasGroup.blocksRaycasts = true;
        SetParent();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // �������������� ��������� ���� � ��������� ���������� RectTransform
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, eventData.position, eventData.pressEventCamera, out Vector2 localPointerPosition);

        // ���������� ������� �������
        transform.localPosition = localPointerPosition;
    }

    private void SetParent()
    {
        CheckIntersectionWithScrollView();
    }

    private void CheckIntersectionWithScrollView()
    {
        // �������� ��� Scroll View �� �����
        ScrollRect[] scrollViews = GameObject.FindObjectsOfType<ScrollRect>();

        foreach (ScrollRect scrollView in scrollViews)
        {
            // �������� RectTransform Scroll View
            RectTransform scrollViewRect = scrollView.GetComponent<RectTransform>();

            // ��������� �����������
            if (RectIntersects(rectTransform, scrollViewRect))
            {
                // ��� ��� ��������� ����������� � ������ Scroll View
                Debug.Log("����������� � " + scrollViewRect.gameObject.name);
                transform.SetParent(scrollView.content.transform, false);
            }
        }
    }


    private bool RectIntersects(RectTransform rect1, RectTransform rect2)
    {
        // �������� ����������� ���� ���������������
        return rect1.rect.Overlaps(rect2.rect);
    }
}
