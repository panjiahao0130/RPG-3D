using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour,IDragHandler,IPointerDownHandler
{
   private RectTransform _rectTransform;
   private Canvas _canvas;

   private void Awake()
   {
      _rectTransform = GetComponent<RectTransform>();
      _canvas = InventoryManager.Instance.GetComponent<Canvas>();
   }

   public void OnDrag(PointerEventData eventData)
   {
      _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor;
   }

   public void OnPointerDown(PointerEventData eventData)
   {
      _rectTransform.SetSiblingIndex(2);
   }
}
