using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class MakeBeverages :XRSimpleInteractable
{
    public string beverageName;
    public IngredientInteractable beveragesCup;
    float intervalTime = 1;
    float countDown = 0;
    bool isRunning;
    GameObject waterPartical;
    private void Start()
    {
        waterPartical = transform.GetChild(1).gameObject;
        selectEntered.AddListener(SelectEntered);
    }
    private void Update()
    {
        if (!isRunning) return;
        countDown += Time.deltaTime;
        if(countDown > intervalTime)
        {
            isRunning = false;
            countDown = 0;
            beveragesCup.enabled = true;
            beveragesCup.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
            waterPartical.SetActive(false);
        }
    }
    private void SelectEntered(SelectEnterEventArgs args)
    {
        if (isRunning) return;
        isRunning = true;
        beveragesCup.name = beverageName;
        beveragesCup.enabled = false;
        waterPartical.SetActive(true);
    }

}
