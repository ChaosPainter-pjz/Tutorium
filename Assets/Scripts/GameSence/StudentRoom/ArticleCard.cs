using GameSence.Classroom;
using GameSence.GameManager;
using UnityEngine;
using UnityEngine.UI;

namespace GameSence.StudentRoom
{
    /// <summary>
    /// 背包内的单个物品控制器
    /// </summary>
    public class ArticleCard : MonoBehaviour
    {
        public Article article;
        [SerializeField] private Image icon;

        public void Init(Article article)
        {
            if (article.number <= 0) return;
            gameObject.SetActive(true);
            this.article = article;
            foreach (var sprite in ResourceManager.Instance.articleList)
                if (sprite.name == article.id)
                {
                    icon.sprite = sprite;
                    break;
                }
        }

        public void OnButton()
        {
            GetComponentInParent<BackpackControl>().OnCard(article);
        }
    }
}