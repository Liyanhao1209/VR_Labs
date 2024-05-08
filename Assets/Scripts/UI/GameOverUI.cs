using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    Text tipText;
    void Awake()
    {
        // 两个按钮，一个重开，一个退出
        Button[] buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(() => { GameControl.RestartGame(); });
        //buttons[0].onClick.AddListener(() => { Application.Quit(); });
        tipText = GetComponentInChildren<Text>();
    }
    public void SetTipStr(string content)
    {
        tipText.text = content;
    }
}
