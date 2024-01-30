using System.Collections.Generic;
using Basic;
using UnityEngine;

namespace GameSence.Hint
{
    public class HintManager : MonoInstance<HintManager>
    {
        [SerializeField] private SaveObject saveObject;
        public Queue<Hint> hints = new();
        [SerializeField] public AudioSource audioSource;
        [Header("面板")] [SerializeField] private Transform parent;
        [SerializeField] private GameObject prefab;
        [SerializeField] private GameObject noHint;
        private List<HintCardControl> hintCardControls;

        public void AddHint(Hint addHint)
        {
            if (addHint.Headline == "") addHint.Headline = "提示";
            saveObject.SaveData.hints.Add(addHint);
            hints.Enqueue(addHint);
            audioSource.Play();
        }

        public Hint DeQueue()
        {
            if (hints.Count <= 0) return null;
            return hints.Dequeue();
        }

        /// <summary>
        /// 刷新班级信息面板中的消息列表
        /// </summary>
        public void UpdateHintPanelUI()
        {
            hintCardControls ??= new List<HintCardControl>();
            noHint.SetActive(saveObject.SaveData.hints.Count == 0);
            while (hintCardControls.Count < saveObject.SaveData.hints.Count)
            {
                var control = Instantiate(prefab, parent).GetComponent<HintCardControl>();
                hintCardControls.Add(control);
            }

            foreach (var control in hintCardControls) control.gameObject.SetActive(false);

            for (var i = 0; i < saveObject.SaveData.hints.Count; i++)
                hintCardControls[i].Init(saveObject.SaveData.hints[i]);
        }
    }
}
