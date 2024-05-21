using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadingScript : MonoBehaviour
{
    public Image LoadingBar;
    // Start is called before the first frame update
    void Start()
    {
        LoadingBar = GameObject.Find("Canvas/Image").GetComponent<Image>();
        StartCoroutine(LoadingScene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator LoadingScene()
    {        
        yield return new WaitForSeconds(1f);
        AsyncOperation A = SceneManager.LoadSceneAsync("Scene2Loader");
        A.allowSceneActivation = false;
        float time = 0;
        while (!A.isDone)
        {
            yield return null;
            time += Time.deltaTime;
            if(A.progress >= 0.9f)
            {
                LoadingBar.fillAmount = Mathf.Lerp(LoadingBar.fillAmount, 1.0f, time);
                if(LoadingBar.fillAmount == 1.0f)
                {
                    A.allowSceneActivation = true;
                }
            }
            else
            {
                LoadingBar.fillAmount = Mathf.Lerp(LoadingBar.fillAmount, A.progress, time);
                if(LoadingBar.fillAmount >= A.progress)
                {
                    time = 0;
                }
            }
        }
    }
}
