using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UI_Animator : MonoBehaviour
{
    [Header("UI Feedback -Shake Effect")]
    [SerializeField] private float shakeMagnitude;
    [SerializeField] private float shakeDuration;

    public void Shake(Transform transformToShake, Vector3 originalPos)
    {
        RectTransform rectTransform = transformToShake.GetComponent<RectTransform>();
        StartCoroutine(ShakeCo(rectTransform, originalPos));
    }

    private IEnumerator ShakeCo(RectTransform rectTransform, Vector3 originalPos)
    {
        float time = 0;
        while (time < shakeDuration)
        {
            float xOffset = Random.Range(-shakeDuration, shakeMagnitude);
            float yOffset = Random.Range(-shakeDuration, shakeMagnitude);

            rectTransform.anchoredPosition = originalPos + new Vector3(xOffset, yOffset);

            time += Time.deltaTime;
            yield return null;
        }

        rectTransform.anchoredPosition = originalPos;
    }

    public void ChangePosition(Transform transform, Vector3 offset, float duration = .3f)
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        StartCoroutine(ChangePositionCo(rectTransform, offset, duration));
    }

    private IEnumerator ChangePositionCo( RectTransform rectTransform, Vector3 offset, float duration)
    {
        float time = 0;

        Vector3 initialPosition = rectTransform.anchoredPosition;
        Vector3 targetPosition = initialPosition + offset;

        while (time < duration)
        {
            float t = time / duration;
            t = Mathf.SmoothStep(0, 1, t);

            rectTransform.anchoredPosition = Vector3.Lerp(initialPosition, targetPosition, t);

            time += Time.deltaTime;

            yield return null;
        }

        rectTransform.anchoredPosition = targetPosition;
    }

    public void ChangeScale(Transform transform, float targetScale, float duration = .25f)
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        StartCoroutine(ChangeScaleCo(rectTransform, targetScale, duration));
    }

    public IEnumerator ChangeScaleCo(RectTransform rectTransform, float newScale, float duration = .25f)
    {
        float time = 0;
        Vector3 initialScale = rectTransform.localScale;
        Vector3 targetScale = new Vector3(newScale, newScale, newScale);

        while (time < duration)
        {
            rectTransform.localScale = Vector3.Lerp (initialScale, targetScale, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        rectTransform.localScale = targetScale;
    }

    public void ChangeColor(Image image, float targetAlpha, float duration)
    {
        StartCoroutine(ChangeColorCo(image, targetAlpha, duration));
    }

    private IEnumerator ChangeColorCo(Image image, float targetAlpha, float duration)
    {
        float time = 0;
        Color currentColor = image.color;
        float startAlpha = currentColor.a;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

            time += Time.deltaTime;
            yield return null;
        }

        image.color = new Color(currentColor.r, currentColor.g, currentColor.b, targetAlpha);
    }
}
