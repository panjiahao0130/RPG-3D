using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionButton : MonoBehaviour
{
    public KeyCode actionKey;
    private SlotHolder currentSlotHolder;

    private void Awake()
    {
        currentSlotHolder = GetComponent<SlotHolder>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(actionKey))
        {
            if (currentSlotHolder._itemUI.GetItemData()!=null)
            {
                currentSlotHolder.UseItem();
            }
        }
    }
}
