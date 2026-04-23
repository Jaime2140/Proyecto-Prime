using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowDragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [Header("Referencias")]
    public HyprlandValidator validator;

    private CanvasGroup canvasGroup;
    private Canvas mainCanvas;

    private GameObject ghostIcon;
    private RectTransform ghostRect;

    void Awake()
    {
        mainCanvas = GetComponentInParent<Canvas>();
        
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.5f;
        canvasGroup.blocksRaycasts = false;

        ghostIcon = new GameObject("Ghost_" + gameObject.name);
        ghostIcon.transform.SetParent(mainCanvas.transform, false);
        ghostIcon.transform.SetAsLastSibling();

        Image myImage = GetComponent<Image>();
        Image ghostImage = ghostIcon.AddComponent<Image>();
        ghostImage.sprite = myImage.sprite;
        ghostImage.type = myImage.type;
        ghostImage.color = new Color(1f, 1f, 1f, 0.7f);

        ghostImage.raycastTarget = false;

        ghostRect = ghostIcon.GetComponent<RectTransform>();
        ghostRect.sizeDelta = GetComponent<RectTransform>().rect.size;
        
        Vector2 localPointerPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPosition);
        ghostRect.localPosition = localPointerPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ghostRect != null)
        {
            Vector2 localPointerPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(mainCanvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPosition);
            ghostRect.localPosition = localPointerPosition;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            RectTransform windowA = eventData.pointerDrag.GetComponent<RectTransform>();
            RectTransform windowB = this.GetComponent<RectTransform>();

            if (windowA.name == "Window_Party" && windowB.parent != null && windowB.parent.name == "FilaInferior_C")
            {
                windowB = windowB.parent.GetComponent<RectTransform>();
            }
            else if (windowA.parent != null && windowA.parent.name == "FilaInferior_C" && windowB.name == "Window_Party")
            {
                windowA = windowA.parent.GetComponent<RectTransform>();
            }

            if (validator != null)
            {
                validator.ExecuteSwap(windowA, windowB);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (ghostIcon != null)
        {
            Destroy(ghostIcon);
        }
    }
}