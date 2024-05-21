using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Folloewr", menuName = "Create follower")]
public class FollowerObject : ScriptableObject
{
    public float distance;
    public string follType;
    public KeyCode KillKey;    
}
