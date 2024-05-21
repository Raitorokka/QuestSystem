using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager
{
    public static Dictionary<int, string[]> DialogeData = new();
    public static GameObject Dialogue;
    public static int ReplicIndex = 0;
    public static int NPCId;    
    public static void Init()
    {        
        DialogeData.Add(1000, new string[] { "Replic1", "Replic2", "Replic3", "Replic4", "Replic5" });
        DialogeData.Add(1001, new string[] { "You didnt complete the quest"});
        DialogeData.Add(1002, new string[] { "You have completed the quest" });
        DialogeData.Add(2000, new string[] { "1", "2", "3", "4", "5" });
        DialogeData.Add(2001, new string[] { "You didnt complete the quest!" });
        DialogeData.Add(2002, new string[] { "You have completed the quest!" });
        Dialogue = GameObject.Find("Player/Canvas/Dialogue");
        Dialogue.SetActive(false);
        Dialogue.transform.GetChild(2).gameObject.SetActive(false);
    }
    public static void ShowDialoge(int id)
    {        
        if (DialogeData[id] != null)
        {
            NPCId = id;
            string[] CurrentReplic = DialogeData[id];
            Dialogue.transform.GetChild(0).gameObject.GetComponent<Text>().text = CurrentReplic[ReplicIndex];
        }
    }    
}
