using UnityEngine;

namespace GameSence.StudentRoom
{
    public class PhotoBigControl : MonoBehaviour
    {
        /// <summary>
        /// 点击大照片，以关闭大照片（动画事件）
        /// </summary>
        public void ClosePhotoBig()
        {
            gameObject.SetActive(false);
        }
    }
}