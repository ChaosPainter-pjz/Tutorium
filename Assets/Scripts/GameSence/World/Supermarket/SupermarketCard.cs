using Basic.CSV2Table;
using Basic.CSV2Table.World;
using GameSence.Classroom;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameSence.World.Supermarket
{
    public class SupermarketCard : MonoBehaviour
    {
        [SerializeField] private Text cardName;
        [SerializeField] private Text productionCompany;
        [SerializeField] private Text description;
        [SerializeField] private Text saleProbability;
        [SerializeField] private Text number;
        [SerializeField] private Text price;
        [SerializeField] private Image cardImage;
        [SerializeField] private Button buyButton;
        private UnityAction<string> CallBack;
        private Article thisArticle;

        public void UpdateUI(Article article, SupermarketGoodsList.Row supermarketGoodRow, ArticleList.Row articleRow,
            Sprite image, UnityAction<string> callBack)
        {
            cardName.text = articleRow.Name;
            productionCompany.text = supermarketGoodRow.productionCompany;
            saleProbability.text = supermarketGoodRow.state;
            description.text = articleRow.description;
            number.text = article.number > 0 ? "剩余" + article.number : "无货";
            price.text = supermarketGoodRow.price;
            CallBack = callBack;
            cardImage.sprite = image;
            thisArticle = article;
            buyButton.gameObject.SetActive(article.number > 0);
        }

        public void OnBuyButton()
        {
            CallBack(thisArticle.id);
        }
    }
}