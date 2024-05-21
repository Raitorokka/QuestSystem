using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public int i = 1;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GameObject.Find("Canvas/Text").GetComponent<Text>();
        StartCoroutine(ss());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator ss()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);            
            text.text = i.ToString();
            i += 1;
        }
    }
}
