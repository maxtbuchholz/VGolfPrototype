using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFadeIn : MonoBehaviour
{
    [SerializeField] Image Button;
    [SerializeField] TextMeshProUGUI ButtonText;
    [SerializeField] float StartOffset = 0;
    [SerializeField] float TimeToFull = 2;
    void Start()
    {
        time -= StartOffset;
    }

    private float time = 0;
    void Update()
    {
        time += Time.deltaTime;
        if(time <= 0)
        {
            Color buttonColor = Button.color;
            buttonColor.a = 0;
            Button.color = buttonColor;

            Color textColor = ButtonText.color;
            textColor.a = 0;
            ButtonText.color = textColor;
        }
        if(time > TimeToFull)
        {
            Color buttonColor =  Button.color;
            buttonColor.a = 1;
            Button.color = buttonColor;

            Color textColor = ButtonText.color;
            textColor.a = 1;
            ButtonText.color = textColor;

        }
        else
        {
            float perc = time / TimeToFull;
            Color buttonColor = Button.color;
            buttonColor.a = perc;
            Button.color = buttonColor;

            Color textColor = ButtonText.color;
            textColor.a = perc;
            ButtonText.color = textColor;
        }
    }
}
