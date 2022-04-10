using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AStarPanel : BasePanel
{
    [SerializeField]
    AStarTilemap astar;

    public AStarTilemap AStar
    {
        get { return astar; }
        set { astar = value; }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "RefreshBtn":
                AStar.RefreshMap();
                break;
            case "AStarBtn":
                AStar.A_Star_Visualization();
                break;
        }
    }

    protected override void OnValueChanged(string toggleName, float value)
    {
        switch (toggleName)
        {
            case "SpeedSlider":
                AStar.ChangePrintSpeed(value);
                break;
        }
    }

}
