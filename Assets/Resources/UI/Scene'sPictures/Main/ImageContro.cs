using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageContro : MonoBehaviour
{
    public Sprite[] Image;
    public int LiveTime;
    public Transform EndPoint;
    public Transform StartPoint;
    private int NowTime;
    private int ImageIndex;

    void Awake()
    {
        NowTime = 0;
        ImageIndex = 0;
    }
    void Update()
    {
        UpdatePosition();
        ChangeSprite();
    }

    void UpdatePosition()
    {
        Vector3 pos = transform.position;
        transform.position = Vector3.Lerp(pos, EndPoint.position, 0.1f);
        if(Vector3.Distance(transform.position, EndPoint.position) <1)
        {
            transform.position = EndPoint.position;
        }
    }

    void ChangeSprite()
    {
        if (NowTime == 0)
        {
            transform.position = StartPoint.position;
            GetComponent<Image>().sprite = Image[ImageIndex];
            ImageIndex = (ImageIndex + 1) % Image.Length;
            NowTime = LiveTime;
            return;
        }
        NowTime--;
    }
}
