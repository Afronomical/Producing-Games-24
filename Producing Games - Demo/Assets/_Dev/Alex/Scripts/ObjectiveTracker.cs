using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveTracker : MonoBehaviour
{
    public static ObjectiveTracker instance;

    public List<TMP_Text> trackers; //Where the text for the objectives displays

    public enum objectiveTypes
    {
        Main,
        //Side,
        Urgent
    }

    public struct Objectives
    {
        public string name;
        public int ID;
        public objectiveTypes type;
    }

    //Lists of objectives (different types)
    private List<Objectives> mainObjectives = new List<Objectives>();
    //private List<Objectives> sideObjectives = new List<Objectives>();
    private List<Objectives> urgentObjectives = new List<Objectives>();

    //Timer for removing objectives
    float timerInterval = 100;
    float timer = 0;

    private void Start()
    {
        if(instance == null)
            instance = this;

        //Testing
        AddObjective("Main objective test!", objectiveTypes.Main);
        AddObjective("Urgent objective test!", objectiveTypes.Urgent);
    }

    private void Update()
    {
        RemoveObjective(mainObj);
        //DisplayObjectives();
        timer++;
    }

   public  Objectives mainObj = new Objectives();
    //Objectives sideObj = new Objectives();
    Objectives urgentObj = new Objectives();

    //Details and type of objective needed
    public void AddObjective(string name, objectiveTypes type)
    {
        switch(type)
        {
            case objectiveTypes.Main:
                trackers[0].fontStyle = FontStyles.Normal;
                mainObj.name = name;
                mainObj.type = type;
                mainObjectives.Add(mainObj);
                mainObj.ID = mainObjectives.Count;
                break;

            //case objectiveTypes.Side:
            //    trackers[1].fontStyle = FontStyles.Normal;
            //    sideObj.name = name;
            //    sideObj.type = type;
            //    sideObjectives.Add(sideObj);
            //    sideObj.ID = sideObjectives.Count;
            //    break; 

            case objectiveTypes.Urgent:
                trackers[1].fontStyle = FontStyles.Normal;
                urgentObj.name = name;
                urgentObj.type = type;
                urgentObjectives.Add(urgentObj);
                urgentObj.ID = urgentObjectives.Count;
                break;
                
            default:
                break;
        }
        DisplayObjectives();
    }

    private void DisplayObjectives()
    {
        trackers[0].text = mainObj.name;
        //trackers[1].text = sideObj.name;
        trackers[1].text = urgentObj.name;
    }

    //Remove a specific objective
    public void RemoveObjective(Objectives objToRemove)
    {

        switch(objToRemove.type)
        {
            case objectiveTypes.Main:
                trackers[0].fontStyle = FontStyles.Strikethrough;
                trackers[0].color = Color.gray;
                //StartCoroutine(RemoveText(mainObj.name));
                if (timer >= timerInterval) mainObj.name = " ";
                break;

            //case objectiveTypes.Side:
            //    trackers[1].fontStyle = FontStyles.Strikethrough;

            //    break;

            case objectiveTypes.Urgent:
                trackers[1].fontStyle = FontStyles.Strikethrough;
                trackers[1].color = Color.gray;
                //StartCoroutine(RemoveText(urgentObj.name));
                if (timer >= timerInterval) urgentObj.name = " ";
                break;

            default:
                break;
        }
        DisplayObjectives();
    }

    //private IEnumerator RemoveText(string name)
    //{
    //    yield return new WaitForSeconds(10);
    //    name = " ";
    //}
}
