using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sss : MonoBehaviour
{    
    public bool IsPaused = false;
    public GameObject Pausse;
    public GameObject Mussic;
    // Start is called before the first frame update
    void Start()
    {
        Mussic = GameObject.Find("music");
        Pausse = GameObject.Find("GameObject");
        Pausse.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        pause();
        if(IsPaused) 
        {
            Mussic.GetComponent<AudioSource>().Pause();
        }
        else
        {
            Debug.Log(1);
            Mussic.GetComponent<AudioSource>().UnPause();
        }
    }
    public void pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused == false)
            {
                Pausse.SetActive(true);
                IsPaused = true;
            }
            else if (IsPaused == true)
            {
                Pausse.SetActive(false);
                IsPaused = false;
            }
        }
        
    }
    
}
