using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogUI : MonoBehaviour
{
    Text dialogText;
    Text countDownText;
    Scrollbar countDownValue;

    Button deliveryBtn;
    float patienceTime;
    int patience;
    private void Start()
    {
        dialogText = transform.GetChild(0).gameObject.GetComponent<Text>();
        countDownText = transform.GetChild(1).gameObject.GetComponent<Text>();
        countDownValue = transform.GetComponentInChildren<Scrollbar>();
        deliveryBtn = GetComponentInChildren<Button>();
        //deliveryBtn.onClick.AddListener(() => {  });
    }
    public void OnShow(int countDown)
    {
        gameObject.SetActive(true);
        patience = countDown;
        patienceTime = countDown;
    }

    // 重置文本
    public void RefreshStr(string content)
    {
        dialogText.text = content;
    }

    // 刷新剩余耐心值
    public void Update()
    {
        if (patienceTime > 0)
        {
            patienceTime -= Time.deltaTime;
            RefreshCountDown((int)patienceTime, patienceTime/patience);
        }
        else
        {
            patience = 0;
            gameObject.SetActive(false);
            GameControl.CustomerVisitTimeOut(); // 超时
        }
    }
    
    private void RefreshCountDown(int time,float value)
    {
        countDownText.text = String.Format("剩余耐心值{0}",time);
        countDownValue.size = value;
    }
}
