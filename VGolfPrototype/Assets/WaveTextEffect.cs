using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class WaveTextEffect : MonoBehaviour
{
    [SerializeField] GameObject childCopyObj;
    private string textToShow = "Nice!";
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
            letters[i] = GameObject.Instantiate(childCopyObj);
            TextMeshProUGUI TMP = letters[i].GetComponent<TextMeshProUGUI>();
            TMP.text = textToShow[i].ToString();
            letters[i].transform.parent = transform;
            letters[i].transform.localScale = childCopyObj.transform.localScale;
            Vector3 textPos = childCopyObj.transform.localPosition;
            textPos.x -= (origWidth / 2);
            //textPos.x -= TMP.preferredWidth / 2;
            currOffset += TMP.preferredWidth / 2;
            textPos.x += currOffset;
            letters[i].transform.localPosition = textPos;
            currOffset += TMP.preferredWidth / 2;
            //if (letters[i].TryGetComponent<WaveTextEffect>(out WaveTextEffect WTE))
            //{
            //    Debug.Log("");
            //    Destroy(this);
            //}
            TMP.enabled = false;
        }
    }

    // Update is called once per frame
    float timeForSin = 0;
    float loopTime = 2.0f;
    float growAmount = 0.5f;
    float letterTimeOffset = 0.2f;
    void Update()
    {
        timeForSin += Time.deltaTime;
        for (int i = 0; i < letters.Length; i++) 
        {
            float locTime = timeForSin;
            locTime -= letterTimeOffset * (float)i;
            locTime %= loopTime;
            locTime = locTime / loopTime;
            if (locTime > 0.75) locTime = 0.75f;
            else letters[i].GetComponent<TextMeshProUGUI>().enabled = true;
            locTime = MathF.Sin(locTime * 6.28318530718f);
            locTime += 1;
            locTime *= growAmount;
            locTime += originalScale;
            letters[i].transform.localScale = new Vector3(locTime, locTime, locTime);
        }
    }
}
