using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Demon Stats", menuName = "New Demon Stats",order = 1)]
public class DemonItemsSO : ScriptableObject
{
    public string demonName;

    public List<GameObject> itemsForExorcism  = new();

    public List<DemonStateBaseClass> likelyStates = new(); // Likely won't work

    public SoundEffect battleCryAudio;
    public SoundEffect defeatCryAudio;

    public GameObject demonPrefab;

}
