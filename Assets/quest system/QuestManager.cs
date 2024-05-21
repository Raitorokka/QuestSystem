using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Quest
{
    public string QuestName;
    public int QuestId;
    public int[] NPCId;
    public int[] EnemyId;
    public string[] QuestReplics;
    public int Rep;
    public bool[] IsQuestAction;
    public int CurrentAction;
    public int[] QuestSteps;
    public Quest(string QuestName, int QuestId,int[] NPCId, int[] EnemyId, string[] QuestReplics, int Rep, bool[] IsQuestAction, int CurrentAction, int[] QuestSteps)
    {
        this.QuestName = QuestName;
        this.QuestId = QuestId;
        this.NPCId = NPCId;
        this.EnemyId = EnemyId;
        this.QuestReplics = QuestReplics;
        this.Rep = Rep;
        this.IsQuestAction = IsQuestAction;
        this.CurrentAction = CurrentAction;
        this.QuestSteps = QuestSteps;
    }
}

public class QuestManager
{
    public static int CurrentQuestProgress = 0;
    public static List<Quest> QuestData = new();
    public static int ActiveQuestId = -1;
    public static bool GVQuest = false;//////unknown
    public static Text QuestMark;
    public static playerscript Player;
    public static Text RepMark;
    public static Manager Manager;    
    public static GameObject MainQuestLine;    
    public static float QMInitialHeight;
    public static GameObject ActiveInfo;
    public static Vector3 AIInitialPose;
    public static Vector3 RepInitialPos;
    public static void Init()
    {
        QuestMark = GameObject.Find("Player/Canvas/ActiveInfo/QuestMark").GetComponent<Text>();
        RepMark = GameObject.Find("Player/Canvas/ActiveInfo/Reputation").GetComponent<Text>();
        QuestData.Add(new Quest("Black Cube Quest", 0, new int[]{1000}, 
            new int[]{500, 500, 500, 500, 500}, new string[] {"0/5 Cubes killed", "Speak to black cube"}, 100, new bool[] {false, true}, 0, new int[] {5, 0}));
        QuestData.Add(new Quest("White Cube Quest", 1, new int[] { 2000 }, new int[] { 501, 502, 503}, 
            new string[] { "0/1 black sphere taken\n 0/1 red sphere\n 0/1 white sphere", "Speak to white cube" }, 100, new bool[] {false, true}, 0, new int[] {5, 0}));
        Player = GameObject.Find("Player").GetComponent<playerscript>();       
        Manager = GameObject.Find("Manager").GetComponent<Manager>();
        MainQuestLine = GameObject.Find("Player/Canvas/JournalBack/MainQuestLine");
        ActiveInfo = GameObject.Find("Player/Canvas/ActiveInfo");
        AIInitialPose = ActiveInfo.transform.position;
        QMInitialHeight = QuestMark.preferredHeight;
        RepInitialPos = RepMark.transform.position;
    }    
    public static void KilledEnemy(int EnemyId)
    {        
        Quest CurrentQuest = QuestData[ActiveQuestId];
        if (CurrentQuest.NPCId[0] == DialogueManager.NPCId && CurrentQuest.EnemyId[0] == EnemyId && GVQuest == false)
        {
            CurrentQuestProgress++;
            QuestMark.text = CurrentQuestProgress.ToString() + 
                CurrentQuest.QuestReplics[CurrentQuest.CurrentAction].Remove(0, 1);
            Player.JournalBack.transform.GetChild(Manager.ActiveQuests[ActiveQuestId] + 1).GetChild(1).GetComponent<Text>().text = 
                CurrentQuestProgress.ToString() + CurrentQuest.QuestReplics[CurrentQuest.CurrentAction].Remove(0, 1);
            if (CurrentQuestProgress >= CurrentQuest.EnemyId.Length)
            {
                GVQuest = true;
                foreach (NPC n in Manager.npc)
                {
                    if (n.id == DialogueManager.NPCId)
                    {
                        n.QuationMark.color = Manager.talk;
                    }
                }
                CurrentQuestProgress = 0;
                CurrentQuest.CurrentAction++;
                QuestMark.text = CurrentQuest.QuestReplics[CurrentQuest.CurrentAction];
                Player.JournalBack.transform.GetChild(Manager.ActiveQuests[ActiveQuestId] + 1).GetChild(1).GetComponent<Text>().text =
                    CurrentQuest.QuestReplics[CurrentQuest.CurrentAction];
            }
        }
    }
    public static void SphereSpawner()
    {
        if(ActiveQuestId == 1)
        {
            for(int i = 0; i < Manager.Spheres.Count; i++)
            {
                Manager.Spheres[i].SetActive(true);
            }
        }
    }
    public static void ChangeActiveInfo()
    {
        if (QuestMark.preferredHeight > QMInitialHeight)
        {
            float h = (QuestMark.preferredHeight - QMInitialHeight) / 24;
            ActiveInfo.transform.position += new Vector3(0, 24 * h, 0);
            RepMark.transform.position += new Vector3(0, 12 * h, 0);
        }
        else
        {
            ActiveInfo.transform.position = AIInitialPose;
            RepMark.transform.position = RepInitialPos;
        }
    }
}
