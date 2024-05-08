using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static Unity.VisualScripting.Metadata;

public static class DemandControl 
{
    static DemandNode root;
    // 构建DAG（具体图见文档）
    // 这里我找的素材包里有些组件没有，列举一下
    /**
        柠檬没有，换成草莓（虽然橙汁杯子边上插个草莓很奇怪，但只能这样了，没素材）
        糖霜没有，换成糖果
        巧克棒没有，换成巧克力
    **/
    internal static void BuildDemandGraphic()
    {
        root = new DemandNode("root",0, true, false);

        DemandNode orange = new DemandNode("橙汁",1,false,false);
        DemandNode sprite = new DemandNode("雪碧",1);
        DemandNode coco = new DemandNode("可乐",1);
        DemandNode cone = new DemandNode("甜筒",1,true,false);
        DemandNode box = new DemandNode("纸盒",1,true,false);
        root.AddChild(orange);
        root.AddChild(sprite);
        root.AddChild(coco);
        root.AddChild(cone);
        root.AddChild(box);

        DemandNode lemon = new DemandNode("草莓", 2);
        orange.AddChild(lemon);

        DemandNode green = new DemandNode("绿冰淇凌",2);
        DemandNode pink = new DemandNode("粉冰淇凌",2);
        DemandNode yellow = new DemandNode("黄冰淇凌",2);
        cone.AddChild(green);
        cone.AddChild(pink);
        cone.AddChild(yellow);

        box.AddChild(green);
        box.AddChild(pink);
        box.AddChild(yellow);

        DemandNode candy = new DemandNode("糖果",3);
        DemandNode chocolate = new DemandNode("巧克力",3);
        DemandNode cherry = new DemandNode("樱桃",3);

        green.AddChild(candy);
        green.AddChild(chocolate);
        green.AddChild(cherry);

        pink.AddChild(candy);
        pink.AddChild(chocolate);
        pink.AddChild(cherry);

        yellow.AddChild(candy);
        yellow.AddChild(chocolate);
        yellow.AddChild(cherry);
    }

    // 为一个用户创建需求
    public static List<DemandNode>[] CreateDemandProduct()
    {
        int childProdectCount = Random.Range(2, 4);
        List<DemandNode>[] products = new List<DemandNode>[childProdectCount];
        for (int i = 0; i < childProdectCount; i++)
        {
            products[i] = new List<DemandNode>();
            root.RandomChild().CreateChildDemand(products[i]);
        }
        return products;
    }
}
