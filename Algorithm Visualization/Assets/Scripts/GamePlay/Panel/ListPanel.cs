using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ListPanel : BasePanel
{
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnClick(string btnName){
        switch(btnName){
            case "HanoiBtn":
                UIManager.GetInstance().HidePanel(this.name);
                ScenesManager.GetInstance().LoadSceneAsync("Hanoi", ()=>{
                    ResourceManager.GetInstance().LoadAsync<GameObject>("Level/HanoiCore", (obj)=>{
                        Hanoi hanoi = obj.GetComponent<Hanoi>();
                        UIManager.GetInstance().ShowPanel<HanoiPanel>("HanoiPanel",E_UI_Layer.Middle, (panel)=>{
                            panel.name = "HanoiPanel";
                            panel.Hanoi = hanoi;
                            panel.GetComponent<RectTransform>().anchorMin = new Vector2(0.84f, 0f);

                            hanoi.show_num = panel.GetTextbyName("显示层数");
                            hanoi.show_speed = panel.GetTextbyName("显示速度");
                        });
                    });
                });
                break;
            case "AStarBtn":
                UIManager.GetInstance().HidePanel(this.name);
                ScenesManager.GetInstance().LoadSceneAsync("AStar", ()=>{
                    ResourceManager.GetInstance().LoadAsync<GameObject>("Level/AStarCore", (obj)=>{
                        AStarTilemap t_AStar = obj.GetComponentInChildren<AStarTilemap>();
                        UIManager.GetInstance().ShowPanel<AStarPanel>("AStarPanel",E_UI_Layer.Middle, (panel)=>{
                            panel.name = "AStarPanel";
                            panel.AStar = t_AStar;
                            panel.GetComponent<RectTransform>().anchorMin = new Vector2(0.85f, 0f);
                        });
                        UIManager.GetInstance().ShowPanel<PromptPanel>("PromptPanel",E_UI_Layer.Top, (panel)=>{
                            panel.name = "PromptPanel";
                            t_AStar.PromptPanel = panel;
                            t_AStar.baseTile = ResourceManager.GetInstance().Load<Tile>("Tilemap/rounded");
                            t_AStar.Init();
                        });
                    });
                });
                break;
            case "DijkstraBtn":
                UIManager.GetInstance().HidePanel(this.name);
                ScenesManager.GetInstance().LoadSceneAsync("Dijkstra", ()=>{
                    ResourceManager.GetInstance().LoadAsync<GameObject>("Level/DijkstraCore", (obj)=>{
                        DijkstraTilemap t_Dijkstra = obj.GetComponentInChildren<DijkstraTilemap>();
                        UIManager.GetInstance().ShowPanel<DijkstraPanel>("DijkstraPanel",E_UI_Layer.Middle, (panel)=>{
                            panel.name = "DijkstraPanel";
                            panel.Dijkstra = t_Dijkstra;
                            panel.GetComponent<RectTransform>().anchorMin = new Vector2(0.85f, 0f);
                        });
                        UIManager.GetInstance().ShowPanel<PromptPanel>("PromptPanel",E_UI_Layer.Top, (panel)=>{
                            panel.name = "PromptPanel";
                            t_Dijkstra.PromptPanel = panel;
                            t_Dijkstra.baseTile = ResourceManager.GetInstance().Load<Tile>("Tilemap/rounded");
                            t_Dijkstra.Init();
                        });
                    });
                });
                break;
        }
    }  
}
