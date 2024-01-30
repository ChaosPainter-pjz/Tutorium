using System.Linq;
using Basic.CSV2Table;
using GameSence.Classroom;
using GameSence.GameManager;
using GameSence.Hint;
using Unit;
using UnityEngine;
using UnityEngine.UI;

namespace GameSence.StudentRoom
{
    /// <summary>
    /// 确认赠送页面
    /// </summary>
    public class GiveGiftsControl : MonoBehaviour
    {
        [SerializeField] private BackpackControl backpackControl;
        [SerializeField] private Image icon;
        [SerializeField] private Text articleName;
        [SerializeField] private Text description;
        [SerializeField] private Text number;
        [SerializeField] private Text enterText;
        [SerializeField] private new ParticleSystem particleSystem;
        [SerializeField] private StudentUnit studentUnit;
        private Article article;
        private ArticleList.Row row;

        public void Init(StudentUnit _studentUnit, Article _article, ArticleList.Row _row)
        {
            gameObject.SetActive(true);
            studentUnit = _studentUnit;
            article = _article;
            row = _row;
            foreach (var sprite in ResourceManager.Instance.articleList)
                if (sprite.name == _article.id)
                {
                    icon.sprite = sprite;
                    break;
                }

            enterText.text = JudgeGiftStatus(studentUnit, article) ? "已拥有" : "确认";
            articleName.text = _row.Name;
            description.text = _row.description;
            number.text = _article.number.ToString();
        }

        public void OnButton()
        {
            UseArticle(studentUnit, article);
            /*
             * 这里要实现道具的功能
             * 使用道具后提醒
             */
            //Debug.Log("用掉了" + article.name);
            particleSystem.Stop();
            particleSystem.Play();
            backpackControl.UpdateUI();
            //gameObject.SetActive(false);
            GetComponent<Animator>().Play("ExitPanel");
        }

        /// <summary>
        /// 判断赠送状态
        /// </summary>
        /// <returns></returns>
        private static bool JudgeGiftStatus(StudentUnit unit, Article article)
        {
            var any = unit.articles.Any(x => x.id == article.id);
            return any;
        }

