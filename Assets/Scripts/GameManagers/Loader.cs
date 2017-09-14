using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
*   Loads scenes through load function
*/
public class Loader : MonoBehaviour {
    public Image loadingScreen;
    public Text percent;
    public float transitionRate = 0.1f;
    public GameObject menu;
    
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Load(string sceneName)
    {
        StartCoroutine(StartLoad(sceneName));
    }

    IEnumerator StartLoad(string sceneName)
    {
        //Transitions loading screen in
        bool complete = false;
        float t = 0;
        Color screenColor = loadingScreen.color;
        Color textColor = percent.color;
        while(t <= 1)
        {
            loadingScreen.color = new Color(screenColor.r, screenColor.g, screenColor.b, t);
            percent.color = new Color(textColor.r, textColor.g, textColor.b, t);
            t += transitionRate;
            yield return new WaitForEndOfFrame();
        }
        //Start actually loading
        AsyncOperation loading = SceneManager.LoadSceneAsync(sceneName);
        while (!loading.isDone)
        {
            percent.text = (int)(loading.progress * 100f) + "%";
            yield return null;
        }
        t = 1;
        Destroy(menu);
        while (t>=0 && loading.isDone)
        {
            loadingScreen.color = new Color(screenColor.r, screenColor.g, screenColor.b, t);
            percent.color = new Color(textColor.r, textColor.g, textColor.b, t);
            t -= transitionRate;
            yield return new WaitForEndOfFrame();
            if (t <= 0)
                complete = true;
        }
        if (complete)
            Destroy(gameObject);
    }
}
