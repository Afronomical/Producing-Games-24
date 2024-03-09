/// <summary>
/// Written By: Matt Brake
/// Moderated By: ...
/// 
/// <para> The Behaviour of the demon once its caught the player </para>
/// </summary>

public class AttackState : DemonStateBaseClass
{
    private void Start()
    {
        character.animator.SetBool("isAttacking", true);
        Reset();
    }

    /// <summary>
    /// Calls the end hour function on the game manager and other logic
    /// </summary>
    private void Reset()
    {
        // INFO: Replaced EndHour with the captured event. Captured event has EndHour within it after the scene
        GameManager.Instance.DemonCaptureEvent();

        /*
        StartCoroutine(GameManager.Instance.EndHour());

        //this is where animation will go before moving player back to start pos
        GetComponent<Animator>().SetBool("isAttacking", false);
        GetComponent<Animator>().SetBool("isChasing", false);

        character.player.transform.position = GameManager.Instance.playerStartPosition.position;
        GameManager.Instance.currentTime = 60;  // End the hour
        character.ChangeDemonState(DemonCharacter.DemonStates.Inactive);
        */
    }
}   
