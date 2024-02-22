using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InventoryBarSettings : MonoBehaviour
{
    
    [SerializeField] private SettingsManager settingsManager;

    private void Start()
    {

        
        settingsManager.inventoryScaleSlider.onValueChanged.AddListener(SetinventoryPanelScale);
        settingsManager.originalInventorySize = settingsManager.inventoryPanel.localScale;
        settingsManager.originalInventoryAspect = settingsManager.inventoryPanel.localScale.x / settingsManager.inventoryPanel.localScale.y;
        settingsManager.inventoryPanelCanvasGroup = settingsManager.inventoryPanel.GetComponent<CanvasGroup>();
        if (settingsManager.inventoryPanelCanvasGroup == null)
        {
           
            settingsManager.inventoryPanelCanvasGroup = settingsManager.inventoryPanel.gameObject.AddComponent<CanvasGroup>();
        }

      
        settingsManager.inventoryPanelCanvasGroup.alpha = 0.5f;

        InventoryHotbar.instance.OnItemPickedUp += StartSizeEffect;
        InventoryHotbar.instance.OnItemSelected += StartSizeEffect;
    }

    private void OnDestroy()
    {
        InventoryHotbar.instance.OnItemPickedUp -= StartSizeEffect;
        InventoryHotbar.instance.OnItemSelected -= StartSizeEffect;
    }

    private void SetinventoryPanelScale(float inventoryScaleValue)
    {
        float newValx = inventoryScaleValue * settingsManager.originalInventoryAspect;
        float newValy = inventoryScaleValue;

        settingsManager.inventoryPanel.localScale = new Vector3(newValx, newValy, 1f);
        settingsManager.originalInventorySize = settingsManager.inventoryPanel.localScale;
    }

    

    private IEnumerator SizeEffectCoroutine()
    {
        float elapsedTimeForInvEffect = 0f;
        Vector3 targetInventorySize = settingsManager.originalInventorySize * settingsManager.effectScaleMultiplier;

        while (elapsedTimeForInvEffect < 1f)
        {
            settingsManager.inventoryPanel.localScale = Vector3.Lerp(settingsManager.originalInventorySize, targetInventorySize, elapsedTimeForInvEffect);
            settingsManager.inventoryPanelCanvasGroup.alpha = Mathf.Lerp(0.5f, 1f, elapsedTimeForInvEffect); //Increase alpha
            elapsedTimeForInvEffect += Time.deltaTime * settingsManager.increaseEffectSpeed;
            yield return null;
        }

        settingsManager.inventoryPanel.localScale = targetInventorySize;

        float timer = 0f;
        while (timer < settingsManager.effectWaitDuration)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                timer = 0f;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        elapsedTimeForInvEffect = 0f;
        while (elapsedTimeForInvEffect < 1f)
        {
            settingsManager.inventoryPanel.localScale = Vector3.Lerp(targetInventorySize, settingsManager.originalInventorySize, elapsedTimeForInvEffect);
            settingsManager.inventoryPanelCanvasGroup.alpha = Mathf.Lerp(1f, 0.5f, elapsedTimeForInvEffect); //Decrease alpha
            elapsedTimeForInvEffect += Time.deltaTime * settingsManager.decreaseEffectSpeed;
            yield return null;
        }

        settingsManager.inventoryPanel.localScale = settingsManager.originalInventorySize;
        settingsManager.inventoryPanelCanvasGroup.alpha = 0.5f; 

        settingsManager.isInventoryIncreasing = false;
    }

    private void StartSizeEffect()
    {
        if (!settingsManager.isInventoryIncreasing)
        {
            settingsManager.isInventoryIncreasing = true;
            StartCoroutine(SizeEffectCoroutine());
        }
    }
}