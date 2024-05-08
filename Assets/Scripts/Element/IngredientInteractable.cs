using System.Collections;
using System.Collections.Generic;
//using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;

public class IngredientInteractable : XRGrabInteractable
{
    public MakeBeverages beverageTool;
    public string name;
    public GameObject tipUI;
    private bool grabState;
    private bool Grabed;
    private ProductArea workArea;
    void Start()
    {
        //print( evens.interactorObject.transform.parent.GetComponent< ActionBasedController>().activateAction.action.IsPressed()); 
        firstHoverEntered.AddListener((evens) => { if (!tipUI) return;  if (Grabed) return; tipUI.SetActive(true); });
        lastHoverExited.AddListener((evens) => { if (!tipUI) return; if (Grabed) return; tipUI.SetActive(false); });
        activated.AddListener(OnProductMakeStepSure);
    }
    private void OnProductMakeStepSure(ActivateEventArgs args)
    {
        if (workArea)
            workArea.ProductAddElement(this);
        
    }

    protected override void Grab()
    {
        grabState = true;
        if (!Grabed)
        {
            if(tipUI)
                tipUI.SetActive(false);
            GameObject obj = Instantiate(gameObject);
            obj.transform.position = transform.position;
            if(tipUI == null)
            {
                obj.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                IngredientInteractable able = obj.GetComponent<IngredientInteractable>();
                able.name = "";
                beverageTool.beveragesCup = able;
            }
            Grabed = true;
        }
        base.Grab();
    }

    protected override void Drop()
    {
        base.Drop();
        grabState = false;
        Rigidbody rigbody = GetComponent<Rigidbody>();
        rigbody.useGravity = true;
        rigbody.isKinematic = false;
    }


    public void SetWorkArea(ProductArea area)
    {
        workArea = area;
    }
    public void LeaveWorkArea()
    {
        workArea = null;
    }
}
