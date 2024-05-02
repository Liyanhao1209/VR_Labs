using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DemandDisplayUI : MonoBehaviour
{
    public ProductArea[] wokeAreas;
    public Sprite[] imageSprite;

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

    // 通过实际根节点的首个食材判断应该在展示UI的地方贴什么图
    // 例如，第一个节点的Name属性是“纸盒”，那么很显然贴一张纸盒冰淇淋的图片
    public void OnShow(List<DemandNode>[] products)
    {
        gameObject.SetActive(true);
        for (int i = 0; i < products.Length; i++)
        {
            Transform itemTrans = transform.GetChild(i);
            itemTrans.gameObject.SetActive(true);
            wokeAreas[i].gameObject.SetActive(true);
            wokeAreas[i].SetDemand(products[i]);
            int transIndex = 0;
            var image = itemTrans.GetChild(transIndex).GetComponentInChildren<Image>();
            if (products[i][0].Name == "橙汁")
                image.sprite = imageSprite[0];
            else if(products[i][0].Name == "雪碧")
                image.sprite = imageSprite[1];
            else if(products[i][0].Name == "可乐")
                image.sprite = imageSprite[2];
            else if(products[i][0].Name == "甜筒")
                image.sprite = imageSprite[3];
            else if(products[i][0].Name == "纸盒")
                image.sprite = imageSprite[4];
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
        if(allCompleted)
            GameControl.DeliveryOfGoods();
    }

}
