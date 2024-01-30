using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameSence.World.Supermarket
{
    public class EnterPanelControl : MonoBehaviour
    {
        [SerializeField] private Button enter;
        [SerializeField] private Image image;
        [SerializeField] private Text price;
        [SerializeField] private Text goodName;
        [SerializeField] private Text haveNumber;
        private UnityAction<string> enterEvent;
        private string goodId;

        public void UpdateUI(string _goodId, string _price, string _goodName, Sprite sprite, bool canBuy,
            int _haveNumber, UnityAction<string> callBack)
        {
            image.sprite = sprite;
            price.text = "￥" + _price;
            goodName.text = _goodName;
            enter.interactable = canBuy;
            enterEvent = callBack;
            goodId = _goodId;
            haveNumber.text = _haveNumber != 0 ? "拥有 " + _haveNumber : "";
            gameObject.SetActive(true);
        }

        public void OnEnter()
        {
            enterEvent(goodId);
            gameObject.SetActive(false);
        }
    }
}