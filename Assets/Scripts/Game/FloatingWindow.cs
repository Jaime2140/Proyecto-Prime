using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingWindow : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        KeepInScreen();
    }

    private void KeepInScreen()
    {
        if (canvas == null) return;

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        Vector2 pos = rectTransform.localPosition;

        float minX = -canvasRect.rect.width / 2f + (rectTransform.rect.width * rectTransform.pivot.x);
        float maxX = canvasRect.rect.width / 2f - (rectTransform.rect.width * (1f - rectTransform.pivot.x));
        
        float minY = -canvasRect.rect.height / 2f + (rectTransform.rect.height * rectTransform.pivot.y);
        float maxY = canvasRect.rect.height / 2f - (rectTransform.rect.height * (1f - rectTransform.pivot.y));

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        rectTransform.localPosition = pos;
    }
}