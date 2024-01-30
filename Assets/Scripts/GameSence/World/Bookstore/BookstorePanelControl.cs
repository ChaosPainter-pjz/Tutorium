using Basic;
using UnityEngine;

namespace GameSence.World.Bookstore
{
    public class BookstorePanelControl : MonoBehaviour
    {
        [SerializeField] private BookstoreManager bookstoreManager;
        [SerializeField] private AudioControl bgmAudioControl;

        private void OnEnable()
        {
            bookstoreManager.UpdateUI();
        }

        public void OnDisable()
        {
            bgmAudioControl.StopPlayAll();
            bgmAudioControl.PlayBackgroundMusic(AudioControl.BackgroundMusicType.World);
        }
    }
}