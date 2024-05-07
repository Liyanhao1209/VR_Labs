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
    public GameObject[] demand;   //����ɹ�ʾ��
    List<GameObject> obList = new List<GameObject>();//����������б�
    private void Awake()
    {
        OnHide();
    }

    // ���������ı�
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

    // ͨ��ʵ�ʸ��ڵ���׸�ʳ���ж�Ӧ����չʾUI�ĵط���ʲôͼ
    // ���磬��һ���ڵ��Name�����ǡ�ֽ�С�����ô����Ȼ��һ��ֽ�б���ܵ�ͼƬ
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
            if (products[i][0].Name == "��֭")
            {
                image.sprite = imageSprite[0];
                demand[i].transform.GetChild(0).gameObject.SetActive(true);
                obList.Add(demand[i].transform.GetChild(0).gameObject);
                if (products[i].Count > 1)
                    Drinkexample(i, products[i]);
            }
            else if (products[i][0].Name == "ѩ��")
            {
                image.sprite = imageSprite[1];
                demand[i].transform.GetChild(1).gameObject.SetActive(true);
                obList.Add(demand[i].transform.GetChild(1).gameObject);
                if (products[i].Count > 1)
                    Drinkexample(i, products[i]);
            }
            else if (products[i][0].Name == "����")
            {
                image.sprite = imageSprite[2];
                demand[i].transform.GetChild(2).gameObject.SetActive(true);
                obList.Add(demand[i].transform.GetChild(2).gameObject);
                if (products[i].Count > 1)
                    Drinkexample(i, products[i]);
            }
            else if (products[i][0].Name == "��Ͳ")
            {
                image.sprite = imageSprite[3];
                if (products[i].Count > 1)
                    Eatexample(4, i, products[i]);
                demand[i].transform.GetChild(4).gameObject.SetActive(true);
                obList.Add(demand[i].transform.GetChild(4).gameObject);
            }
            else if (products[i][0].Name == "ֽ��")
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

    // ������Ʒ�Ƿ񶼷��Ϲ˿�����
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
        if (nodeList[1].Name == "��ݮ")
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
            if (nodeList[j].Name == "�Ʊ����")
            {
                demand[i].transform.GetChild(index).transform.GetChild(height).gameObject.SetActive(true);
                former = demand[i].transform.GetChild(index).transform.GetChild(height).transform;
                obList.Add(demand[i].transform.GetChild(index).transform.GetChild(height).gameObject);
                height++;
            }
            else if (nodeList[j].Name == "�۱����")
            {
                demand[i].transform.GetChild(index).transform.GetChild(height + 3).gameObject.SetActive(true);
                obList.Add(demand[i].transform.GetChild(index).transform.GetChild(height + 3).gameObject);
                former = demand[i].transform.GetChild(index).transform.GetChild(height+3).transform;
                height++;
            }
            else if (nodeList[j].Name == "�̱����")
            {
                demand[i].transform.GetChild(index).transform.GetChild(height + 6).gameObject.SetActive(true);
                obList.Add(demand[i].transform.GetChild(index).transform.GetChild(height + 6).gameObject);
                former = demand[i].transform.GetChild(index).transform.GetChild(height+6).transform;
                height++;
            }
            else if (nodeList[j].Name == "�ǹ�")
            {
                Debug.Log(former.gameObject.name);
                if (former != null)
                {
                    former.GetChild(0).gameObject.SetActive(true);
                    obList.Add(former.GetChild(0).gameObject);
                }
            }
            else if (nodeList[j].Name == "�ɿ���")
            {
                Debug.Log(former.gameObject.name);
                if (former != null)
                {
                    former.GetChild(1).gameObject.SetActive(true);
                    obList.Add(former.GetChild(1).gameObject);
                }
            }
            else if (nodeList[j].Name == "ӣ��")
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