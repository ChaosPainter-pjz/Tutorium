using System;
using UnityEngine;

namespace Basic
{
    public class AudioControl : MonoInstance<AudioControl>
    {
        [Header("BGM")] public AudioClip[] audioClips;
        [Header("音效")] public AudioClip[] soundAudioClips;
        [Header("UI音效")] public AudioClip[] uiAudioClips;
        public AudioSource audioSource;

        /// <summary>
        ///  按照音频文件名播放音乐
        /// </summary>
        /// <param name="audioName"></param>
        public void PlayAudio(string audioName)
        {
            foreach (var audioClip in audioClips)
                if (audioClip.name == audioName)
                {
                    audioSource.clip = audioClip;
                    audioSource.Play();
                }
        }

        public enum BackgroundMusicType
        {
            Main,
            World,
            Classroom,
            StudentRoom,
            BookStore,
            StoryLine,
            Ktv,
            GameOver,
            Supermarket
        }

        [SerializeField] public BackgroundMusicType currentType = BackgroundMusicType.Main;

        /// <summary>
        /// 播放背景音乐
        /// </summary>
        public void PlayBackgroundMusic(BackgroundMusicType type)
        {
            if (currentType == type && currentType != BackgroundMusicType.StoryLine) return;
            switch (type)
            {
                case BackgroundMusicType.Main:
                    PlayLoop("Mellow beginnings");
                    break;
                case BackgroundMusicType.World:
                    PlayLoop("Beautiful Dreamer(world)");
                    break;
                case BackgroundMusicType.Classroom:
                    PlayLoop("魔王魂  ピアノ26(Classroom)");
                    break;
                case BackgroundMusicType.StudentRoom:
                    break;
                case BackgroundMusicType.Ktv:
                    PlayLoop("摇滚金属乐-动感十足节奏感强-梦指苍穹荡气回肠(KTV)");
                    break;
                case BackgroundMusicType.BookStore:
                case BackgroundMusicType.StoryLine:
                case BackgroundMusicType.GameOver:
                    PlayLoop("众星之子(Starchild)");
                    break;
                case BackgroundMusicType.Supermarket:
                    PlayLoop("Holiday Gift");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            currentType = type;
        }

        public void PlaySound(AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip);
        }

        public void PlaySound(string audioClip)
        {
            foreach (var clip in soundAudioClips)
                if (clip.name == audioClip)
                    audioSource.PlayOneShot(clip);
        }

        /// <summary>
        /// 停止播放所有的音频
        /// </summary>
        public void StopPlayAll()
        {
            // foreach (AudioClip audioClip in audioClips)
            // {
            //     audioSource.clip = audioClip;
            //     audioSource.Stop();
            // }
            audioSource.Stop();
        }

        /// <summary>
        /// 按照音频文件名停止播放某个音频
        /// </summary>
        public void StopPlayOne(string audioName)
        {
            foreach (var audioClip in audioClips)
                if (audioClip.name == audioName)
                {
                    audioSource.clip = audioClip;
                    audioSource.Stop();
                }
        }

        /// <summary>
        /// 循环播放某个音频
        /// </summary>
        public void PlayLoop(string audioName, BackgroundMusicType type)
        {
            currentType = type;
            foreach (var audioClip in audioClips)
                if (audioClip.name == audioName)
                {
                    audioSource.clip = audioClip;
                    audioSource.loop = true; //设置声音为循环播放 ;
                    audioSource.Play();
                    return;
                }

            Debug.LogWarning($"找不到指定名称的BGM{audioName}");
        }

        /// <summary>
        /// 循环播放某个音频
        /// </summary>
        private void PlayLoop(string audioName)
        {
            foreach (var audioClip in audioClips)
                if (audioClip.name == audioName)
                {
                    audioSource.clip = audioClip;
                    audioSource.loop = true; //设置声音为循环播放 ;
                    audioSource.Play();
                    return;
                }

            Debug.LogWarning($"找不到指定名称的BGM{audioName}");
        }

        public void PlayLoop(AudioClip audioClip, BackgroundMusicType type)
        {
            currentType = type;
            audioSource.clip = audioClip;
            audioSource.loop = true; //设置声音为循环播放 ;
            audioSource.Play();
        }
    }
}