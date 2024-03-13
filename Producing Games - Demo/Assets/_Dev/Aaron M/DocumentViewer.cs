using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DocumentViewer : MonoBehaviour
{
    public static DocumentViewer instance;

    public Image page;
    public TMP_Text title, text;
    private Image panel;
    private bool active;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        panel = GetComponent<Image>();
    }

    private void Update()
    {
        if (active)
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                active = false;
                page.gameObject.SetActive(false);
                panel.enabled = false;
                text.text = "";
                title.text = "";
                GameManager.Instance.player.GetComponent<PlayerInput>().enabled = true;
            }
        }
    }

    public void ShowDocument(DocumentInteract doc)
    {
        page.sprite = doc.pageSprite;
        title.text = doc.title;
        text.text = doc.text;
        panel.enabled = true;
        page.gameObject.SetActive(true);
        GameManager.Instance.player.GetComponent<PlayerInput>().enabled = false;
        active = true;
    }
}
