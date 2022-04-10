using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 资源加载模块
/// 1.异步加载
/// 2.委托和lambda表达式
/// 3.协程
/// 4.泛型
/// </summary>
public class ResourceManager : BaseManager<ResourceManager>
{
    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T Load<T>(string name) where T: Object{
        T res = Resources.Load<T>(name);
        //  如果对象是一个GameObject类型的，把他实例化之后再返回出去，外部直接使用即可
        if(res is GameObject)
            return GameObject.Instantiate(res);
        //  TextAsset/AudioClip
        else
            return res;
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    /// <typeparam name="T"></typeparam>
    public void LoadAsync<T>(string name, UnityAction<T> callback) where T: Object{
        //  开启异步加载的协程
        MonoManager.GetInstance().StartCoroutine(ReallyLoadAsync(name, callback));
    }

    /// <summary>
    /// 真正的异步加载
    /// </summary>
    /// <param name="name"></param>
    /// <param name="callback"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T: Object{
        ResourceRequest r = Resources.LoadAsync<T>(name);
        yield return r;

        if(r.asset is GameObject){
            callback(GameObject.Instantiate(r.asset) as T);
        }else{
            callback(r.asset as T);
        }
    }
}
