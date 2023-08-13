using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GravityEffect : MonoBehaviour
{
    private SpriteRenderer surroundSprite;
    private bool notSimulation = true;
    void Start()
    {
        if (gameObject.scene.name == "Simulation")
        {
            Destroy(this);
            return;
        }
        else
        {
            offset = effectRepeatTime / effectAmount;
            surroundSprite = transform.GetComponent<SpriteRenderer>();
            CreateGravEffect();
        }
    }
    private int effectAmount = 6;
    private SpriteRenderer[] effectSprites;
    private GameObject[] effects;
    private float effectRepeatTime = 5.0f;
    private float offset;
    void CreateGravEffect()
    {
        effects = new GameObject[effectAmount];
        effectSprites = new SpriteRenderer[effectAmount];
        for (int i = 0; i < effectAmount; i++)
        {
            effects[i] = new GameObject("GravEffect" + i.ToString());
            SceneManager.MoveGameObjectToScene(effects[i], gameObject.scene);
            effects[i].transform.parent = transform;
            effects[i].transform.position = transform.position;
            effects[i].transform.localScale = Vector3.one;
            effects[i].transform.rotation = transform.rotation;
            SpriteRenderer spr = effects[i].AddComponent<SpriteRenderer>();
            spr.maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
            effectSprites[i] = spr;
            spr.sprite = surroundSprite.sprite;
            spr.sortingOrder = -1;
            spr.color = new Color(1,1,1, 0.2f);

            GameObject child = GameObject.Instantiate(effects[i]);
            child.transform.parent = effects[i].transform;
            child.transform.position = effects[i].transform.position;
            child.transform.rotation = effects[i].transform.rotation;
            child.transform.localScale = new Vector2(0.75f, 0.75f);
            SpriteRenderer chSr = child.GetComponent<SpriteRenderer>();
            chSr.color = Color.clear;
            SpriteMask cSm = child.AddComponent<SpriteMask>();
            cSm.sprite = effectSprites[i].sprite;

            UnityEngine.Rendering.SortingGroup sog = effects[i].AddComponent<UnityEngine.Rendering.SortingGroup>();
            sog.sortAtRoot = true;
        }
    }
    private float currTime = 0.0f;
    void Update()
    {
        for (int i = 0; i < effectAmount; i++)
        {
            float locTime = currTime + (i * offset);
            locTime %= effectRepeatTime;
            locTime /= effectRepeatTime;
            effectSprites[i].color = new Color(1, 1, 1, locTime / 4);
            locTime = 1 - locTime;
            effects[i].transform.localScale = new Vector2(locTime, locTime);
        }
        currTime += Time.deltaTime;
        currTime %= effectRepeatTime;
    }
}
