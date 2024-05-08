using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelInforUI : MonoBehaviour
{
    Text levelTimeValue;
    Text targetValue;
    Text incomeValue;
    Text title;
    void Awake()
    {
        Debug.Log("start");
        // 通过组件属性反解析文本值
        levelTimeValue = transform.Find("Time/target").GetComponent<Text>();
        targetValue = transform.Find("Target/target").GetComponent<Text>();
        incomeValue = transform.Find("Income/target").GetComponent<Text>();
        title = transform.Find("Header Text").GetComponent<Text>();
    }

    // 重渲染（并不需要每帧修改，仅在变化时修改即可
    public void RefreshLevelTime(int time)
    {
        levelTimeValue.text = time.ToString();
    }
    public void RefreshLevelTaget(int targetIncome)
    {
        targetValue.text = targetIncome.ToString();
    }
    public void RefreshCurrentIncome(int currentIncome)
    {
        incomeValue.text = currentIncome.ToString();
    }
    public void RefreshLevelTitle(int level)
    {
        title.text = string.Format("第{0}关", level);
    }
}
