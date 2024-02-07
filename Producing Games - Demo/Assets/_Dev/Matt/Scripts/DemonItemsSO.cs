using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Demon Stats", menuName = "New Scriptable Object/New Demon Stats",order = 1)]
public class DemonItemsSO : ScriptableObject
{
    public string DemonName;

    public List<GameObject> ItemsForExorcism = new();

    public List<StateBaseClass> LikelyStates = new(); 

    public string BattleCry;
}
