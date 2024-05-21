using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public Dropdown difficulty;
    public int level = 1;
    public List<NPC> npc;    
    public Color talk;
    public Color ntalk;
    public Sprite QuationImage;
    public playerscript PlayerScript;
    public static List<int> ActiveQuests = new();
    public GameObject QuestLine;
    public List<GameObject> Spheres;
    public float QNInitialHeights;
    public float TotalJournalHeight;
    void Start()
    {
        QuestManager.Init();
        DialogueManager.Init();
        difficulty = GameObject.Find("Player/Canvas/Dropdown").GetComponent<Dropdown>();
        PlayerScript = GameObject.Find("Player").GetComponent<playerscript>();
        QNInitialHeights = QuestLine.transform.GetChild(1).GetComponent<Text>().preferredHeight;
        for (int i = 0; i < Spheres.Count; i++)
        {
            Spheres[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        switch (difficulty.value)
        {
            case 0:
                level = 1;
                break;
            case 1:
                level = 2;
                break;
            case 2:
                level = 3;
                break;
        }
        QuestManager.SphereSpawner();
    }
    public void NextReplic()
    {
        int SpeakQuestAction = 0;
        Quest GivenQuest = null;
        foreach (Quest Q in QuestManager.QuestData)
        {            
            if (Q.NPCId[0] == DialogueManager.NPCId)
            {
                SpeakQuestAction = Q.CurrentAction;
                GivenQuest = Q;
                break;
            }
        }        
        string[] CurrentReplic = DialogueManager.DialogeData[DialogueManager.NPCId + SpeakQuestAction];        
        if(CurrentReplic.Length > SpeakQuestAction)
        {
            DialogueManager.ReplicIndex++;
            if (CurrentReplic.Length > DialogueManager.ReplicIndex)
            {

                DialogueManager.Dialogue.transform.GetChild(0).gameObject.GetComponent<Text>().text = CurrentReplic[DialogueManager.ReplicIndex];
                if (DialogueManager.ReplicIndex == CurrentReplic.Length - 1)
                {
                    DialogueManager.Dialogue.transform.GetChild(2).gameObject.SetActive(true);
                }
            }
            else
            {
                DialogueManager.ReplicIndex = 0;
                
                foreach(NPC n in npc)
                {
                    if(n.id == DialogueManager.NPCId)
                    {
                        n.QuationMark.sprite = QuationImage;
                        n.QuationMark.color = ntalk;
                    }                
                }    
                DialogueManager.Dialogue.transform.GetChild(2).gameObject.SetActive(false);
                DialogueManager.Dialogue.SetActive(false);
                ActiveQuests.Add(GivenQuest.QuestId);
                if (ActiveQuests.Count != 0)
                {
                    QuestManager.MainQuestLine.SetActive(false);
                }
                if(ActiveQuests.Count > 0)
                {
                    QuestManager.ActiveQuestId = GivenQuest.QuestId;
                }
                for (int i = 1; i < PlayerScript.JournalBack.transform.childCount; i++ )
                {
                    TotalJournalHeight += PlayerScript.JournalBack.transform.GetChild(i).GetComponent<RectTransform>().sizeDelta.y + 30;
                }
                if(ActiveQuests.Count == 1)
                {
                    TotalJournalHeight += 90;
                }
                GameObject line = Instantiate(QuestLine, QuestManager.MainQuestLine.transform.position + 
                    new Vector3(0, -TotalJournalHeight, 0), QuestLine.transform.rotation, PlayerScript.JournalBack.transform);               
                line.transform.GetChild(1).GetComponent<Text>().text = 
                    QuestManager.QuestData[QuestManager.ActiveQuestId].QuestReplics[QuestManager.QuestData[QuestManager.ActiveQuestId].CurrentAction];
                line.transform.GetChild(0).GetComponent<Text>().text = QuestManager.QuestData[QuestManager.ActiveQuestId].QuestName;
                line.GetComponent<QuestLine>().TargetQuestId = DialogueManager.NPCId / 1000 - 1;                
                if(line.transform.GetChild(1).GetComponent<Text>().preferredHeight > QNInitialHeights)
                {
                    float h = line.transform.GetChild(1).GetComponent<Text>().preferredHeight - QNInitialHeights + 25;
                    line.GetComponent<RectTransform>().sizeDelta += new Vector2(0, h);/////////////////////////////////////////не правильное отображение квестов
                }
                QuestManager.QuestMark.text = 
                    QuestManager.QuestData[QuestManager.ActiveQuestId].QuestReplics[QuestManager.QuestData[QuestManager.ActiveQuestId].CurrentAction];
                QuestManager.ChangeActiveInfo();
            }
        }
        else
        {
            PlayerScript.Reputation += QuestManager.QuestData[QuestManager.ActiveQuestId].Rep;
            QuestManager.RepMark.text += PlayerScript.Reputation.ToString();
            foreach (NPC n in npc)
            {
                if (n.id == DialogueManager.NPCId)
                {
                    n.QuationMark.enabled = false;
                    DialogueCancel();
                    n.enabled = false;
                    QuestManager.QuestMark.text = "No Quest";
                }
            }
        }
    }
    public static void DialogueCancel()
    {
        DialogueManager.ReplicIndex = 0;
        DialogueManager.Dialogue.transform.GetChild(2).gameObject.SetActive(false);
        DialogueManager.Dialogue.SetActive(false);
    }    
}
