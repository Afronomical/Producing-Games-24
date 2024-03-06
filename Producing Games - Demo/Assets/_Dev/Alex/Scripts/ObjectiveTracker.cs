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
        Side,
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
    private List<Objectives> sideObjectives = new List<Objectives>();
    private List<Objectives> urgentObjectives = new List<Objectives>();


    private void Start()
    {
        if(instance == null)
            instance = this;
    }

    Objectives mainObj = new Objectives();
    Objectives sideObj = new Objectives();
    Objectives urgentObj = new Objectives();

    public void AddObjective(string name, objectiveTypes type)
    {
        switch(type)
        {
            case objectiveTypes.Main:
                //trackers[0].text = name;
                mainObj.name = name;
                mainObj.type = type;
                mainObjectives.Add(mainObj);
                mainObj.ID = mainObjectives.Count;
                break;

            case objectiveTypes.Side:
                //trackers[1].text = name;
                sideObj.name = name;
                sideObj.type = type;
                sideObjectives.Add(sideObj);
                sideObj.ID = sideObjectives.Count;
                break; 
            
            case objectiveTypes.Urgent:
                //trackers[2].text = name;
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

    //Displays all objectives
    private void DisplayObjectives()
    {
        trackers[0].fontStyle = FontStyles.Normal;
        trackers[0].text = mainObj.name;
        trackers[1].fontStyle = FontStyles.Normal;
        trackers[1].text = sideObj.name;
        trackers[2].fontStyle = FontStyles.Normal;
        trackers[2].text = urgentObj.name;
    }

    //Remove a specific objective
    public void RemoveObjective(Objectives objToRemove)
    {
        switch(objToRemove.type)
        {
            case objectiveTypes.Main:
                trackers[0].fontStyle = FontStyles.Strikethrough;
                
                break;

            case objectiveTypes.Side:
                trackers[1].fontStyle = FontStyles.Strikethrough;

                break;

            case objectiveTypes.Urgent:
                trackers[2].fontStyle = FontStyles.Strikethrough;

                break;

            default:
                break;
        }
    }
}
