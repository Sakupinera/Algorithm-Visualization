using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : BasePanel{

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnClick(string btnName){
        Debug.Log(this.name);
        switch(btnName){
            case "StartBtn":
                UIManager.GetInstance().HidePanel(this.name);
                UIManager.GetInstance().ShowPanel<ListPanel>("ListPanel", E_UI_Layer.Middle, (panel)=>{
                    panel.name = "ListPanel";
                    panel.GetComponent<RectTransform>().anchorMin = new Vector2(0.1f, 0.12f);
                    panel.GetComponent<RectTransform>().anchorMax = new Vector2(0.9f, 0.88f);
                });
                UIManager.GetInstance().ShowPanel<FunctionalPanel>("FunctionalPanel", E_UI_Layer.Bottom, (panel)=>{
                    panel.name = "FunctionalPanel";
                });
                break;
            case "SettingBtn":
                break;
            case "ExitBtn":
                // 在编辑器模式退出
                #if UNITY_EDITOR    
                    UnityEditor.EditorApplication.isPlaying = false;
                // 发布后退出
                #else   
                    Application.Quit();
                #endif
                break;
        }
    }   
}