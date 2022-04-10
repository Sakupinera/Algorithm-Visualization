﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Internal;

/// <summary>
/// 1.可以提供给外部添加帧更新事件的方法
/// 2.可以提供给外部添加协程的方法
/// </summary>
public class MonoManager : BaseManager<MonoManager>
{
    private MonoController controller;

    public MonoManager(){
        //  保证了MonoController对象的唯一性
        GameObject obj = new GameObject("MonoController");
        controller = obj.AddComponent<MonoController>();
    }

    /// <summary>
    /// 给外部提供的添加帧更新的函数
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun){
        controller.AddUpdateListener(fun);
    }

    /// <summary>
    /// 给外部提供的移除帧更新的函数
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpdateListener(UnityAction fun){
        controller.RemoveUpdateListener(fun);
    }

    public Coroutine StartCoroutine(string methodName){
        return controller.StartCoroutine(methodName);
    }

    public Coroutine StartCoroutine(IEnumerator routine){
        return controller.StartCoroutine(routine);
    }

    public Coroutine StartCoroutine(string methodName, [DefaultValue("null")] object value){
        return controller.StartCoroutine(methodName, value);
    }

    public void StopAllCoroutines(){
        controller.StopAllCoroutines();
    }

    public void StopCoroutine(IEnumerator routine){
        controller.StopCoroutine(routine);
    }

    public void StopCoroutine(Coroutine routine){
        controller.StopCoroutine(routine);
    }

    public void StopCoroutine(string methodName){
        controller.StopCoroutine(methodName);
    }
}
