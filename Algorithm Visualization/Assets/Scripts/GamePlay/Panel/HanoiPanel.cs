using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HanoiPanel : BasePanel
{
    [SerializeField]
    Hanoi hanoi;

    public Hanoi Hanoi
    {
        get { return hanoi; }
        set { hanoi = value; }
    }
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void OnClick(string btnName)
    {
        switch(btnName)
        {
            case "重置":
                Hanoi.resets();
                break;
            case "下一步":
                Hanoi.next_step();
                break;
            case "自动":
                Hanoi.auto_run_switch();
                break;
        }
    }

    protected override void OnValueChanged(string toggleName, float value){
        Debug.Log(value);
        switch(toggleName){
            case "数目":
                Hanoi.number_change((int)value);
                break;
            case "速度":
                Hanoi.speed_change(value);
                break;
        }
    }

    public Text GetTextbyName(string name){
        return GetControl<Text>(name);
    }
}
