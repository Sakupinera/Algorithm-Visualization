using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventInfo{}

public class EventInfo<T>: IEventInfo {
    public UnityAction<T> actions;

    public EventInfo(UnityAction<T> action){
        action += action;
    }
}

public class EventInfo: IEventInfo{
        public UnityAction actions;

    public EventInfo(UnityAction action){
        action += action;
    }
}

/// <summary>
/// 事件中心 单例模式对象
/// 1.Dictionary
/// 2.委托
/// 3.观察者设计模式
/// </summary>
public class EventCenter : BaseManager<EventCenter>
{
    //  key -- 事件的名字（比如怪物死亡、玩家死亡，通关等等）
    //  value -- 对应的是 监听这个事件对应的委托函数
    private Dictionary<string, IEventInfo> eventDic = new Dictionary<string, IEventInfo>();

    /// <summary>
    /// 添加事件监听(泛型)
    /// </summary>
    /// <param name="name">事件的名字</param>
    /// <param name="action">准备处理事件的委托函数</param>
    public void AddEventListener<T>(string name, UnityAction<T> action){
        //  有没有对应的事件监听
        //  有的情况
        if(eventDic.ContainsKey(name)){
            // eventDic[name] += action;
            (eventDic[name] as EventInfo<T>).actions += action;
        }
        //  没有的情况
        else{
            // eventDic.Add(name, action);
            eventDic.Add(name, new EventInfo<T>(action));
        }
    }

    /// <summary>
    /// 添加事件监听
    /// </summary>
    /// <param name="name">事件的名字</param>
    /// <param name="action">准备处理事件的委托函数</param>
    public void AddEventListener(string name, UnityAction action){
        //  有没有对应的事件监听
        //  有的情况
        if(eventDic.ContainsKey(name)){
            // eventDic[name] += action;
            (eventDic[name] as EventInfo).actions += action;
        }
        //  没有的情况
        else{
            // eventDic.Add(name, action);
            eventDic.Add(name, new EventInfo(action));
        }
    }

    /// <summary>
    /// 移除对应的事件监听(泛型)
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void RemoveEventListener<T>(string name, UnityAction<T> action){
        if(eventDic.ContainsKey(name)){
            (eventDic[name] as EventInfo<T>).actions -= action;
        }
    }

    /// <summary>
    /// 移除对应的事件监听
    /// </summary>
    /// <param name="name"></param>
    /// <param name="action"></param>
    public void RemoveEventListener(string name, UnityAction action){
        if(eventDic.ContainsKey(name)){
            (eventDic[name] as EventInfo).actions -= action;
        }
    }

    /// <summary>
    /// 事件触发(泛型)
    /// </summary>
    /// <param name="name"></param>
    public void EventTrigger<T>(string name, T info){
        //  有没有对应的事件监听
        //  有的情况
        if(eventDic.ContainsKey(name)){
            // eventDic[name]();
            (eventDic[name] as EventInfo<T>).actions.Invoke(info);
        }
    }

    /// <summary>
    /// 事件触发
    /// </summary>
    /// <param name="name"></param>
    public void EventTrigger(string name){
        //  有没有对应的事件监听
        //  有的情况
        if(eventDic.ContainsKey(name)){
            // eventDic[name]();
            (eventDic[name] as EventInfo).actions.Invoke();
        }
    }

    /// <summary>
    /// 清空事件中心，主要用在场景切换时
    /// </summary>
    public void Clear(){
        eventDic.Clear();
    }
}
