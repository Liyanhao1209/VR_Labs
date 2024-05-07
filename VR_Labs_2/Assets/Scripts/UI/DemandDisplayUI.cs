using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
public class DemandDisplayUI : MonoBehaviour
{
    public ProductArea[] wokeAreas;
    public Sprite[] imageSprite;
    [SerializeField]
    public GameObject[] demand;   //需求成果示例
    List<GameObject> obList = new List<GameObject>();//储存物体的列表
    private void Awake()
    {
        OnHide();
    }

    // 重置需求文本
    private void ResetAllText()
    {
        Text[] texts = transform.GetComponentsInChildren<Text>();
        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].text = "";
        }
    }
    public void refresh()
    {
        for (int i = 0; i < obList.Count; i++)
        {

            obList[i].SetActive(false);
        }
        obList.Clear();
    }

    // 通过实际根节点的首个食材判断应该在展示UI的地方贴什么图
    // 例如，第一个节点的Name属性是“纸盒”，那么很显然贴一张纸盒冰淇淋的图片
    public void OnShow(List<DemandNode>[] products)
    {
        gameObject.SetActive(true);
        refresh();
        for (int i = 0; i < products.Length; i++)
        {
            Transform itemTrans = transform.GetChild(i);
            itemTrans.gameObject.SetActive(true);
            wokeAreas[i].gameObject.SetActive(true);
            demand[i].SetActive(true);
            wokeAreas[i].SetDemand(products[i]);
            int transIndex = 0;
            var image = itemTrans.GetChild(transIndex).GetComponentInChildren<Image>();
            if (products[i][0].Name == "橙汁")
            {
                image.sprite = imageSprite[0];
                demand[i].transform.GetChild(0).gameObject.SetActive(true);
                obList.Add(demand[i].transform.GetChild(0).gameObject);
                if (products[i].Count > 1)
                    Drinkexample(i, products[i]);
            }
            else if (products[i][0].Name == "雪碧")
            {
                image.sprite = imageSprite[1];
                demand[i].transform.GetChild(1).gameObject.SetActive(true);
                obList.Add(demand[i].transform.GetChild(1).gameObject);
                if (products[i].Count > 1)
                    Drinkexample(i, products[i]);
            }
            else if (products[i][0].Name == "可乐")
            {
                image.sprite = imageSprite[2];
                demand[i].transform.GetChild(2).gameObject.SetActive(true);
                obList.Add(demand[i].transform.GetChild(2).gameObject);
                if (products[i].Count > 1)
                    Drinkexample(i, products[i]);
            }
            else if (products[i][0].Name == "甜筒")
            {
                image.sprite = imageSprite[3];
                if (products[i].Count > 1)
                    Eatexample(4, i, products[i]);
                demand[i].transform.GetChild(4).gameObject.SetActive(true);
                obList.Add(demand[i].transform.GetChild(4).gameObject);
            }
            else if (products[i][0].Name == "纸盒")
            {
                if (products[i].Count > 1)
                    Eatexample(5, i, products[i]);
                image.sprite = imageSprite[4];
                demand[i].transform.GetChild(5).gameObject.SetActive(true);
                obList.Add(demand[i].transform.GetChild(5).gameObject);
            }
            for (int j = 0; j < products[i].Count; j++)
            {
                if (products[i][j].Level < 3)
                {
                    transIndex += 1;
                    itemTrans.GetChild(transIndex).GetChild(0).GetComponent<Text>().text = products[i][j].Name;
                }
                else
                    itemTrans.GetChild(transIndex).GetChild(0).GetComponent<Text>().text += "\n+" + products[i][j].Name;
            }
        }
    }

    public void OnHide()
    {
        ResetAllText();
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
            wokeAreas[i].gameObject.SetActive(false);
            wokeAreas[i].ProductModelRecycle();
            demand[i].SetActive(false);
        }
        gameObject.SetActive(false);
    }

    // 所有商品是否都符合顾客需求
    public void DeliveryOfGoodsCheck()
    {
        if (!gameObject.activeInHierarchy) return;
        bool allCompleted = true;
        for (int i = 0; i < wokeAreas.Length; i++)
        {
            if (!wokeAreas[i].ProductCompleted && transform.GetChild(i).gameObject.activeInHierarchy)
            {
                allCompleted = false;
                break;
            }
        }
        if (allCompleted)
            GameControl.DeliveryOfGoods();
    }



    public void Drinkexample(int i, List<DemandNode> nodeList)
    {
        if (nodeList[1].Name == "草莓")
            demand[i].transform.GetChild(3).gameObject.SetActive(true);
        else
            return;
    }
    public void Eatexample(int index, int i, List<DemandNode> nodeList)
    {
        Transform former = null;
        int height = 0;
        for (int j = 1; j < nodeList.Count; j++)
        {
            if (nodeList[j].Name == "黄冰淇凌")
            {
                demand[i].transform.GetChild(index).transform.GetChild(height).gameObject.SetActive(true);
                former = demand[i].transform.GetChild(index).transform.GetChild(height).transform;
                obList.Add(demand[i].transform.GetChild(index).transform.GetChild(height).gameObject);
                height++;
            }
            else if (nodeList[j].Name == "粉冰淇凌")
            {
                demand[i].transform.GetChild(index).transform.GetChild(height + 3).gameObject.SetActive(true);
                obList.Add(demand[i].transform.GetChild(index).transform.GetChild(height + 3).gameObject);
                former = demand[i].transform.GetChild(index).transform.GetChild(height+3).transform;
                height++;
            }
            else if (nodeList[j].Name == "绿冰淇凌")
            {
                demand[i].transform.GetChild(index).transform.GetChild(height + 6).gameObject.SetActive(true);
                obList.Add(demand[i].transform.GetChild(index).transform.GetChild(height + 6).gameObject);
                former = demand[i].transform.GetChild(index).transform.GetChild(height+6).transform;
                height++;
            }
            else if (nodeList[j].Name == "糖果")
            {
                Debug.Log(former.gameObject.name);
                if (former != null)
                {
                    former.GetChild(0).gameObject.SetActive(true);
                    obList.Add(former.GetChild(0).gameObject);
                }
            }
            else if (nodeList[j].Name == "巧克力")
            {
                Debug.Log(former.gameObject.name);
                if (former != null)
                {
                    former.GetChild(1).gameObject.SetActive(true);
                    obList.Add(former.GetChild(1).gameObject);
                }
            }
            else if (nodeList[j].Name == "樱桃")
            {
                Debug.Log(former.gameObject.name);
                if (former != null)
                {
                    former.GetChild(2).gameObject.SetActive(true);
                    obList.Add(former.GetChild(2).gameObject);
                }
            }

        }
    }
}