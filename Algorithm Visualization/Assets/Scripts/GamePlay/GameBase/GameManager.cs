using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    protected override void Awake(){
        if(Instance == null)
            DontDestroyOnLoad(gameObject);
        base.Awake();
    }
    // protected override void Init(){}
    // protected override void DisInit(){}

    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<AspectRatioController>();

        ScenesManager.GetInstance().LoadScene("MainMenu", ()=>{
            UIManager.GetInstance().ShowPanel<MainMenuPanel>("MainMenuPanel", E_UI_Layer.Middle, (panel)=>{
                panel.name = "MainMenuPanel";
            });
        });
    }

}
