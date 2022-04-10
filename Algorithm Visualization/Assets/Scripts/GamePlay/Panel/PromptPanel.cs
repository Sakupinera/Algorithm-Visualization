using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PromptPanel : BasePanel
{
    protected override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// 显示自己
    /// </summary>
    public override void ShowMe(){
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 隐藏自己
    /// </summary>
    public override void HideMe(){
        gameObject.SetActive(false);
    }

    public Text GetPromptText(){
        return GetControl<Text>("PromptText");
    }
}
