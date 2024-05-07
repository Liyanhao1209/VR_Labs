using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DemandNode 
{
    string nodeName;
    List<DemandNode> children;
    private bool moreChild; // 是否能有多个孩子(防止类似于小料撒多了用户看不清撒几份)
    private bool isProduct; // 至此是否已经是一个完备的商品了（防止提出一个“我要一个脆皮甜筒，上面不要冰淇淋”这样的需求）
    public int ProductTime { get; private set; } // 这件商品理应用多少时间制作
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

    // 深搜递归添加商品子组件
    public void CreateChildDemand(List<DemandNode> prodect)
    {
        prodect.Add(this);
        if (children.Count == 0) return; // 没有子节点
        bool AddChild = true;
        if (isProduct)
            AddChild = Random.Range(1, 101) < 50; // 如果至此可以成为一个合法的商品，那么按一定概率停止（随机生成随机数与阈值比较）
        
        // 如果不能成为合法商品，那么一定添加子组件
        if(AddChild)
        {
            int addCount = 1; 
            if(moreChild)
                addCount = Random.Range(1, 4); // 如果可以添加多个子节点，那么随机出添加的子组件数量
            for (int i = 0; i < addCount; i++)
                RandomChild().CreateChildDemand(prodect); // 随机挑选addCount个子组件添加进列表，递归这个过程
            
        }
    }
    public bool IsChildNode(DemandNode node)
    {
        return children.Contains(node);
    }

}
