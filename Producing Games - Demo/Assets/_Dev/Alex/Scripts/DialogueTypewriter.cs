using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueTypewriter : MonoBehaviour
{
    public DialogueTypewriter instance;
    public TMP_Text dialogueBox;

    private void Start()
    {
        if(instance == null)
            instance = this;

        dialogueBox.enabled = false;
    }

    public void Dialogue(SoundEffect voiceLine)
    {
        dialogueBox.enabled = true;
        dialogueBox.text = voiceLine.sentence;
        dialogueBox.text = "";

        StartCoroutine(WriteText(voiceLine));
    }

    private IEnumerator WriteText(SoundEffect vA)
    {
        foreach (char c in vA.sentence)
        {
            dialogueBox.text += c;
            yield return new WaitForSeconds(vA.typingSpeed);
        }

        StartCoroutine(Delay(vA));
    }

    private IEnumerator Delay(SoundEffect vA)
    {
        yield return new WaitForSeconds(vA.waitTillHide);
        dialogueBox.enabled = false;
        //Destroy(gameObject);
    }
}
