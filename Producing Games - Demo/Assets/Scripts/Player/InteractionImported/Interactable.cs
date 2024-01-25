using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public InteractiveObject thisObject;
    public Document thisDocument;

    public virtual void Interact() { }
}
