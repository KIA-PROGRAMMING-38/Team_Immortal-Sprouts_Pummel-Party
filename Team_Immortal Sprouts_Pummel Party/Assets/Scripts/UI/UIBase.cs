using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class UIBase : MonoBehaviour
{
    protected Dictionary<Type, UnityEngine.Object[]> Objects = new Dictionary<Type, UnityEngine.Object[]>();

    /// <summary>
    /// enum 값과 오브젝트를 묶어주는 함수
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] keyNames = Enum.GetNames(type);
        int lengthOfKeyNames = keyNames.Length;

        UnityEngine.Object[] objs = new UnityEngine.Object[lengthOfKeyNames];

        Objects.Add(typeof(T), objs);

        for (int i = 0; i < lengthOfKeyNames; ++i)
        {
            objs[i] = transform.Find(keyNames[i]);

            Debug.Assert(objs[i] != null);
        }
    }

    /// <summary>
    /// enum값에 해당하는 오브젝트들중 index에 해당하는 데이터를 반환하는 함수 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="index"></param>
    /// <returns></returns>
    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objectList = null;

        bool isObjectPresent = Objects.TryGetValue(typeof(T), out objectList);

        if (!isObjectPresent)
        {
            return null;
        }

        return objectList as T;
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
