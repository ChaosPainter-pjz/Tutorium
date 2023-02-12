using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotoAlbumControl : MonoBehaviour
{
    [SerializeField] private StudentRoomControl studentRoomControl;
    [SerializeField] private GameObject nullPhotoAlbum;
    [SerializeField] private List<PhotoControl> photoControls;
    [SerializeField] private GameObject photoObj;
    [SerializeField] private Transform photoParent;
    [SerializeField] private Image photoBig;
    private StudentUnit studentUnit;

    void Start()
    {
    }

    public void Open()
    {
        gameObject.SetActive(true);
        Init();
        UpdateUI();
    }

    private void Init()
    {
        studentUnit = studentRoomControl.studentUnit;
        photoControls ??= new List<PhotoControl>();
    }

    private void UpdateUI()
    {
        foreach (var control in photoControls)//隐藏所有照片
        {
            control.gameObject.SetActive(false);
        }

        nullPhotoAlbum.SetActive(studentUnit.photoAlbum.Count == 0);//控制空相册的提示

        while (studentUnit.photoAlbum.Count > photoControls.Count)//补充不足的相册框物体
        {
            var control = Instantiate(photoObj, photoParent).GetComponent<PhotoControl>();
            photoControls.Add(control);
        }

        for (int i = 0; i < studentUnit.photoAlbum.Count; i++)//设置照片
        {
            foreach (var sprite in ResourceManager.Instance.PhotoAlbum)
            {
                if (sprite.name ==studentUnit.photoAlbum[i])
                {
                    photoControls[i].Init(sprite, this);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 点击照片
    /// </summary>
    public void OnPhoto(Sprite sprite)
    {
        photoBig.sprite = sprite;
        photoBig.gameObject.SetActive(true);
    }


    public void AddPhoto()
    {
        studentUnit.photoAlbum.Add("肖薇坐船");
        UpdateUI();
    }
}