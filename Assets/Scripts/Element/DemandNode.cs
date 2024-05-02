using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DemandNode 
{
    string nodeName;
    List<DemandNode> children;
    private bool moreChild; // �Ƿ����ж������(��ֹ������С���������û�������������)
    private bool isProduct; // �����Ƿ��Ѿ���һ���걸����Ʒ�ˣ���ֹ���һ������Ҫһ����Ƥ��Ͳ�����治Ҫ����ܡ�����������
    public int ProductTime { get; private set; } // �����Ʒ��Ӧ�ö���ʱ������
    public int Level { get; private set; }
    public string Name { get { return nodeName; } }

    internal DemandNode(string name,int level ,bool addMoreChild = false,bool product = true)
    {
        nodeName = name;
        moreChild = addMoreChild;
        isProduct = product;
        children = new List<DemandNode>();
        Level = level;
        ProductTime = 15;
    }

    public void AddChild(DemandNode newChild)
    {
        children.Add(newChild);
    }

    public DemandNode RandomChild()
    {
        int childIndex = Random.Range(0, children.Count);
        return children[childIndex];
    }

    // ���ѵݹ������Ʒ�����
    public void CreateChildDemand(List<DemandNode> prodect)
    {
        prodect.Add(this);
        if (children.Count == 0) return; // û���ӽڵ�
        bool AddChild = true;
        if (isProduct)
            AddChild = Random.Range(1, 101) < 50; // ������˿��Գ�Ϊһ���Ϸ�����Ʒ����ô��һ������ֹͣ������������������ֵ�Ƚϣ�
        
        // ������ܳ�Ϊ�Ϸ���Ʒ����ôһ����������
        if(AddChild)
        {
            int addCount = 1; 
            if(moreChild)
                addCount = Random.Range(1, 4); // ���������Ӷ���ӽڵ㣬��ô�������ӵ����������
            for (int i = 0; i < addCount; i++)
                RandomChild().CreateChildDemand(prodect); // �����ѡaddCount���������ӽ��б��ݹ��������
            
        }
    }
    public bool IsChildNode(DemandNode node)
    {
        return children.Contains(node);
    }

}
