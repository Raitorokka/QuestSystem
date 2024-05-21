using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CubeScript : MonoBehaviour
{
    public List<Material> Materials;        
    public Transform Cam;
    public Vector3 camRotation;
    public float PlayerSpeed = 5;
    public float sensitivity = 800f;
    private float minAngle = -30f;
    private float maxAngle = 75f;
    public Text PressE;    
    public CharacterController C;
    public Vector3 move;
    public int CurrentColorNumber = 0;
    public int NumberPressing = 0;
    public int[] Order = { 1, 3, 2 };
    public bool CorrectOrder = true;
    public List<GameObject> Cubes;
    public GameObject Sphere1;
    public GameObject Sphere2;
    void Start()
    {        
        Cursor.lockState = CursorLockMode.Locked;
        Cam = Camera.main.transform;
        PressE = GameObject.Find("Player/Canvas/PressEText").GetComponent<Text>();
        C = GetComponent<CharacterController>();
        Sphere1 = GameObject.Find("Sphere (1)");
        Sphere1.SetActive(false);
        Sphere2 = GameObject.Find("Sphere (2)");
    }

    void Update()
    {
        Movement();
        C.Move(move * Time.deltaTime);
        CamRotation();
        Sphere();
        StartCoroutine(Cube());
    }
    public void Movement()
    {
        float Vertical = Input.GetAxisRaw("Vertical");
        float Horizontal = Input.GetAxisRaw("Horizontal");
        move = transform.forward * Vertical + transform.right * Horizontal;
        move.Normalize();
        move *= PlayerSpeed;
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Scene1Load")
        {
            SceneManager.LoadScene("Scene1Loader");
        }
        if(other.tag == "Sphere2")
        {
            Destroy(Sphere2);
        }
    }
    public void CamRotation()
    {
        transform.Rotate(Vector3.up * sensitivity * Time.deltaTime * Input.GetAxis("Mouse X"));
        camRotation.x -= Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        camRotation.x = Mathf.Clamp(camRotation.x, minAngle, maxAngle);
        Cam.localEulerAngles = camRotation;
    }
    public void Sphere()
    {
        Vector3 target = GameObject.Find("Player/Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10));
        Vector3 direction = target - Cam.position;
        Ray ray = new Ray(Cam.position, direction.normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10))
        {            
            if (hit.collider.gameObject.tag == "Sphere")
            {
                PressE.enabled = true;
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (CurrentColorNumber >= 7)
                    {
                        CurrentColorNumber = 0;
                    }
                    hit.collider.gameObject.GetComponent<MeshRenderer>().material = Materials[CurrentColorNumber];                    
                    CurrentColorNumber++;                    
                }
            }            
        }
        else
        {
            PressE.enabled = false;
        }
    }
    public IEnumerator Cube()
    {
        Vector3 target = GameObject.Find("Player/Main Camera").GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, 10));
        Vector3 direction = target - Cam.position;
        Ray ray = new Ray(Cam.position, direction.normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 10))
        {
            if (hit.collider.gameObject.tag == "Cube")
            {
                PressE.enabled = true;
                if (Input.GetKeyDown(KeyCode.E) && NumberPressing < 3)
                {
                    if(hit.collider.gameObject.GetComponent<CubeNumberController>().CubeNum.number == Order[NumberPressing])
                    {
                        NumberPressing++;                        
                    }
                    else
                    {
                        CorrectOrder = false;
                        foreach (GameObject C in Cubes)
                        {                                                       
                            C.GetComponent<MeshRenderer>().material = Materials[0];
                        }
                    }
                    if (NumberPressing == 3)
                    {                        
                        foreach(GameObject C in Cubes)
                        {                            
                            C.GetComponent<MeshRenderer>().material = Materials[0];                                                        
                        }
                        yield return new WaitForSeconds(2);
                        foreach (GameObject C in Cubes)
                        {                            
                            C.SetActive(false);
                        }
                        yield return new WaitForSeconds(1);
                        Sphere1.SetActive(true);
                        NumberPressing = 0;
                    }
                }
            }
        }
        else
        {
            PressE.enabled = false;
        }
    }
}
