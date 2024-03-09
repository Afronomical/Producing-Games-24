using UnityEngine;

/// <summary>
/// Written By: Matej Cincibus
/// Moderated By: ...
/// 
/// When the rage mode isn't happening and the demon has been instantiated
/// </summary>

public class InactiveState : DemonStateBaseClass
{
    private void Awake()
    {
        gameObject.SetActive(false);

        Debug.LogWarning("Demon is inactive.");
    }
}
