using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMainUI : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        // 一开始的界面，开始或者退出
        Button[] buttons = GetComponentsInChildren<Button>();
        buttons[0].onClick.AddListener(() => { GameControl.StartGame(); });
        buttons[0].onClick.AddListener(() => { Application.Quit(); });
    }
}
