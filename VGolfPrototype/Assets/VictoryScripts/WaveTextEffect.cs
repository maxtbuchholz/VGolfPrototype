using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class WaveTextEffect : MonoBehaviour
{
    [SerializeField] GameObject childCopyObj;
    [SerializeField] string textToShow = "Put Text Here";
    [SerializeField] bool GoAtStart = false;
    private GameObject[] letters;
    private float offset = 0f;
    private float originalScale;
    private float currOffset = 0;
    void Start()
    {
        childCopyObj.GetComponent<TextMeshProUGUI>().text = ""+ textToShow;
        float origWidth = childCopyObj.GetComponent<TextMeshProUGUI>().preferredWidth;
        offset = childCopyObj.GetComponent<TextMeshProUGUI>().fontSize;
        originalScale = childCopyObj.transform.localScale.x;
        letters = new GameObject[textToShow.Length];
        childCopyObj.GetComponent<TextMeshProUGUI>().text = "" ;
        for (int i = 0; i < textToShow.Length; i++)
        {
            bool isSpace = false;
            letters[i] = GameObject.Instantiate(childCopyObj);
            TextMeshProUGUI TMP = letters[i].GetComponent<TextMeshProUGUI>();
            TMP.text = textToShow[i].ToString();
            if (textToShow[i].ToString() == " ")
            {
                TMP.text = "-";
                isSpace = true;
            }
            letters[i].transform.parent = transform;
            letters[i].transform.localScale = childCopyObj.transform.localScale;
            Vector3 textPos = childCopyObj.transform.localPosition;
            textPos.x -= (origWidth / 2);
            //textPos.x -= preferedWidth / 2;
            float preferedWidth = TMP.preferredWidth;
            currOffset += preferedWidth / 2;
            textPos.x += currOffset;
            letters[i].transform.localPosition = textPos;
            currOffset += preferedWidth / 2;
            //if (letters[i].TryGetComponent<WaveTextEffect>(out WaveTextEffect WTE))
            //{
            //    Debug.Log("");
            //    Destroy(this);
            //}
            if(isSpace)
                TMP.text = " ";
            TMP.enabled = false;
        }
        letterAnimationCompletionData = new int[letters.Length];
        timeForSin -= initOffsetTime;
        if (GoAtStart) Go(); 
    }

    // Update is called once per frame
    float timeForSin = -9999999999;
    float loopTime = 2.0f;
    float growAmount = 0.15f;
    [SerializeField] float letterTimeOffset = 0.1f;
    [SerializeField] float initOffsetTime = 0;
    int[] letterAnimationCompletionData;
    void Update()
    {
        if(timeForSin >= -initOffsetTime)
            timeForSin += Time.deltaTime;
        for (int i = 0; i < letters.Length; i++) 
        {
            if ((timeForSin >= letterTimeOffset * i) && (letterAnimationCompletionData[i] == 0))
            {
                letters[i].GetComponent<TextMeshProUGUI>().enabled = true;
                float locTime = timeForSin;
                locTime -= letterTimeOffset * (float)i;
                locTime %= loopTime;
                locTime = locTime / loopTime;
                if (locTime > 0.75)
                {
                    locTime = 0.75f;
                    letterAnimationCompletionData[i] = 1;
                }
                locTime = MathF.Sin(locTime * 6.28318530718f);
                locTime += 1;
                locTime *= growAmount;
                locTime += originalScale;
                letters[i].transform.localScale = new Vector3(locTime, locTime, locTime);
            }
        }
    }
    public void Go()
    {
        timeForSin = -initOffsetTime;
    }
}
