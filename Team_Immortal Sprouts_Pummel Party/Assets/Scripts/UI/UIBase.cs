using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public abstract class UIBase : MonoBehaviour
{
    private void Start()
    {
        Init();
    }

    public virtual void Init()
    {

    }


    /// <summary>
    /// enum 값과 오브젝트를 묶어주는 함수
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    protected void Bind<T>(Type type) where T : Object
    {
        string[] keyNames = Enum.GetNames(type);

        Object[] objs = new Object[keyNames.Length];
        Managers.UI.ObjectDict.Add(typeof(T), objs);

        T[] components = gameObject.GetComponentsInChildren<T>();

        Debug.Assert(components.Length == keyNames.Length);

        for (int i = 0; i < components.Length; ++i)
        {
            T component = components[i];
            if (component.name == keyNames[i])
            {
                objs[i] = component;
                Debug.Assert(objs[i] != null);
            }
        }
    }

    protected T Get<T>(Enum what) where T : Object
    {
        Object[] objectList = null;

        int index = Convert.ToInt32(what);

        bool isObjectPresent = Managers.UI.ObjectDict.TryGetValue(typeof(T), out objectList);

        if (!isObjectPresent)
        {
            return null;
        }

        T desiredObj = objectList[index] as T;

        return desiredObj;
    }


    /// <summary>
    /// 버튼과 함수를 연동하는 함수
    /// </summary>
    /// <param name="button"></param>
    /// <param name="function"></param>
    protected void BindButtonEvent(Button button, UnityAction function)
    {
        button.onClick.RemoveListener(function);
        button.onClick.AddListener(function);
    }
}
