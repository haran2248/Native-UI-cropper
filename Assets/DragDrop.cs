using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler,IBeginDragHandler,IEndDragHandler,IDragHandler
{
    [SerializeField] private Canvas canvas;
    public RawImage img;
    private RectTransform rectTransform;
    public RectTransform panel;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("start:" + rectTransform.anchoredPosition);
        Debug.Log("On Begin Drag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        float wid = panel.rect.width;
        float hei = panel.rect.height;
        Debug.Log("On Drag");
        Debug.Log("event delta: "+eventData.delta/canvas.scaleFactor);
        Debug.Log("anchored Position: " + (img.rectTransform.anchoredPosition.y+eventData.delta.y));
        Debug.Log("height:" + img.rectTransform.rect.height);
        if (img.rectTransform.rect.width == wid)
        {
            if ((img.rectTransform.anchoredPosition.y + eventData.delta.y <= 0) && ((img.rectTransform.anchoredPosition.y + eventData.delta.y) * -1 + hei <= img.rectTransform.rect.height))
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x,rectTransform.anchoredPosition.y+eventData.delta.y);
        }
        if (img.rectTransform.rect.height == hei)
        {
            if ((img.rectTransform.anchoredPosition.x + eventData.delta.x<=0) && ((img.rectTransform.anchoredPosition.x + eventData.delta.x)*-1+wid<=img.rectTransform.rect.width))
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x + eventData.delta.x, rectTransform.anchoredPosition.y);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("start:" + rectTransform.anchoredPosition);
        
    }
}
