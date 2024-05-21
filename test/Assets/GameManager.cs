using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public player Player;
    public int FollowersOnPoint;
    public List<NavMeshModifierVolume> Barriers;
    public NavMeshModifierVolume BattleZone;
    public bool CheckBattle = false;
    public FollowerController Warrior;
    public FollowerController Magician;
    public FollowerController Healler;    
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("doom eternal slayer").GetComponent<player>();
        Warrior = GameObject.Find("Warrior").GetComponent<FollowerController>();
        Magician = GameObject.Find("Magician").GetComponent<FollowerController>();
        Healler = GameObject.Find("Healler").GetComponent<FollowerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(FollowersOnPoint == 3)
        {
           Player.Prep = false;
        }
        if (CheckBattle)
        {
            StartCoroutine(Player.CheckBattle());
            CheckBattle = false;
        }
    }
    public IEnumerator ReachedPoint()
    {
        while (true)
        {
            if (FollowersOnPoint == 3)
            {
                foreach (NavMeshModifierVolume b in Barriers)
                {
                    b.enabled = true;
                }
                Player.MainNavmesh.BuildNavMesh();
                yield return new WaitForSeconds(7f);
                foreach (NavMeshModifierVolume b in Barriers)
                {
                    b.enabled = false;
                }
                BattleZone.enabled = false;
                Player.PlayerPoint = null;
                Warrior.DestinationPoint = null;
                Healler.DestinationPoint = null;
                Magician.DestinationPoint = null;
                FollowersOnPoint = 0;
                Player.MainNavmesh.BuildNavMesh();
                CheckBattle = true;
                yield break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
