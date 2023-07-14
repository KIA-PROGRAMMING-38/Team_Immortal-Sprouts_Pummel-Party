using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;
using Object = UnityEngine.Object;

public class UIManager
{
    
    public void Init()
    {
        
    }

    public Stack<UIBase> UIStack = new Stack<UIBase>();

    public T PopUI<T>(string name = null, Transform parent = null) where T : UIBase
    {
        if (string.IsNullOrEmpty(name))
        {
            name = typeof(T).Name;  
        }

        GameObject prefab = Managers.Resource.Instantiate<GameObject>(name);

        T UI = prefab.GetComponent<T>();

        UIStack.Push(UI);

        if (parent != null)
        {
            prefab.transform.SetParent(parent);
        }

        prefab.transform.localScale = Vector3.one;
        prefab.transform.localPosition = prefab.transform.position;

        return UI;
    }


    public void CloseUI(UIBase UI = null)
    {
        if (UIStack.Count <= 0)
        {
            return;
        }

        if (UI != null)
        {
            Debug.Assert(UI == UIStack.Peek());
        }

        UIBase chosenUI = UIStack.Pop();
        Managers.Resource.Destroy(chosenUI.gameObject);
    }

    public void CloseAllUI()
    {
        while (0 < UIStack.Count)
        {
            CloseUI();
        }
    }
}
