using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Basic;
using UnityEngine;
/// <summary>
/// 背包控制器
/// </summary>
public class BackpackControl : MonoBehaviour
{
    private List<Article> articles;
    private ArticleList articleList;
    private StudentUnit studentUnit;
    [SerializeField] private GiveGiftsControl giftsControl;
    [SerializeField] private GameObject nullBackpack;//空背包提示
    [SerializeField] private GameObject articleCard;
    [SerializeField] private Transform articleParent;
    private List<ArticleCard> articleCards;

    public void Init(StudentUnit studentUnit)
    {
        articleCards ??= new List<ArticleCard>();
        articleList ??= GameManager.Instance.articleList;
        articles ??= GameManager.Instance.saveObject.SaveData.playerUnit.articles;
        this.studentUnit = studentUnit;
        gameObject.SetActive(true);
        UpdateUI();
    }

    public void UpdateUI()
    {

        nullBackpack.SetActive(articles.Count == 0||articles.All(x=>x.number<=0));//设置空物品提示文字的显示
        while (articles.Count>articleCards.Count)//使卡片数量不少于总物品种类数
        {
            ArticleCard control = Instantiate(articleCard, articleParent).GetComponent<ArticleCard>();
            articleCards.Add(control);
        }
        foreach (var control in articleCards)
            control.gameObject.SetActive(false);//隐藏卡片

        for (int i = 0; i < articles.Count; i++)
            articleCards[i].Init(articles[i]);//初始化卡片，里面带显示
    }

    /// <summary>
    /// 点击背包内物品
    /// </summary>
    public void OnCard(Article article)
    {
        giftsControl.Init(studentUnit,article,articleList.Find_ID(article.id));
    }

    public void Add()
    {
        // Article article = articles.Find(x => x.id == "1");
        // if (article==null)
        // {
        //     Article row2Article = GameManager.Instance.articleList.Find_ID("1").Row2Article();
        //     row2Article.number = 1;
        //     articles.Add(row2Article);
        // }
        // else
        // {
        //     article.number++;
        // }

    }
}