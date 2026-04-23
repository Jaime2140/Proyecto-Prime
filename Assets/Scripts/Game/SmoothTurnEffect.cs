using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothTurnEffect : MonoBehaviour
{
    public RenderTexture gameScreenTexture;
    public RawImage gameScreenDisplay;
    public RawImage snapshotOld;
    public RawImage snapshotNew;

    public float turnSpeed = 0.20f;

    private bool isTurning = false;
    public bool IsTurning => isTurning;

    public IEnumerator AnimateTurn(int direction, Action onTurnMidpoint)
    {
        if (isTurning) yield break;
        isTurning = true;

        snapshotOld.rectTransform.SetAsLastSibling();
        snapshotNew.rectTransform.SetAsLastSibling();

        gameScreenDisplay.gameObject.SetActive(false);

        RenderTexture oldRT = CaptureRT();

        snapshotOld.texture = oldRT;
        snapshotOld.color = Color.white;
        snapshotOld.rectTransform.anchoredPosition = Vector2.zero;
        snapshotOld.gameObject.SetActive(true);

        yield return null;

        onTurnMidpoint?.Invoke();

        yield return null;

        RenderTexture newRT = CaptureRT();

        float moveDistance = gameScreenDisplay.rectTransform.rect.width;

        snapshotNew.texture = newRT;
        snapshotNew.color = Color.white;
        snapshotNew.rectTransform.anchoredPosition =
            Vector2.right * direction * moveDistance;

        snapshotNew.gameObject.SetActive(true);

        float elapsed = 0f;
        float extra = 8f;

        while (elapsed < turnSpeed)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / turnSpeed);
            t = Mathf.Pow(t, 0.8f);

            float offset = Mathf.Lerp(0, moveDistance + extra, t);

            snapshotOld.rectTransform.anchoredPosition =
                Vector2.left * direction * offset;

            snapshotNew.rectTransform.anchoredPosition =
                Vector2.right * direction * (moveDistance - offset);

            float fade = 1f - t * 0.2f;
            snapshotOld.color = new Color(fade, fade, fade, 1f);

            yield return null;
        }

        snapshotOld.rectTransform.anchoredPosition = Vector2.zero;
        snapshotNew.rectTransform.anchoredPosition = Vector2.zero;

        snapshotOld.gameObject.SetActive(false);
        snapshotNew.gameObject.SetActive(false);

        gameScreenDisplay.gameObject.SetActive(true);

        oldRT.Release();
        Destroy(oldRT);

        newRT.Release();
        Destroy(newRT);

        isTurning = false;
    }

    RenderTexture CaptureRT()
    {
        RenderTexture copy = new RenderTexture(
            gameScreenTexture.width,
            gameScreenTexture.height,
            0,
            RenderTextureFormat.ARGB32
        );

        copy.filterMode = FilterMode.Point;

        Graphics.Blit(gameScreenTexture, copy);

        return copy;
    }
}