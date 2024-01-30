using UnityEngine;
using UnityEngine.UI;

namespace GameSence.StudentRoom
{
    public class PhotoControl : MonoBehaviour
    {
        private Sprite sprite;
        [SerializeField] private Image image;
        private PhotoAlbumControl photoAlbumControl;

        public void Init(Sprite _sprite, PhotoAlbumControl _photoAlbumControl)
        {
            gameObject.SetActive(true);
            sprite = _sprite;
            image.sprite = sprite;
            photoAlbumControl = _photoAlbumControl;
        }

        public void Open()
        {
            photoAlbumControl.OnPhoto(sprite);
        }
    }
}