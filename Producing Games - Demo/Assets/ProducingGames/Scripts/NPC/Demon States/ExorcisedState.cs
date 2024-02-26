using Steamworks;
using UnityEngine;
/// <summary>
/// Written By: Matt Brake
/// <para>Moderated By: Matej Cincibus </para>
/// <para> The Demon state once a completed exorcism has taken place </para>
/// </summary>

public class ExorcisedState : DemonStateBaseClass
{
    protected Callback<UserAchievementStored_t> m_UserAchievementStored;

    private void Start()
    {
        // maybe scream sound effect of some kind

        //bool to change animation to death
        GetComponent<Animator>().SetBool("isExorcised", true);
        GetComponent<Animator>().SetBool("isChasing", false);

        //SteamUserStats.SetAchievement("ACH_WIN_100_GAMES");

        //steam achievement for banishing demon
        if (SteamManager.Initialized)
        {


            SteamUserStats.GetAchievement("ACH_WIN_100_GAMES", out bool completed);

            if (!completed)
            {
                m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);

                SteamUserStats.SetAchievement("ACH_WIN_100_GAMES");
                SteamUserStats.StoreStats();
            }
        }


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

    void OnAchievementStored(UserAchievementStored_t pCallback)
    {

    }
}
