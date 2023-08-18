using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ThumbsUpAnimation : MonoBehaviour
{
    [SerializeField] SpriteRenderer Thumbs;
    private float OriginalScale;
    private float LargeScale;
    private float SmallScale;
    private float ScaleDiff = 0.01f;
    private float SmallToLargeLinearScaleDiff;
    private float LargeToOriginalScaleDiff;
    private float TimeForScale = 0.5f;
    private float MiddleWaitTime = 0f;
    private float TimeForScale2 = 0.2f;
    int DoCurrAnimation = -1;
    float AnimationTime = 0;
    void Start()
    {
        //Thumbs = GetComponent<SpriteRenderer>();
        if(DoCurrAnimation == -1)
            Thumbs.enabled = false;
        OriginalScale = transform.localScale.x;
        LargeScale = OriginalScale + 10;
        SmallScale = OriginalScale * ScaleDiff;
        SmallToLargeLinearScaleDiff = LargeScale - SmallScale;
        LargeToOriginalScaleDiff = LargeScale - OriginalScale;
    }
    public void StartAnimation()
    {
        Thumbs.enabled = true;
        DoCurrAnimation = 0;
    }
    public void Update()
    {
        if (DoCurrAnimation != -1)
            AnimationTime += Time.deltaTime;
        if (DoCurrAnimation == 0)
        {
            if(AnimationTime < TimeForScale)
            {
                //linear
                //float Scale = ((AnimationTime / TimeForScale) * SmallToLargeLinearScaleDiff) + SmallScale;
                //Root-ear
                float Scale = (SmallToLargeLinearScaleDiff * Mathf.Pow(AnimationTime / TimeForScale, (1f/4f))) + SmallScale;
                transform.localScale = new Vector2(Scale,Scale);
            }
            else
            {
                transform.localScale = new Vector2(LargeScale, LargeScale);
                DoCurrAnimation = 1;
                AnimationTime = 0;
            }
        }
        else if(DoCurrAnimation == 1)
        {
            if (AnimationTime >= MiddleWaitTime)
            {
                DoCurrAnimation = 2;
                AnimationTime = 0;
            }
        }
        else if (DoCurrAnimation == 2)
        {
            if (AnimationTime < TimeForScale2)
            {
                float Scale = ((1 - (AnimationTime / TimeForScale2)) * LargeToOriginalScaleDiff) + OriginalScale;
                transform.localScale = new Vector2(Scale, Scale);
            }
            else
            {
                transform.localScale = new Vector2(OriginalScale, OriginalScale);
                DoCurrAnimation = -1;
                AnimationTime = 0;
            }
        }
    }
}
