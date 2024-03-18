using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVoiceController : MonoBehaviour
{
    public static PlayerVoiceController instance;

    [Space(20)] public Dialogue medicineDialogue = new Dialogue();
    [Space(20)] public Dialogue checkCameraDialogue = new Dialogue();
    [Space(20)] public Dialogue escortingDialogue = new Dialogue();
    [Space(20)] public Dialogue checklistDialogue = new Dialogue();
    [Space(20)] public Dialogue exorcismItemDialogue = new Dialogue();
    [Space(20)] public Dialogue horrorEventDialogue = new Dialogue();
    [Space(20)] public Dialogue leaveStudyDialogue = new Dialogue();
    [Space(20)] public Dialogue leaveTutorialDialogue = new Dialogue();



    public void PlayDialogue(Dialogue dialogue)
    {
        int rand = Random.Range(0, 100);
        if (rand <= dialogue.dialoguePercentageChance)
        {
            rand = Random.Range(0, dialogue.dialogue.Length);
            if (dialogue.dialogue[rand] == null)
            {
                AudioManager.instance.PlaySound(dialogue.dialogue[rand], transform);
            }
        }
    }


    [System.Serializable]
    public struct Dialogue
    {
        [Range(0, 100)] public int dialoguePercentageChance;
        public SoundEffect[] dialogue;

        public Dialogue(int _dialoguePercentageChance, SoundEffect[] _dialogue)
        {
            dialoguePercentageChance = _dialoguePercentageChance;
            dialogue = _dialogue;
        }
    }
}

