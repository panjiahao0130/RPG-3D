using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MouseManager : Singleton<MouseManager>
{
    public Texture2D point, doorway, attack, target, arrow;
    
    //public UnityEvent <Vector3> OnMouseClicked;
    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClicked; 
    private RaycastHit hitInfo;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        
        if (InteractWithUI())
        {
            return;
        }
        SetCursorTexture();
        MouseControl();
    }

    /// <summary>
    /// 设置光标图片
    /// </summary>
    void SetCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray,out hitInfo))
        {
            //设置鼠标贴图
            switch (hitInfo.transform.tag)
            {
                //地面 显示为圆形的鼠标
                case "Ground":
                    Cursor.SetCursor(target,new Vector2(16,16),CursorMode.Auto);
                    break;
                //敌人 显示为一把剑的鼠标
                case "Enemy":
                    Cursor.SetCursor(attack,new Vector2(16,16),CursorMode.Auto);
                    break;
                //可攻击的 显示为一把剑 比如说石头人的石头
                case "Attackable":
                    Cursor.SetCursor(attack,new Vector2(16,16),CursorMode.Auto);
                    break;
                //传送门 显示为门的鼠标
                case "Portal":
                    Cursor.SetCursor(doorway,new Vector2(16,16),CursorMode.Auto);
                    break;
                //物品 显示为一个手的鼠标
                case "Item":
                    Cursor.SetCursor(point,new Vector2(16,16),CursorMode.Auto);
                    break;
                //默认显示为正常鼠标的样子
                default:
                    Cursor.SetCursor(arrow,new Vector2(16,16),CursorMode.Auto);
                    break;
            }
        }
    }

    /// <summary>
    /// 鼠标控制
    /// </summary>
    void MouseControl()
    {
        if (Input.GetMouseButtonDown(0)&&hitInfo.collider!=null)
        {
            if (hitInfo.collider.gameObject.CompareTag("Ground"))//hitInfo.transform.CompareTag("Ground")
            {
                OnMouseClicked?.Invoke(hitInfo.point); ;
            }
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag("Attackable"))
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag("Portal"))
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.CompareTag("Item"))
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            }
        }
    }

    bool InteractWithUI()
    {
        if (EventSystem.current!=null&&EventSystem.current.IsPointerOverGameObject())
        {
            Cursor.SetCursor(arrow,new Vector2(16,16),CursorMode.Auto);
            return true;
        }
        else
        {
            return false;
        }
    }
}
