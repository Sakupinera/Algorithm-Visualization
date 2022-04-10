using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换模块
/// 知识点：
/// 1.场景异步加载
/// 2.协程
/// 3.委托
/// </summary>
public class ScenesManager : BaseManager<ScenesManager>
{
    public void LoadScene(string name, UnityAction fun){
        //  场景同步加载
        SceneManager.LoadScene(name);
        //  加载完成之后才会执行fun
        fun();
    }

    /// <summary>
    /// 提供给外部的异步加载的方法接口
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    public void LoadSceneAsync(string name, UnityAction fun){
        MonoManager.GetInstance().StartCoroutine(ReallyLoadSceneAsync(name, fun));
    }

    /// <summary>
    /// 协程异步加载场景
    /// </summary>
    /// <param name="name"></param>
    /// <param name="fun"></param>
    /// <returns></returns>
    private IEnumerator ReallyLoadSceneAsync(string name, UnityAction fun){
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        //  可以得到场景加载的一个进度
        while(!ao.isDone){
            //  事件中心向外分发进度情况，外面想用就用
            EventCenter.GetInstance().EventTrigger("Loading", ao.progress);
            //  这里面去更新进度条
            yield return ao.progress;
        }
        //  加载完成之后才会执行fun
        fun();
    }
}
