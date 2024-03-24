using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceTrigger : MonoBehaviour
{
    public SoundEffect AmbienceMusic;
    private AmbienceMain ambienceMain;
    
    // Start is called before the first frame update
    void Start()
    {
        ambienceMain = transform.parent.gameObject.GetComponent<AmbienceMain>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ambienceMain.NewAmbiencePlay(AmbienceMusic);
    }
}
