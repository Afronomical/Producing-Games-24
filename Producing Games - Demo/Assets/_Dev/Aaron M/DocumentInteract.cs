using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class DocumentInteract : InteractableTemplate
{
    [Space(30)]
    public string title;
    [TextArea]
    public string text;

    public Sprite pageSprite;
    public SoundEffect pickupSound;

    private Image page;
    private TMP_Text titleText, textText;


    void Awake()
    {
        page = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        titleText = page.transform.GetChild(0).GetComponent<TMP_Text>();
        textText = page.transform.GetChild(1).GetComponent<TMP_Text>();
        page.sprite = pageSprite;
        titleText.text = title;
        textText.text = text;
    }

    public override void Interact()
    {
        AudioManager.instance.PlaySound(pickupSound, transform);
        DocumentViewer.instance.ShowDocument(this);
    }
}
