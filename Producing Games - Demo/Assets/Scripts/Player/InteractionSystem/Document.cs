using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Document", menuName = "Interactive Document")]
public class Document : ScriptableObject
{
    public string docName;
    public Sprite[] pages;
    public string[] plainText;
    public bool canBeTaken;
    //public Sound sound;

    [Header("Tooltip")]
    public string tooltipText;
    public Sprite kbImage, contImage, psImage;
}
