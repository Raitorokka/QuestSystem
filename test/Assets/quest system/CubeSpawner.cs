using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject Cube;
    public int MaxRange = 10;
    public playerscript player;
    public bool HasQuest = false;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<playerscript>();
        StartCoroutine(Spawner());
    }

    // Update is called once per frame
    void Update()
    {
        if(QuestManager.ActiveQuestId == 0)//////////////////////не проверяет номер действия
        {
            HasQuest = true;
        }
    }
    public IEnumerator Spawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (player.points < 10 && HasQuest)
            {
                yield return new WaitForSeconds(1);
                Vector3 range = transform.position + new Vector3(Random.Range(-MaxRange, MaxRange), 0, Random.Range(-MaxRange, MaxRange));
                Instantiate(Cube, range, Quaternion.identity);
            }
        }
    }    
}
