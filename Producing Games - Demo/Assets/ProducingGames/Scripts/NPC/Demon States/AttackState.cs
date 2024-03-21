using UnityEngine;
using static DemonCharacter;

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
        // INFO: Final check before attacking player so the demon doesn't
        // get stuck in attacking state
        if (character.playerMovement.isHiding)
        {
            character.animator.SetBool("isAttacking", false);
            character.ChangeDemonState(DemonCharacter.DemonStates.Patrol);
            return;
        }

        character.animator.SetBool("isAttacking", true);

        Invoke(nameof(CheckBeforeAttack), 1.5f);
        //Reset();
    }

    //public override void UpdateLogic()
    //{
    //    // INFO: Final check before attacking player so the demon doesn't
    //    // get stuck in attacking state
    //    if (character.playerMovement.isHiding)
    //    {
    //        character.animator.SetBool("isAttacking", false);
    //        character.ChangeDemonState(DemonCharacter.DemonStates.Patrol);
    //    }
    //}

    /// <summary>
    /// Calls the end hour function on the game manager and other logic
    /// </summary>  
    private void Reset()
    {
        // INFO: Replaced EndHour with the captured event. Captured event has EndHour within it after the scene
        GameManager.Instance.DemonCaptureEvent();

        //StartCoroutine(GameManager.Instance.EndHour());

        //this is where animation will go before moving player back to start pos
        GetComponent<Animator>().SetBool("isAttacking", false);
        GetComponent<Animator>().SetBool("isChasing", false);

        //character.player.transform.position = GameManager.Instance.playerStartPosition.position;
        //GameManager.Instance.currentTime = 60;  // End the hour
        //character.ChangeDemonState(DemonCharacter.DemonStates.Inactive);
        
    }

    private void CheckBeforeAttack()
    {
        // INFO: Final check before attacking player so the demon doesn't
        // get stuck in attacking state
        if (character.playerMovement.isHiding)
        {
            character.animator.SetBool("isAttacking", false);
            character.ChangeDemonState(DemonStates.Patrol);
            return;
        }

        // INFO: If the player is outside of the demons attack radius when the demon slashes at them then
        // the player won't be killed and the demon will go into the chase state.
        if (Vector3.Distance(character.transform.position, GameManager.Instance.player.transform.position) > character.attackRadius)
        {
            character.ChangeDemonState(DemonStates.Chase);
        }
        else
        {
            Debug.Log("Triggered event");
            GameManager.Instance.DemonCaptureEvent();
        }

        character.animator.SetBool("isAttacking", false);
    }

}   
