using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowerController : MonoBehaviour
{
    public FollowerObject Type;
    public GameObject Player;
    public NavMeshAgent Agent;
    public GameManager GameManager;
    public Transform DestinationPoint;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("doom eternal slayer");
        Agent = GetComponent<NavMeshAgent>();
        GameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {        
        if (Player.GetComponent<player>().Prep)
        {
            switch (Type.follType)
            {
                case "Warrior":
                    {                        
                        DestinationPoint = GameManager.BattleZone.transform.GetChild(2);
                        break;
                    }
                case "Healler":
                    {
                        DestinationPoint = GameManager.BattleZone.transform.GetChild(3);
                        break;
                    }
                case "Magician":
                    {
                        DestinationPoint = GameManager.BattleZone.transform.GetChild(4);
                        break;
                    }                
            }
            Agent.SetDestination(DestinationPoint.position);
            if (Agent.remainingDistance <= 1f)
            {
                GameManager.FollowersOnPoint++;
            }
        }
        else
        {
            Agent.SetDestination(Player.transform.position);
            if (Agent.remainingDistance <= Type.distance)
            {
                Agent.isStopped = true;
            }
            else
            {
                Agent.isStopped = false;
            }
        }
    }   
}
