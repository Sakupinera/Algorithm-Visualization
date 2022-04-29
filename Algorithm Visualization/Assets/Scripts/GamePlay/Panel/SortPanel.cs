using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SortPanel : BasePanel
{
    [SerializeField]
    Sort sort;

    public string SortName { get; set; }

    public Sort Sort
    {
        get { return sort; }
        set { sort = value; }
    }
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnClick(string btnName)
    {
        switch (btnName)
        {
            case "ResetBtn":
                sort.restart();
                break;
            case "StartBtn":
                sort.StartSorting(SortName);
                break;
        }
    }

    protected override void OnValueChanged(string toggleName, float value)
    {
        switch (toggleName)
        {
            case "NumSlider":
                sort.Num_Changed((int)value);
                break;
            case "SpeedSlider":
                sort.Speed_Changed(value);
                break;
        }
    }

    public Text GetTextbyName(string name)
    {
        return GetControl<Text>(name);
    }
}
