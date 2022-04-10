using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    protected override void Awake(){
        if(Instance == null)
            DontDestroyOnLoad(gameObject);
        base.Awake();
    }
    protected override void Init(){}
    protected override void DisInit(){}

    // Start is called before the first frame update
    void Start()
    {
        ScenesManager.GetInstance().LoadSceneAsync("MainMenu", ()=>{
            UIManager.GetInstance().ShowPanel<MainMenuPanel>("MainMenuPanel", E_UI_Layer.Middle, (panel)=>{
                panel.name = "MainMenuPanel";
            });
        });
    }

}
