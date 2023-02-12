using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoControl : MonoBehaviour
{
    private Sprite sprite;
    [SerializeField]private Image image;
    private PhotoAlbumControl photoAlbumControl;

    public void Init(Sprite _sprite,PhotoAlbumControl _photoAlbumControl)
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