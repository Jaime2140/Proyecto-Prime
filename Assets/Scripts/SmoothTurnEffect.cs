using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothTurnEffect : MonoBehaviour
{
    [Header("Referencias UI")]
    public RenderTexture gameScreenTexture;
    public RawImage snapshotOverlay;

    [Header("Configuración")]
    public float turnSpeed = 0.30f;

    private bool isTurning = false;

    public IEnumerator AnimateTurn(int direction)
    {
        if (isTurning) yield break;
        isTurning = true;
        
        Texture2D snapshot = new Texture2D(gameScreenTexture.width, gameScreenTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = gameScreenTexture;
        snapshot.ReadPixels(new Rect(0, 0, gameScreenTexture.width, gameScreenTexture.height), 0, 0);
        snapshot.Apply();
        RenderTexture.active = null;

        snapshotOverlay.texture = snapshot;
        snapshotOverlay.rectTransform.anchoredPosition = Vector2.zero;
        snapshotOverlay.gameObject.SetActive(true);

        yield return null; 

        float elapsed = 0f;
        float moveDistance = 256f;

        while (elapsed < turnSpeed)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / turnSpeed;
            
            snapshotOverlay.rectTransform.anchoredPosition = new Vector2(-direction * moveDistance * t, 0);
            yield return null;
        }

        snapshotOverlay.gameObject.SetActive(false);
        Destroy(snapshot);
        isTurning = false;
    }
}