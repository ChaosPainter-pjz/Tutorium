using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bookstore
{

    public class BookstorePanelControl : MonoBehaviour
    {
        [SerializeField]private BookstoreManager bookstoreManager;
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