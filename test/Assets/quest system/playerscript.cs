using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;

public class playerscript : MonoBehaviour
{    
    public Transform Cam;
    public Vector3 camRotation;
    public float PlayerSpeed = 5;
    public float sensitivity = 800f;
    private float minAngle = -30f;
    private float maxAngle = 75f;
    public CharacterController C;
    public Vector3 move;
    public float gravityScale = 5f;
    public int points = 0;
    public Text pointsText;
    public int HitNumber = 0;
    public Manager Manager;
    public bool CursorLocked = true;
    public int Reputation;
    public GameObject JournalBack;
    void Start()
    {
        Cam = Camera.main.transform;
        C = GetComponent<CharacterController>();        
        pointsText = GameObject.Find("Player/Canvas/Points").GetComponent<Text>();
        Manager = GameObject.Find("Manager").GetComponent<Manager>();
        JournalBack = GameObject.Find("Player/Canvas/JournalBack");
        JournalBack.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        CurMode();
        if (!DialogueManager.Dialogue.activeInHierarchy)
        {
            Movement();                        
            CamRotation();
            move.y += gravityScale * Physics.gravity.y * Time.deltaTime;            
        }
        else
        {
            move = Vector3.zero;
        }
        Dialoge();        
        CubeKiller();
        Journal();
        C.Move(move * Time.deltaTime);
    }
    public void CurMode()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && CursorLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            CursorLocked = false;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && !CursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            CursorLocked = true;
        }
    }
    public void Movement()
    {
        float Vertical = Input.GetAxisRaw("Vertical");
        float Horizontal = Input.GetAxisRaw("Horizontal");
        move = transform.forward * Vertical + transform.right * Horizontal;
        move.Normalize();
        move *= PlayerSpeed;
    }
    public void CamRotation()
    {
        transform.Rotate(Vector3.up * sensitivity * Time.deltaTime * Input.GetAxis("Mouse X"));
        camRotation.x -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        camRotation.x = Mathf.Clamp(camRotation.x, minAngle, maxAngle);
        Cam.localEulerAngles = camRotation;
    }
    public void Dialoge()
    {
        Vector3 target = GameObject.Find("Player/Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10));
        Vector3 direction = target - Cam.position;
        Ray ray = new Ray(Cam.position, direction.normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10))
        {
            if (hit.collider.gameObject.tag == "NPC" && hit.collider.gameObject.GetComponent<NPC>().enabled)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    DialogueManager.Dialogue.SetActive(true);
                    DialogueManager.ShowDialoge(hit.collider.gameObject.GetComponent<NPC>().id);
                }
            }
        }        
    }
    public void CubeKiller()
    {
        Vector3 target = GameObject.Find("Player/Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10));
        Vector3 direction = target - Cam.position;
        Ray ray = new Ray(Cam.position, direction.normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10))
        {
            if (hit.collider.gameObject.tag == "Enemy")
            {

                if (Input.GetMouseButtonDown(0))
                {
                    HitNumber += 1;                                                            
                    if (HitNumber == Manager.level)
                    {
                        Destroy(hit.collider.gameObject);
                        QuestManager.KilledEnemy(hit.collider.gameObject.GetComponent<EnemyScipt>().EnemyId);
                        points += 1;
                        pointsText.text = points.ToString();
                        HitNumber = 0;                        
                    }
                }
            }
        }
    }
    public void Journal()
    {
        if (Input.GetKeyDown(KeyCode.J) && !JournalBack.activeInHierarchy)
        {
            JournalBack.SetActive(true);            
        }
        else if(Input.GetKeyDown(KeyCode.J) && JournalBack.activeInHierarchy)
        {
            JournalBack.SetActive(false);
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "QuestSphere")
        {
            other.gameObject.SetActive(false);
        }
    }
}
    
