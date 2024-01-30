using System.Collections.Generic;
using Basic;
using Basic.CSV2Table.GameOver;
using UnityEngine;
using UnityEngine.UI;

namespace StartScene.Galleries
{
    /// <summary>
    /// 追忆面板控制器
    /// </summary>
    public class GalleriesControl : MonoBehaviour
    {
        [SerializeField] private SaveObject saveObject;
        [SerializeField] private Transform galleriesParent;
        [SerializeField] private GameObject galleriesPrefab;
        [SerializeField] private Text proportion;
        private GameOverList gameOverList;
        [SerializeField] private TextAsset gameOverTextAsset;
        private List<GalleriesCardControl> cardControls;

        private void Start()
        {
            gameOverList = new GameOverList(gameOverTextAsset);
            //gameOverList.Load(gameOverTextAsset);
            cardControls = new List<GalleriesCardControl>();
            while (cardControls.Count < saveObject.OverSaveData.overUnits.Count)
            {
                var control = Instantiate(galleriesPrefab, galleriesParent).GetComponent<GalleriesCardControl>();
                cardControls.Add(control);
            }

            for (var i = 0; i < saveObject.OverSaveData.overUnits.Count; i++)
                cardControls[i].Init(saveObject.OverSaveData.overUnits[i],
                    gameOverList.Find_EndID(saveObject.OverSaveData.overUnits[i].overId));

            proportion.text = saveObject.OverSaveData.overUnits.Count + "/" + gameOverList.Count();
        }
    }
}
