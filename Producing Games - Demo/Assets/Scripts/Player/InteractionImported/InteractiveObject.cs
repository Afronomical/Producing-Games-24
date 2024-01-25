using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Interactive Object", menuName = "Interactive Object")]
public class InteractiveObject : ScriptableObject
{
    public string objectName;
    public GameObject obj;
    public GameObject prefab;
    public Sprite objectImage;
    //public Sound sound;

    [Header("Inventory")]
    public string description;

    [Header("Tooltip")]
    public string tooltipText;
    public Sprite kbImage, contImage, psImage;

    //[Header("Keypad")]
    //public string solution;
}
