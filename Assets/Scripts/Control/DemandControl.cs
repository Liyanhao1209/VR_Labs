using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static Unity.VisualScripting.Metadata;

public static class DemandControl 
{
    static DemandNode root;
    // ����DAG������ͼ���ĵ���
    // �������ҵ��زİ�����Щ���û�У��о�һ��
    /**
        ����û�У����ɲ�ݮ����Ȼ��֭���ӱ��ϲ����ݮ����֣���ֻ�������ˣ�û�زģ�
        ��˪û�У������ǹ�
        �ɿ˰�û�У������ɿ���
    **/
    internal static void BuildDemandGraphic()
    {
        root = new DemandNode("root",0, true, false);

        DemandNode orange = new DemandNode("��֭",1,false,false);
        DemandNode sprite = new DemandNode("ѩ��",1);
        DemandNode coco = new DemandNode("����",1);
        DemandNode cone = new DemandNode("��Ͳ",1,true,false);
        DemandNode box = new DemandNode("ֽ��",1,true,false);
        root.AddChild(orange);
        root.AddChild(sprite);
        root.AddChild(coco);
        root.AddChild(cone);
        root.AddChild(box);

        DemandNode lemon = new DemandNode("��ݮ", 2);
        orange.AddChild(lemon);

        DemandNode green = new DemandNode("�̱����",2);
        DemandNode pink = new DemandNode("�۱����",2);
        DemandNode yellow = new DemandNode("�Ʊ����",2);
        cone.AddChild(green);
        cone.AddChild(pink);
        cone.AddChild(yellow);

        box.AddChild(green);
        box.AddChild(pink);
        box.AddChild(yellow);

        DemandNode candy = new DemandNode("�ǹ�",3);
        DemandNode chocolate = new DemandNode("�ɿ���",3);
        DemandNode cherry = new DemandNode("ӣ��",3);

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

    // Ϊһ���û���������
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
