using Steamworks;
using UnityEngine;
/// <summary>
/// Written By: Matt Brake
/// <para>Moderated By: Matej Cincibus </para>
/// <para> The Demon state once a completed exorcism has taken place </para>
/// </summary>

public class ExorcisedState : DemonStateBaseClass
{
    

    private void Start()
    {
        // maybe scream sound effect of some kind

        //bool to change animation to death
        GetComponent<Animator>().SetBool("isExorcised", true);
        GetComponent<Animator>().SetBool("isChasing", false);

        //SteamUserStats.SetAchievement("ACH_WIN_100_GAMES");

        


        //invoke the destruction of the character
        Invoke(nameof(Exorcise), 4);
    }

    /*public override void UpdateLogic()
    {
    }*/

    void Exorcise()
    {
        gameObject.SetActive(false); //de-activates the Demon 
    }

    
}
