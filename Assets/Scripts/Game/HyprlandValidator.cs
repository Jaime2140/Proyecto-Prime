using UnityEngine;
using UnityEngine.UI;

public class HyprlandValidator : MonoBehaviour
{
    [Header("Configuración")]
    public float tolerance = 2.0f;
    public float alignmentTolerance = 50f;

    public bool CanSwapWindows(RectTransform windowA, RectTransform windowB)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(windowA);
        LayoutRebuilder.ForceRebuildLayoutImmediate(windowB);

        bool isInFilaA = windowA.parent != null && windowA.parent.name == "FilaInferior_C";
        bool isInFilaB = windowB.parent != null && windowB.parent.name == "FilaInferior_C";
        
        if (isInFilaA && isInFilaB) return true;

        if (isInFilaA && !isInFilaB) windowA = windowA.parent.GetComponent<RectTransform>();
        if (isInFilaB && !isInFilaA) windowB = windowB.parent.GetComponent<RectTransform>();

        float widthA = windowA.rect.width;
        float heightA = windowA.rect.height;
        float widthB = windowB.rect.width;
        float heightB = windowB.rect.height;

        bool sameWidth = Mathf.Abs(widthA - widthB) <= tolerance;
        bool sameHeight = Mathf.Abs(heightA - heightB) <= tolerance;

        bool shareRow = Mathf.Abs(windowA.position.y - windowB.position.y) < alignmentTolerance || Mathf.Abs(windowA.anchorMin.y - windowB.anchorMin.y) < 0.1f;
        bool shareColumn = Mathf.Abs(windowA.position.x - windowB.position.x) < alignmentTolerance || Mathf.Abs(windowA.anchorMin.x - windowB.anchorMin.x) < 0.1f;

        if (!shareRow && !shareColumn)
        {
            Debug.LogWarning("❌ Swap Denegado: Movimiento Diagonal detectado.");
            return false;
        }

        if (shareRow && sameHeight) return true;
        
        if (shareColumn && sameWidth) return true;

        Debug.LogWarning($"❌ Swap Denegado: Medidas incompatibles. Las ventanas no caben en la cuadrícula.");
        return false;
    }

    public void ExecuteSwap(RectTransform windowA, RectTransform windowB)
    {
        if (!CanSwapWindows(windowA, windowB)) return;

        bool isInFilaA = windowA.parent != null && windowA.parent.name == "FilaInferior_C";
        bool isInFilaB = windowB.parent != null && windowB.parent.name == "FilaInferior_C";

        if (isInFilaA && isInFilaB)
        {
            int indexA = windowA.GetSiblingIndex();
            int indexB = windowB.GetSiblingIndex();
            windowA.SetSiblingIndex(indexB);
            windowB.SetSiblingIndex(indexA);
            LayoutRebuilder.ForceRebuildLayoutImmediate(windowA.parent.GetComponent<RectTransform>());
            return;
        }

        if (isInFilaA && !isInFilaB) windowA = windowA.parent.GetComponent<RectTransform>();
        if (isInFilaB && !isInFilaA) windowB = windowB.parent.GetComponent<RectTransform>();

        Transform parentA = windowA.parent;
        int siblingIndexA = windowA.GetSiblingIndex();
        Transform parentB = windowB.parent;
        int siblingIndexB = windowB.GetSiblingIndex();
        
        Vector2 anchorMinA = windowA.anchorMin;
        Vector2 anchorMaxA = windowA.anchorMax;
        Vector2 pivotA = windowA.pivot;
        Vector2 posA = windowA.anchoredPosition;

        windowA.SetParent(parentB, false);
        windowB.SetParent(parentA, false);
        
        windowA.SetSiblingIndex(siblingIndexB);
        windowB.SetSiblingIndex(siblingIndexA);
        
        windowA.anchorMin = windowB.anchorMin;
        windowA.anchorMax = windowB.anchorMax;
        windowA.pivot = windowB.pivot;
        windowA.anchoredPosition = windowB.anchoredPosition;

        windowB.anchorMin = anchorMinA;
        windowB.anchorMax = anchorMaxA;
        windowB.pivot = pivotA;
        windowB.anchoredPosition = posA;

        LayoutElement elemA = windowA.GetComponent<LayoutElement>();
        if (elemA != null) windowA.sizeDelta = new Vector2(elemA.preferredWidth, elemA.preferredHeight);
        
        LayoutElement elemB = windowB.GetComponent<LayoutElement>();
        if (elemB != null) windowB.sizeDelta = new Vector2(elemB.preferredWidth, elemB.preferredHeight);

        Canvas.ForceUpdateCanvases();
        if (parentA != null && parentA.GetComponent<LayoutGroup>() != null) LayoutRebuilder.ForceRebuildLayoutImmediate(parentA.GetComponent<RectTransform>());
        if (parentB != null && parentB.GetComponent<LayoutGroup>() != null && parentA != parentB) LayoutRebuilder.ForceRebuildLayoutImmediate(parentB.GetComponent<RectTransform>());
            
        Debug.Log($"✅ Swap Exitoso entre {windowA.name} y {windowB.name}!");
    }
}