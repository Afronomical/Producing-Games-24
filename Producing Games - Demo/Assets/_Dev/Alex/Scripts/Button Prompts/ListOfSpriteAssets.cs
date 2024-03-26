using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "List of Sprite Assets", menuName = "List of Sprite Assets", order = 0)]

public class ListOfSpriteAssets : ScriptableObject
{
    public List<TMP_SpriteAsset> spriteAssets;
}
