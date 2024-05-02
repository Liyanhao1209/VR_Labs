using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSucessUI : MonoBehaviour
{
    // Start is called before the first frame update
    Button nextBtn;
    void Start()
    {
        nextBtn = GetComponentInChildren<Button>();
        nextBtn.onClick.AddListener(() => { GameControl.NextLevel(); }); // 新增一个下一关的按钮，绑定一个回调，点击按钮进入下一关
    }
}
