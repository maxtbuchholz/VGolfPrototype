using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnivFadeIn : MonoBehaviour
{
    [SerializeField] Image[] images;
    [SerializeField] TextMeshProUGUI[] texts;
    [SerializeField] SpriteRenderer[] sprites;
    [SerializeField] float StartOffset = 0;
    [SerializeField] float TimeToFull = 2;
    void Start()
    {
        time -= StartOffset;
        images = transform.GetComponentsInChildren<Image>();
        texts = transform.GetComponentsInChildren<TextMeshProUGUI>();
        sprites = transform.GetComponentsInChildren<SpriteRenderer>();
    }

    private float time = 0;
    void Update()
    {
        time += Time.deltaTime;
        if(time <= 0)
        {
            for (int i = 0; i < images.Length; i++)
            {
                Color buttonColor = images[i].color;
                buttonColor.a = 0;
                images[i].color = buttonColor;
            }
            for (int i = 0; i < texts.Length; i++)
            {
                Color textColor = texts[i].color;
                textColor.a = 0;
                texts[i].color = textColor;
            }
            for (int i = 0; i < sprites.Length; i++)
            {
                Color spriteColor = sprites[i].color;
                spriteColor.a = 0;
                sprites[i].color = spriteColor;
            }
        }
        if(time > TimeToFull)
        {
            for (int i = 0; i < images.Length; i++)
            {
                Color buttonColor = images[i].color;
                buttonColor.a = 1;
                images[i].color = buttonColor;
            }
            for (int i = 0; i < texts.Length; i++)
            {
                Color textColor = texts[i].color;
                textColor.a = 1;
                texts[i].color = textColor;
            }
            for (int i = 0; i < sprites.Length; i++)
            {
                Color spriteColor = sprites[i].color;
                spriteColor.a = 1;
                sprites[i].color = spriteColor;
            }
            Destroy(this);

        }
        else
        {
            float perc = time / TimeToFull;
            for (int i = 0; i < images.Length; i++)
            {
                Color buttonColor = images[i].color;
                buttonColor.a = perc;
                images[i].color = buttonColor;
            }
            for (int i = 0; i < texts.Length; i++)
            {
                Color textColor = texts[i].color;
                textColor.a = perc;
                texts[i].color = textColor;
            }
            for (int i = 0; i < sprites.Length; i++)
            {
                Color spriteColor = sprites[i].color;
                spriteColor.a = perc;
                sprites[i].color = spriteColor;
            }
        }
    }
}
