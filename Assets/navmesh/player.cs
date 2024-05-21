using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    public NavMeshAgent Agent;
    public Camera MainCamera;    
    public NavMeshSurface MainNavmesh;
    public Transform PlayerPoint;
    public bool Prep = false;
    public Transform Point1;
    public Transform Point2;
    public int PlayerHP = 100;
    public Text PlayerHPText;
    public GameManager GM;
    public LayerMask Ground;
    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        MainCamera = Camera.main;
        Agent = GetComponent<NavMeshAgent>();        
        MainNavmesh = GameObject.Find("navmesh").GetComponent<NavMeshSurface>();         
        Point1 = GameObject.Find("doom eternal slayer/Point1").transform;
        Point2 = GameObject.Find("doom eternal slayer/Point2").transform;
        PlayerHPText = GameObject.Find("doom eternal slayer/Canvas/PlayerHP").GetComponent<Text>();
        StartCoroutine(CheckBattle());
        StartCoroutine(CheckHealDamage());  
    }
    
    void Update()
    {
        PlayerHPText.text = PlayerHP.ToString();
        if(Input.GetMouseButtonDown(0) && !Prep)
        {
            RaycastHit hit;
            if(Physics.Raycast(MainCamera.ScreenPointToRay(Input.mousePosition), out hit))
            {
                Agent.SetDestination(hit.point);
            }
        }
    }    
    
    public IEnumerator CheckBattle()
    {        
        while (true)
        {
            NavMeshHit nhit;
            if (NavMesh.SamplePosition(Point1.position, out nhit, 100f, NavMesh.AllAreas) || NavMesh.SamplePosition(Point2.position, out nhit, 100f, NavMesh.AllAreas))
            {
                if (nhit.mask == 1 << NavMesh.GetAreaFromName("BattleZone"))
                {
                    Prep = true;
                    Collider[] colls = Physics.OverlapSphere(transform.position, 1.5f, Ground);
                    GM.BattleZone = colls[0].gameObject.GetComponent<NavMeshModifierVolume>();
                    Debug.Log(colls[0].gameObject.name);
                    for(int i = 0; i < colls[0].gameObject.transform.GetChild(0).childCount; i++)
                    {
                        GM.Barriers.Add(colls[0].gameObject.transform.GetChild(0).GetChild(i).GetComponent<NavMeshModifierVolume>()); 
                    }
                    PlayerPoint = colls[0].gameObject.transform.GetChild(1);
                    Agent.SetDestination(PlayerPoint.position);
                    StartCoroutine(GM.ReachedPoint());
                    yield break;
                }
                yield return new WaitForSeconds(0.5f);
            }
        }
    }
    public IEnumerator CheckHealDamage()
    {
        while (true)
        {
            NavMeshHit nhit;
            if (NavMesh.SamplePosition(Point1.position, out nhit, 100f, NavMesh.AllAreas) || NavMesh.SamplePosition(Point2.position, out nhit, 100f, NavMesh.AllAreas))
            {
                if (nhit.mask == 1 << NavMesh.GetAreaFromName("DamageZone"))
                {
                    PlayerHP -= 5;
                }
                if (nhit.mask == 1 << NavMesh.GetAreaFromName("HealZone"))
                {
                    PlayerHP += 5;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
}
