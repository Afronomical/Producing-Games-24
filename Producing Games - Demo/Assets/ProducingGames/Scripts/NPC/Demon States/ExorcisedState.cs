using UnityEngine;

/// <summary>
/// Written By: Matt Brake
/// <para>Moderated By: Matej Cincibus </para>
/// 
/// <para> The Demon state once a completed exorcism has taken place </para>
/// </summary>

public class ExorcisedState : DemonStateBaseClass
{
    private void Start()
    {
        // PLAY EXORCISED SFX HERE

        // INFO: Bool to change animation to death
        character.animator.SetBool("isExorcised", true);

        //SteamUserStats.SetAchievement("ACH_WIN_100_GAMES");

        Invoke(nameof(Exorcise), 4);
    }

    void Exorcise()
    {
        gameObject.SetActive(false);
    }
}
