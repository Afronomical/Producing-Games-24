using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestClick : MonoBehaviour, ISaveable
{
    private int mouseClicks = 0;

    public int GetMouseClicks() => mouseClicks;

    public void LoadData(GameData data)
    {
        mouseClicks = data.clicksCompleted;
    }

    public void SaveData(GameData data)
    {
        data.clicksCompleted = mouseClicks;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseClicks++;
        }
    }
}
