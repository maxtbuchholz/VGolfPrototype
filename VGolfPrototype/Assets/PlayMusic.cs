using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class PlayMusic : MonoBehaviour
{
    [SerializeField] AudioSource aSource;
    int testNum = 0;
    //public static PlayMusic Instance { get; private set; }
    // Start is called before the first frame update
    private string[] prevLoadedScenes = { };
    string musicToPlay = "";
    [SerializeField] AudioClip MenuMusic;
    [SerializeField] AudioClip GameMusic;
    [SerializeField] AudioClip VictMusic;
    private void Awake()
    {
        GameObject[] musicSources = GameObject.FindGameObjectsWithTag("PlayMusic");
        if (musicSources.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public void FixedUpdate()
    {
        aSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1);
        int countLoaded = SceneManager.sceneCount;
        string[] loadedScenes = new string[countLoaded];

        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = SceneManager.GetSceneAt(i).name;
        }
        if(!Enumerable.SequenceEqual(prevLoadedScenes, loadedScenes))
        {
            prevLoadedScenes = loadedScenes;
            testNum++;
            //if(loadedScenes.Contains<string>("Victory"))
            //{
            //    musicToPlay = "spooky";
            //    if (aSource.clip != VictMusic)
            //    {
            //        aSource.clip = VictMusic;
            //        aSource.Play();
            //    }
            //}
            if (loadedScenes.Contains<string>("BallGame") || loadedScenes.Contains<string>("Victory"))
            {
                musicToPlay = "sohng";
                if (aSource.clip != GameMusic)
                {
                    aSource.clip = GameMusic;
                    aSource.Play();
                }
            }
            else
            {
                musicToPlay = "menu";
                if(aSource.clip != MenuMusic)
                {
                    aSource.clip = MenuMusic;
                    aSource.Play();
                }
            }
        }
    }
    void Update()
    {
        Debug.Log(testNum + " " + musicToPlay);
    }
}
