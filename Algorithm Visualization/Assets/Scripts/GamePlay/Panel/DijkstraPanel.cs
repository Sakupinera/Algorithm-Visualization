using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraPanel : BasePanel
{
    [SerializeField]
    DijkstraTilemap dijkstra;

    public DijkstraTilemap Dijkstra
    {
        get { return dijkstra; }
        set { dijkstra = value; }
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
                Dijkstra.RefreshMap();
                break;
            case "DijkstraBtn":
                Dijkstra.Dijstra_Visualization();
                break;
        }
    }

    protected override void OnValueChanged(string toggleName, float value)
    {
        switch (toggleName)
        {
            case "SpeedSlider":
                Dijkstra.ChangePrintSpeed(value);
                break;
        }
    }
}
