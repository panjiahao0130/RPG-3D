using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemTooltip : MonoBehaviour
{

    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemDescription;

    private RectTransform _rectTransform;
    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        UpdatePosition();
        //强制立即重建布局 用于解决ToolTip无法快速适配
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 mousePos= Input.mousePosition;
        Vector3[] corners = new Vector3[4];
        //将四个点的坐标放到corner数组中
        _rectTransform.GetWorldCorners(corners);
        float width = corners[3].x - corners[0].x;
        float height = corners[1].y - corners[0].y;
        if (mousePos.y<height)
        {
            _rectTransform.position = mousePos + Vector3.up * height * 0.6f;
        }
        else if (Screen.width-mousePos.x>width)
        {
            _rectTransform.position = mousePos + Vector3.right * width * 0.6f;
        }
        else
        {
            _rectTransform.position = mousePos + Vector3.left * width * 0.6f;
        }
    }


    public void SetItemTooltips(ItemData_SO item)
    {
        itemName.text = item.itemName;
        itemDescription.text = item.description;
    }
}
