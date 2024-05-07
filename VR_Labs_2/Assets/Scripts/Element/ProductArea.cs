using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class ProductNode
{
    public string Name { get; private set; }
    public int Level { get; private set; }
    public Transform modelTrans { get; set; }
    public int ExistChildrenCount { get {
            int count = 0;
            for (int i = 0; i < childNode.Count; i++)
                if (childNode[i].Exist)
                    count++;
            return count;
        } 
    }
    public bool Exist { get; private set; }
    bool competed;
    public bool Completed { 
        get {
            if(competed) return competed;
            else
            {
                if (!Exist) return false;
                bool temp = true;
                for (int i = 0; i < childNode.Count; i++)
                {
                    if (!childNode[i].Completed)
                        return false;
                }
                competed = temp ;
                return competed;
            }
        }
        private set { competed = value; } }
    public ProductNode Parent { get; private set; }
    List<ProductNode> childNode;
    public List<ProductNode> children { get { return childNode; } }
    internal ProductNode(string productName,int level)
    {
        Name = productName;
        Exist = false;
        childNode = new List<ProductNode>();
        competed = false;
        Level = level;
    }
    public void AddChildProduct(ProductNode child)
    {
        childNode.Add(child);
        child.SetParent(this);
    }
    private void SetParent(ProductNode p)
    {
        Parent = p;
    }
    internal void ProductAdd()
    {
        Exist = true;
    }
    internal ProductNode StepCheck(string name,Action<ProductNode> addStep)
    {
        if (!Exist)
        {
            if (name == Name)
            {
                addStep(this);
                Exist = true;
                if (!Completed)
                    return this;
                else
                    return Parent;
            }
            else
                return this;
        }
        else
        {
            for (int i = 0; i < childNode.Count; i++)
            {
                if (!childNode[i].Completed)
                {
                    if (childNode[i].Name == name)
                    {
                        ProductNode temp = childNode[i].StepCheck(name, addStep);
                        if (temp == this)
                            while (temp != null && temp.Completed)
                                temp = temp.Parent;
                        return temp;
                    }
                }
            }
            return this;
        }
    }
    public void ForAll(List<ProductNode> next)
    {
        next.Add(this);
        for (int i = 0; i < childNode.Count; i++)
        {
            childNode[i].ForAll(next);
        }
    }
}

public class ProductArea : MonoBehaviour
{

    public Mesh[] ICEMeshs;
    ProductNode currentStep;
    ProductNode root;
    public bool ProductCompleted { get { return root!=null ? root.Completed : false; }}
    public void SetDemand(List<DemandNode> demands)
    {
        //������������ ��Ʒ�ڵ�
        List<ProductNode> allProduct = new List<ProductNode>();
        for (int i = 0; i < demands.Count; i++)
        {
            allProduct.Add(new ProductNode(demands[i].Name, demands[i].Level));
        }
        //��������ȼ� ���ɲ�Ʒ�� ����У�����Ʋ�Ʒ�����ɹ���
        List<ProductNode> parentPath = new List<ProductNode>();
        for (int i = 0; i < demands.Count; i++)
        {
            if (demands[i].Level > parentPath.Count)
                NewSubNode(parentPath,allProduct[i]);
            else if (demands[i].Level == parentPath.Count)
                LevelEqualNode(parentPath, allProduct[i]);
            else
            {   //��ǰ�ڵ�ȼ�����ǰһ���ڵ㣬�Ƴ����ڵ�·���͵ȼ��Ľڵ�   ��������    �½ڵ�ȼ��͵��������
                while (parentPath.Count >= demands[i].Level)
                    parentPath.RemoveAt(parentPath.Count - 1);
                NewSubNode(parentPath, allProduct[i]);
            }
        }
        //��һ����Ʒ�ڵ�Ϊ��Ʒ�ĸ��ڵ�
        root = allProduct[0];
        currentStep = root;
    }

    //���������� ��ǰ�ڵ��ǰһ���ڵ�ĵȼ��� �򽫽ڵ���������ڵ�·���� ͬʱΪǰһ���ڵ�����ӽڵ�
    private void NewSubNode(List<ProductNode> parentPath,ProductNode newNode)
    {
        parentPath.Add(newNode);
        if (parentPath.Count > 1)
            parentPath[parentPath.Count - 2].AddChildProduct(newNode);
    }
    //���������� ��ǰ�ڵ���ǰһ���ڵ�ȼ���ͬ  ��Ϊ���ڵ�·���е�ǰһ���ڵ�����ӽڵ� 
    private void LevelEqualNode(List<ProductNode> parentPath, ProductNode newNode)
    {
        int parentIndex = parentPath.Count - 2;
        if (parentIndex > -1)
            parentPath[parentIndex].AddChildProduct(newNode);
        parentPath[parentIndex + 1] = newNode;
    }
    private void OnTriggerEnter(Collider other)
    {
        IngredientInteractable interactive = other.GetComponent<IngredientInteractable>();
        if (interactive)
            interactive.SetWorkArea(this);
    }
    private void OnTriggerExit(Collider other)
    {
        IngredientInteractable interactive = other.GetComponent<IngredientInteractable>();
        if (interactive)
            interactive.LeaveWorkArea();
    }
    public void ProductAddElement(IngredientInteractable element)
    {
        if (currentStep == null) return;
        currentStep = currentStep.StepCheck(element.name, (addNode) => {
            //pres += "> >AddRgith " + addNode.Name;
            if (addNode.Level == 1)
                addNode.modelTrans = transform.Find(addNode.Name);
            else if (addNode.Level == 2)
            {
                addNode.modelTrans = addNode.Parent.modelTrans.GetChild(addNode.Parent.ExistChildrenCount);
                var meshFile = addNode.modelTrans.GetComponent<MeshFilter>();
                if (addNode.Name == "�Ʊ����")
                    meshFile.mesh = ICEMeshs[0];
                else if (addNode.Name == "�۱����")
                    meshFile.mesh = ICEMeshs[1];
                else if(addNode.Name == "�̱����")
                    meshFile.mesh = ICEMeshs[2];
                else
                    addNode.modelTrans = addNode.Parent.modelTrans.Find(addNode.Name);
            }
            else
                addNode.modelTrans = addNode.Parent.modelTrans.Find(addNode.Name);

            addNode.modelTrans.gameObject.SetActive(true);
            Destroy(element.gameObject);
        });
        //if (currentStep == null)
        //    Debug.LogError("compeleted");
        //else
        //    Debug.LogWarning(currentStep.Name);
    }
    public void ProductModelRecycle()
    {
        if (root == null) return;
        List<ProductNode> list = new List<ProductNode>();
        root.ForAll(list);
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].modelTrans!= null)
                list[i].modelTrans.gameObject.SetActive(false);
        }
        root = null;
        currentStep = null;
    }
}