        /// <summary>
        /// 使用道具,是否添加到学生背包是根据有没有展示或有没有获取数量限制来决定的
        /// 如 物品只限一个 那么就添加；物品需要展示 那么也添加
        /// 不限个数，又要展示的，在 JudgeGiftStatus方法中添加判定
        /// </summary>
        private static void UseArticle(StudentUnit unit, Article article)
        {
            article.number--;
            switch (article.id)
            {
                case "1":
                    unit.Mood += 10;
                    HintManager.Instance.AddHint(new Hint.Hint(article.name, $"吸溜~简简单单的快乐，{unit.fullName}心情+10"));
                    break;
                case "2":
                    if (unit.id == "3")
                    {
                        unit.Mood += 20;
                        HintManager.Instance.AddHint(new Hint.Hint(article.name,
                            $"{unit.fullName}非常喜欢三尾仓鼠，和它玩了一晚上，心情+20"));
                    }
                    else
                    {
                        unit.Mood += 10;
                        HintManager.Instance.AddHint(new Hint.Hint(article.name,
                            $"{unit.fullName}认为小宠物挺有意思的，给仓鼠起名“备用口粮”。心情+10"));
                    }

                    break;
                case "3":
                    if (unit.id == "2")
                    {
                        unit.Mood += 20;
                        HintManager.Instance.AddHint(new Hint.Hint(article.name, $"免费公交，去你想去。周·街溜·子轩表示非常满意。心情+20"));
                    }
                    else
                    {
                        unit.Mood += 10;
                        HintManager.Instance.AddHint(
                            new Hint.Hint(article.name, $"{unit.fullName}收下了学生卡，并没有过多表示。心情+10"));
                    }

                    break;
                case "4":
                    if (unit.id == "1")
                    {
                        unit.Mood += 20;
                        HintManager.Instance.AddHint(new Hint.Hint(article.name,
                            $"现在{unit.fullName}每天都会学习一个小知识，虽然没有什么大用，但是心情确实变好了。心情+20"));
                    }
                    else
                    {
                        unit.Mood += 10;
                        HintManager.Instance.AddHint(new Hint.Hint(article.name,
                            $"{unit.fullName}看了一眼会员状态，并没有过多表示。心情+10"));
                    }

                    break;
                case "5":
                    if (unit.id == "4")
                    {
                        unit.Mood += 20;
                        HintManager.Instance.AddHint(new Hint.Hint(article.name, $"甜党的胜利。{unit.fullName}心情+20"));
                    }
                    else
                    {
                        unit.Mood += 10;
                        HintManager.Instance.AddHint(new Hint.Hint(article.name, $"饱餐一顿，但是也不敢吃多。{unit.fullName}心情+10"));
                    }

                    break;
                case "6":
                    unit.Mood += 10;
                    unit.properties.Find(x => x.gradeID == "temperament").score += 8;
                    unit.interestGrade.Find(x => x.gradeID == "10").score += 50;
                    unit.interestGrade.Find(x => x.gradeID == "11").score += 30;
                    unit.interestGrade.Find(x => x.gradeID == "13").score += 10;
                    if (unit.articles.All(x => x.id != article.id))
                    {
                        unit.articles.Add(article.Copy());
                        if (unit.id == "2")
                        {
                            unit.Trust += 5;
                            HintManager.Instance.AddHint(new Hint.Hint(article.name,
                                $"{unit.fullName}得到了吉他，心情+10、信任+5、气质+8、音乐+50、表演+30、手工+10"));
                        }
                        else
                        {
                            HintManager.Instance.AddHint(new Hint.Hint(article.name,
                                $"{unit.fullName}得到了吉他，心情+10、气质+8、音乐+50、表演+30、手工+10"));
                        }
                    }

                    break;
                case "7":
                    unit.Mood += 15;
                    unit.interestGrade.Find(x => x.gradeID == "13").score += 20;
                    unit.articles.Add(article.Copy());
                    HintManager.Instance.AddHint(new Hint.Hint(article.name, $"{unit.fullName}得到了干花摆件，心情+15、手工+20"));
                    break;
                case "8":
                    unit.Mood += 10;
                    unit.interestGrade.Find(x => x.gradeID == "20").score += 40;
                    unit.properties.Find(x => x.gradeID == "temperament").score += 10;
                    if (unit.articles.All(x => x.id != article.id)) unit.articles.Add(article.Copy());

                    HintManager.Instance.AddHint(new Hint.Hint(article.name,
                        $"{unit.fullName}得到了画板，心情+15、绘画+40、气质+10"));
                    break;
                case "9":
                    unit.properties.Find(x => x.gradeID == "thought").score += 6;
                    unit.interestGrade.Find(x => x.gradeID == "14").score += 50;
                    unit.interestGrade.Find(x => x.gradeID == "13").score += 10;
                    if (unit.articles.All(x => x.id != article.id)) unit.articles.Add(article.Copy());

                    HintManager.Instance.AddHint(new Hint.Hint(article.name, $"{unit.fullName}得到了象棋，思维+6、棋技+50、手工+10"));
                    break;
                case "10":
                    var mood = unit.gender == Gender.Man ? 40 : 15;
                    unit.Mood += mood;
                    unit.Thought.score += 6;
                    unit.interestGrade.Find(x => x.gradeID == "13").score += 40;
                    unit.Physique.score += 6;
                    HintManager.Instance.AddHint(new Hint.Hint(article.name,
                        $"{unit.fullName}得到了一套高达，并且组装了起来，思维+6、手工+40、心情+{mood}、体质+6"));
                    break;
                case "11":
                    unit.Physique.score += 6;
                    unit.Temperament.score += 5;
                    unit.Mood += 10;
                    unit.interestGrade.Find(x => x.gradeID == "21").score += 10;
                    HintManager.Instance.AddHint(new Hint.Hint(article.name,
                        $"{unit.fullName}得到了一对哑铃，练了一会儿就失去了兴趣，体质+6、心情+10、运动+10、气质+5"));
                    break;
                case "12":
                    unit.Physique.score += 10;
                    unit.Mood += 10;
                    unit.interestGrade.Find(x => x.gradeID == "21").score += 10;
                    unit.interestGrade.Find(x => x.gradeID == "12").score += 30;
                    unit.articles.Add(article.Copy());
                    HintManager.Instance.AddHint(new Hint.Hint(article.name,
                        $"{unit.fullName}表示很喜欢，当天就在垫子上奔奔跳跳到凌晨12点。体质+10、运动+10、舞蹈+30、心情+10"));
                    break;
                case "13":
                    unit.Mood += 10;
                    unit.Thought.score += 3;
                    unit.interestGrade.Find(x => x.gradeID == "18").score += 40;
                    unit.mainGrade[4].score += 8;
                    unit.mainGrade[5].score += 10;
                    unit.mainGrade[8].score += 10;
                    HintManager.Instance.AddHint(new Hint.Hint(article.name,
                        $"由于这种化石并不坚固，{unit.fullName}不小心捏碎了三叶虫化石。心情+10、考古+40、思维+3、历史+8、生物+10、地理+10"));
                    break;
                case "14":

                    foreach (var grade in unit.mainGrade) grade.score += 20;

                    HintManager.Instance.AddHint(new Hint.Hint(article.name,
                        $"{unit.fullName}接受这个散发荣耀光芒的奖杯，心中意志坚定了一分。全文化课+20"));
                    break;
                case "15":
                    if (unit.GoodAndEvil.score < 0) unit.GoodAndEvil.score = 0;
                    HintManager.Instance.AddHint(
                        new Hint.Hint(article.name, $"{unit.fullName}深刻的认识到了之前的所作所为是不对的。改邪归正了"));
                    break;
                case "16":
                    var nu = "";
                    for (var i = 0; i < 4; i++)
                    {
                        var random = new System.Random(i);
                        var n = random.Next(0, 4);
                        unit.properties[n].score++;
                        nu += unit.properties[n].name + "+1 ";
                    }

                    HintManager.Instance.AddHint(new Hint.Hint(article.name, $"{unit.fullName}：牛奶很好喝，要是每天都有就好了{nu}"));
                    break;
                case "17":
                    var date = GameManager.GameManager.Instance.saveObject.SaveData.gameDate;
                    if (date.Semester == 0 && date.Week < Unit.Date.MaxWeek / 2) //夏天
                    {
                        unit.Mood += 15;
                        HintManager.Instance.AddHint(
                            new Hint.Hint(article.name, $"炎炎酷暑，{unit.fullName}吃了西瓜后非常开心，心情+15"));
                    }
                    else
                    {
                        unit.Mood += 5;
                        HintManager.Instance.AddHint(new Hint.Hint(article.name,
                            $"现在不是夏天，{unit.fullName}吃了西瓜只觉得味道不错，心情+5"));
                    }

                    break;
                default:
                    Debug.Log("未配置的物品id：" + article.id);
                    break;
            }
        }

        /// <summary>
        /// 退出面板
        /// </summary>
        public void ExitEnterPanel()
        {
            gameObject.SetActive(false);
        }
    }
}