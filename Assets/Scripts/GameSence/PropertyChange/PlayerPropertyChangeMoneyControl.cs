using UnityEngine;
using UnityEngine.UI;

namespace GameSence.PropertyChange
{
    public class PlayerPropertyChangeMoneyControl : MonoBehaviour
    {
        [SerializeField] private Text lowMoneyText;
        [SerializeField] private Text newMoneyText;
        public int LowMoneyNumber { get; set; }

        public void SetMoneyPanel(int newMoney)
        {
            if (LowMoneyNumber == newMoney)
            {
                gameObject.SetActive(false);
            }
            else
            {
                gameObject.SetActive(true);
                lowMoneyText.text = LowMoneyNumber.ToString();
                newMoneyText.text = newMoney.ToString();
                transform.SetAsLastSibling();
            }
        }
    }
}