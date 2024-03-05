using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Interactive Object", menuName = "Interactive Object")]
public class InteractiveObject : ScriptableObject
{
    public string objectName;
    public GameObject obj;
    public GameObject prefab;
    public SoundEffect interactSound;
    public Sprite objectImage;
    //public Sound sound;

    [Header("Tooltip")]
    public string tooltipText;
    public Image tooltipImage;
}
