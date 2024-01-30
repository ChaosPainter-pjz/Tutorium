using System.Collections.Generic;
using Basic;
using Basic.CSV2Table;
using Basic.CSV2Table.World;
using GameSence.Classroom;
using GameSence.GameManager;
using GameSence.Hint;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameSence.World.Supermarket
{
    public class SupermarketControl : MonoBehaviour
    {
        [SerializeField] private GameManager.GameManager gameManager;
        [SerializeField] private ResourceManager resourceManager;
        [SerializeField] private AudioControl bgmAudioControl;
        [SerializeField] private EnterPanelControl enterPanelControl;
        [SerializeField] private GameObject prefab;
        [SerializeField] private Transform parent;
        [SerializeField] private Text money;

        /// <summary>
        /// 当前展示的卡片集合
        /// </summary>
        private List<SupermarketCard> supermarketCards;

        private Unit.Date supermarketDate;
        private Unit.Date date;

        /// <summary>
        /// 超市现有的商品
        /// </summary>
        private List<Article> supermarketCommodities;

        /// <summary>
        /// 可用商品集合
        /// </summary>
        private SupermarketGoodsList supermarketGoodsList;

        // Start is called before the first frame update
        private void OnEnable()
        {
            supermarketDate ??= gameManager.saveObject.SaveData.supermarketDate;
            date ??= gameManager.saveObject.SaveData.gameDate;
            supermarketGoodsList ??= gameManager.SupermarketGoodsList;
            supermarketCommodities ??= gameManager.saveObject.SaveData.supermarketCommodities;
            supermarketCards ??= new List<SupermarketCard>();
            bgmAudioControl.PlayBackgroundMusic(AudioControl.BackgroundMusicType.Supermarket);
            if (date.year != supermarketDate.year
                || (date.Semester == supermarketDate.Semester && date.Week - supermarketDate.Week >= 3)
                || (date.Semester != supermarketDate.Semester &&
                    date.Week + Unit.Date.MaxWeek - supermarketDate.Week >= 3))
            {
                UpdateCommodity();
                //进货后要刷新进货日期
                supermarketDate.year = date.year;
                supermarketDate.Semester = date.Semester;
                supermarketDate.Week = date.Week;
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            while (supermarketCommodities.Count > supermarketCards.Count)
            {
                var card = Instantiate(prefab, parent).GetComponent<SupermarketCard>();
                supermarketCards.Add(card);
            }

            for (var i = 0; i < supermarketCommodities.Count; i++)
            {
                var row1 = gameManager.SupermarketGoodsList.Find_id(supermarketCommodities[i].id);
                var row2 = gameManager.articleList.Find_ID(supermarketCommodities[i].id);
                var sprite = resourceManager.articleList[0];
                foreach (var spr in resourceManager.articleList)
                    if (spr.name == supermarketCommodities[i].id)
                    {
                        sprite = spr;
                        break;
                    }

                supermarketCards[i].UpdateUI(supermarketCommodities[i], row1, row2, sprite, BuyButton);
            }

            money.text = gameManager.saveObject.SaveData.money.ToString();
        }

        /// <summary>
        /// 超市进货商品
        /// </summary>
        private void UpdateCommodity()
        {
            foreach (var raw in supermarketGoodsList.GetRowList())
            {
                var gl = float.Parse(raw.SaleProbability);

                var range = Random.Range(0f, 1f);
                //Debug.Log(range);
                if (range <= gl)
                {
                    //到这儿，就是进货了该商品
                    var art = supermarketCommodities.Find(article => article.id == raw.id);
                    var min = int.Parse(raw.minQuantity);
                    var max = int.Parse(raw.maxQuantity);
                    var number = Random.Range(min, max + 1);
                    if (art == null)
                    {
                        var articleName = gameManager.articleList.Find_ID(raw.id);
                        supermarketCommodities.Add(new Article(raw.id, articleName.Name, number));
                    }
                    else
                    {
                        if (art.number + number > int.Parse(raw.maxQuantity))
                            art.number = max;
                        else
                            art.number += number;
                    }
                }
            }
        }

        /// <summary>
        /// 点击了购买按钮
        /// </summary>
        /// <param name="id"></param>
        private void BuyButton(string id)
        {
            var sprite = resourceManager.articleList[0];
            foreach (var spr in resourceManager.articleList)
                if (spr.name == id)
                {
                    sprite = spr;
                    break;
                }

            var price = supermarketGoodsList.Find_id(id).price;
            var canBuy = gameManager.saveObject.SaveData.money >= int.Parse(price);
            var article = gameManager.saveObject.SaveData.playerUnit.articles.Find(art => art.id == id);
            var haveNumber = article?.number ?? 0;
            enterPanelControl.UpdateUI(id, price, gameManager.articleList.Find_ID(id).Name, sprite, canBuy, haveNumber,
                EnterBuy);
        }

        /// <summary>
        /// 点击了确认购买
        /// </summary>
        /// <param name="id"></param>
        private void EnterBuy(string id)
        {
            var price = int.Parse(supermarketGoodsList.Find_id(id).price);
            if (MoneyManager.Instance.Money - price >= 0)
            {
                //付费
                MoneyManager.Instance.Money -= price;
                //减少超市的库存
                supermarketCommodities.Find(art => art.id == id).number--;
                //把物品带回去
                var article = gameManager.saveObject.SaveData.playerUnit.articles.Find(art => art.id == id);
                var articleName = gameManager.articleList.Find_ID(id).Name;
                if (article == null)
                    gameManager.saveObject.SaveData.playerUnit.AddArticle(id, articleName, 1);
                else
                    article.number++;
                UpdateUI();
                HintManager.Instance.AddHint(new Hint.Hint("付款成功", $"{articleName}已存入库中，可在赠送面板中查看"));
            }
            else
            {
                Debug.LogError("余额不足");
            }
        }

        public void OnDisable()
        {
            //bgmAudioControl.StopPlayAll();
            bgmAudioControl.PlayBackgroundMusic(AudioControl.BackgroundMusicType.World);
        }
    }
}