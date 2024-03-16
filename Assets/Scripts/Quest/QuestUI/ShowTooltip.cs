using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowTooltip : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    private ItemUI currentItemUI;

    private bool isMouseOver = false;

    private void Awake()
    {
        currentItemUI = GetComponent<ItemUI>();
    }

    private IEnumerator DelayShow()
    {
        yield return new WaitForSeconds(0.5f);

        if (isMouseOver)
        {
            QusetUIManager.Instance.tooltip.SetItemTooltips(currentItemUI.currentData);
            QusetUIManager.Instance.tooltip.gameObject.SetActive(true); 
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        StartCoroutine(DelayShow());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
        if (QusetUIManager.Instance.tooltip.gameObject.activeInHierarchy )
        {
            QusetUIManager.Instance.tooltip.gameObject.SetActive(false);
        }
    }
}
