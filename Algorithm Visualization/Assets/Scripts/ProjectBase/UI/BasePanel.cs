using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 面板基类
/// 找到自己面板下的所有控件
/// 提供显示或隐藏的行为
/// 方便我们在子类中处理逻辑，节约找控件的工作量
/// </summary>
public class BasePanel : MonoBehaviour
{
    // 通过里氏替换原则，来存储所有的控件
    private Dictionary<string, List<UIBehaviour>> controlDic = new Dictionary<string, List<UIBehaviour>>();

    protected virtual void Awake()
    {
        FindChidrenControls<Button>();
        FindChidrenControls<Image>();
        FindChidrenControls<Text>();
        FindChidrenControls<Toggle>();
        FindChidrenControls<Slider>();
        FindChidrenControls<ScrollRect>();
        FindChidrenControls<Scrollbar>();
    }

    /// <summary>
    /// 显示自己
    /// </summary>
    public virtual void ShowMe(){}

    /// <summary>
    /// 隐藏自己
    /// </summary>
    public virtual void HideMe(){}

    /// <summary>
    /// 按钮点击事件
    /// </summary>
    /// <param name="btnName"></param>
    protected virtual void OnClick(string btnName){}    

    /// <summary>
    /// 单选框或多选框事件
    /// </summary>
    /// <param name="toggleName"></param>
    /// <param name="value"></param>
    protected virtual void OnValueChanged(string toggleName, bool value){}

    /// <summary>
    /// 滑动条事件
    /// </summary>
    /// <param name="toggleName"></param>
    /// <param name="value"></param>
    protected virtual void OnValueChanged(string toggleName, float value){}

    protected T GetControl<T>(string controlName) where T: UIBehaviour{
        if(controlDic.ContainsKey(controlName)){
            for(int i = 0; i< controlDic[controlName].Count; i++){
                // 同一个Object身上每种控件只能拥有一个，故保证了返回值的唯一性
                if(controlDic[controlName][i] is T)
                    return controlDic[controlName][i] as T;
            }
        }
        return null;
    }

    /// <summary>
    /// 找到子类型对应的控件，并添加事件的监听
    /// </summary>
    /// <typeparam name="T"></typeparam>
    private void FindChidrenControls<T>() where T: UIBehaviour{
        T[] controls = this.GetComponentsInChildren<T>();
        for(int i = 0; i < controls.Length; i++){
            string objName = controls[i].gameObject.name;
            if(controlDic.ContainsKey(objName))
                controlDic[objName].Add(controls[i]);
            else
                controlDic.Add(objName, new List<UIBehaviour>(){ controls[i] });
        
            // 如果是按钮控件
            if(controls[i] is Button){
                (controls[i] as Button).onClick.AddListener(()=>{
                    OnClick(objName);
                });
            }
            // 如果是单选框或者多选框
            else if(controls[i] is Toggle){
                (controls[i] as Toggle).onValueChanged.AddListener((value)=>{
                    OnValueChanged(objName, value);
                });
            }
            // 如果是滑动条
            else if(controls[i] is Slider){
                (controls[i] as Slider).onValueChanged.AddListener((value)=>{
                    OnValueChanged(objName, value);
                });
            }
            // 如果是文本框
            else if(controls[i] is Text){
                
            }
            else if(controls[i] is Scrollbar){
                (controls[i] as Scrollbar).onValueChanged.AddListener((value)=>{
                    OnValueChanged(objName, value);
                });
            }
        }
    }
    
}
