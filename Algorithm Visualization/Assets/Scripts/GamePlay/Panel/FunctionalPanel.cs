using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FunctionalPanel : BasePanel
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnClick(string btnName){
        switch(btnName){
            case "BackBtn":
                UIManager.GetInstance().HideAllPanel();
                if(SceneManager.GetActiveScene().name != "MainMenu"){
                    ScenesManager.GetInstance().LoadSceneAsync("MainMenu", ()=>{
                        UIManager.GetInstance().ShowPanel<MainMenuPanel>("MainMenuPanel", E_UI_Layer.Middle, (panel)=>{
                            panel.name = "MainMenuPanel";
                        });
                    });
                }else{
                    UIManager.GetInstance().ShowPanel<MainMenuPanel>("MainMenuPanel", E_UI_Layer.Middle, (panel)=>{
                        panel.name = "MainMenuPanel";
                    });
                }
                break;
        }
    }  
}
