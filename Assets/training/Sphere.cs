using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour
{
    public GameObject player;
    public GameObject Sphere2;
    // Start is called before the first frame update
    void Start()
    {
        Sphere2 = GameObject.Find("Sphere (2)");
        Sphere2.SetActive(false);
        Debug.Log(Sphere2);
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Gate")
        {
            Sphere2.SetActive(true);
            Sphere2.transform.position = player.transform.forward * 2;
            Sphere2.transform.position = new Vector3(Sphere2.transform.position.x, 1, Sphere2.transform.position.z);
            Destroy(gameObject);
        }
    }
}
