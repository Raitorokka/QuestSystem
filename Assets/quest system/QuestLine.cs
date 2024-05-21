using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.UI;

public class QuestLine : MonoBehaviour
{
    public int TargetQuestId = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(TargetQuestId == QuestManager.ActiveQuestId)
        {
            GetComponent<Image>().color = Color.yellow;
            GetComponent<Button>().enabled = false;
        }
        else
        {
            GetComponent<Image>().color = Color.white;
            GetComponent<Button>().enabled = true;
        }
    }
    public void ChangeActiveQuest()
    {
        QuestManager.ActiveQuestId = TargetQuestId;
        QuestManager.QuestMark.text =
                    QuestManager.QuestData[QuestManager.ActiveQuestId].QuestReplics[QuestManager.QuestData[QuestManager.ActiveQuestId].CurrentAction];
        QuestManager.ChangeActiveInfo();
    }
}
