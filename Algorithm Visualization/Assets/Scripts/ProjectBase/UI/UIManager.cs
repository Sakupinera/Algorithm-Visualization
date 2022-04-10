using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// UI层级
/// </summary>
public enum E_UI_Layer{
    Bottom,
    Middle,
    Top,
    System
}

/// <summary>
/// UI管理器
/// 1.管理所有显示的面板
/// 2.提供给外部显示和隐藏等等接口
/// </summary>
public class UIManager : BaseManager<UIManager>
{
    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();

    private Transform bottom;
    private Transform middle;
    private Transform top;
    private Transform system;

    public RectTransform canvas;

    public UIManager(){
        // 找到Canvas并创建，让其过场景时不被移除
        GameObject obj = ResourceManager.GetInstance().Load<GameObject>("UI/Canvas");
        canvas = obj.transform as RectTransform;
        GameObject.DontDestroyOnLoad(obj);

        // 找到各层
        bottom = canvas.Find("Bottom");
        middle = canvas.Find("Middle");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        // 找到EventSystem并创建，让其过场景时不被移除
        obj = ResourceManager.GetInstance().Load<GameObject>("UI/EventSystems");
        GameObject.DontDestroyOnLoad(obj);
    }

    public Transform GetLayerFather(E_UI_Layer layer){
        switch(layer){
            case E_UI_Layer.Bottom:
                return this.bottom;
            case E_UI_Layer.Middle:
                return this.middle;
            case E_UI_Layer.Top:
                return this.top;
            case E_UI_Layer.System:
                return this.system;
        }
        return null;
    }

    /// <summary>
    /// 显示面板
    /// </summary>
    /// <param name="panelName">面板的脚本类型</param>
    /// <param name="layer">面板名</param>
    /// <param name="callback">显示在哪一层</param>
    /// <typeparam name="T">当面板预制体创建成功时，要做的事情</typeparam>
    public void ShowPanel<T>(string panelName, E_UI_Layer layer = E_UI_Layer.Middle, UnityAction<T> callback = null) where T: BasePanel{
        // 如果已经存在该面板
        if(panelDic.ContainsKey(panelName)){
            if(callback != null){
                callback(panelDic[panelName] as T);
            }

            return;
        }
        // 如果还不存在该面板
        ResourceManager.GetInstance().LoadAsync<GameObject>("UI/" + panelName, (obj) =>{
            Transform father = null;
            switch(layer){
                case E_UI_Layer.Bottom:
                    father = bottom;
                    break;
                case E_UI_Layer.Middle:
                    father = middle;
                    break;
                case E_UI_Layer.Top:
                    father = top;
                    break;
                case E_UI_Layer.System:
                    father = system;
                    break;
            }
            // 设置父对象
            obj.transform.SetParent(father.transform);

            //  设置相对位置和大小
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            // 得到预制体身上的面板脚本
            T panel = obj.GetComponent<T>();
            // 处理面板创建完成后的逻辑
            if(callback != null){
                callback(panel);
            }

            // 把面板存起来
            panelDic.Add(panelName, panel);

        });
    }

    /// <summary>
    /// 隐藏面板
    /// </summary>
    /// <param name="panelName"></param>
    public void HidePanel(string panelName){
        if(panelDic.ContainsKey(panelName)){
            //  由子类实现
            panelDic[panelName].HideMe();

            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }

    public void HideAllPanel(){
        foreach(var panel in panelDic){
            // 由子类实现
            panelDic[panel.Key].HideMe();
            GameObject.Destroy(panelDic[panel.Key].gameObject);
        }
        panelDic.Clear();
    }

    /// <summary>
    /// 得到某一个已经显示的面板，方便外部使用
    /// </summary>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetPanel<T>(string name) where T: BasePanel{
        if(panelDic.ContainsKey(name))
            return panelDic[name] as T;
        return null;
    }

    /// <summary>
    /// 添加自定义事件监听
    /// </summary>
    /// <param name="control"></param>
    /// <param name="type"></param>
    /// <param name="callback"></param>
    public static void AddCustomEventListener(UIBehaviour control, EventTriggerType type, UnityAction<BaseEventData> callback){
        EventTrigger trigger = control.GetComponent<EventTrigger>();
        if(trigger == null)
            trigger = control.gameObject.AddComponent<EventTrigger>();
        
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = type;
        entry.callback.AddListener(callback);

        trigger.triggers.Add(entry);
    }
}
