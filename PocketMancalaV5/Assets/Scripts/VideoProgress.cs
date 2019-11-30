using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoProgress : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField]
    private VideoPlayer videoPlayer;
    private Image fill;

    private void Awake()
    {
        fill = GetComponent<Image>();
    }

    private void Update()
    {
        if (videoPlayer.frameCount > 0)
        {
            fill.fillAmount = (float) videoPlayer.frame / (float) videoPlayer.frameCount;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        TrySkip(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        TrySkip(eventData);
    }

    private void TrySkip(PointerEventData eventData)
    {
        Vector2 localPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(fill.rectTransform, eventData.position, null, out localPoint))
        {
            float pct = Mathf.InverseLerp(fill.rectTransform.rect.xMin, fill.rectTransform.rect.xMax, localPoint.x);
            SkipToPercent(pct);
        }
    }

    private void SkipToPercent(float x)
    {
        var frame = videoPlayer.frameCount * x;
        videoPlayer.frame = (long)frame;
    }
}
