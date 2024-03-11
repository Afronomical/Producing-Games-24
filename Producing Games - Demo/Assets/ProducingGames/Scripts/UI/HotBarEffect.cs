using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HotBarEffect : MonoBehaviour
{
    
    [SerializeField] private AccessibilitySettingsManager AccessibilitySettings;

    private void Start()
    {

        
        AccessibilitySettings.inventoryScaleSlider.onValueChanged.AddListener(SetinventoryPanelScale);
        AccessibilitySettings.originalInventorySize = AccessibilitySettings.inventoryPanel.localScale;
        AccessibilitySettings.originalInventoryAspect = AccessibilitySettings.inventoryPanel.localScale.x / AccessibilitySettings.inventoryPanel.localScale.y;
        AccessibilitySettings.inventoryPanelCanvasGroup = AccessibilitySettings.inventoryPanel.GetComponent<CanvasGroup>();
        if (AccessibilitySettings.inventoryPanelCanvasGroup == null)
        {
           
            AccessibilitySettings.inventoryPanelCanvasGroup = AccessibilitySettings.inventoryPanel.gameObject.AddComponent<CanvasGroup>();
        }

      
        AccessibilitySettings.inventoryPanelCanvasGroup.alpha = 0.0f;

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
        float newValx = inventoryScaleValue * AccessibilitySettings.originalInventoryAspect;
        float newValy = inventoryScaleValue;

        AccessibilitySettings.inventoryPanel.localScale = new Vector3(newValx, newValy, 1f);
        AccessibilitySettings.originalInventorySize = AccessibilitySettings.inventoryPanel.localScale;
    }

    

    private IEnumerator SizeEffectCoroutine()
    {
        float elapsedTimeForInvEffect = 0f;
        Vector3 targetInventorySize = AccessibilitySettings.originalInventorySize * AccessibilitySettings.effectScaleMultiplier;

        while (elapsedTimeForInvEffect < 1f)
        {
            AccessibilitySettings.inventoryPanel.localScale = Vector3.Lerp(AccessibilitySettings.originalInventorySize, targetInventorySize, elapsedTimeForInvEffect);
            AccessibilitySettings.inventoryPanelCanvasGroup.alpha = Mathf.Lerp(0.0f, 1f, elapsedTimeForInvEffect); //Increase alpha
            elapsedTimeForInvEffect += Time.deltaTime * AccessibilitySettings.increaseEffectSpeed;
            yield return null;
        }

        AccessibilitySettings.inventoryPanel.localScale = targetInventorySize;

        float timer = 0f;
        while (timer < AccessibilitySettings.effectWaitDuration)
        {
            if (Input.mouseScrollDelta.y != 0)
            {
                if (AccessibilitySettings.demonBook.activeSelf == true || AccessibilitySettings.checkList.activeSelf == true )
                {
                    yield return null;
                }

                else
                {

                     timer = 0f;

                }
                
            }

            timer += Time.deltaTime;
            yield return null;
        }

        elapsedTimeForInvEffect = 0f;
        while (elapsedTimeForInvEffect < 1f)
        {
            AccessibilitySettings.inventoryPanel.localScale = Vector3.Lerp(targetInventorySize, AccessibilitySettings.originalInventorySize, elapsedTimeForInvEffect);
            AccessibilitySettings.inventoryPanelCanvasGroup.alpha = Mathf.Lerp(1f, 0.0f, elapsedTimeForInvEffect); //Decrease alpha
            elapsedTimeForInvEffect += Time.deltaTime * AccessibilitySettings.decreaseEffectSpeed;
            yield return null;
        }

        AccessibilitySettings.inventoryPanel.localScale = AccessibilitySettings.originalInventorySize;
        AccessibilitySettings.inventoryPanelCanvasGroup.alpha = 0.0f; 

        AccessibilitySettings.isInventoryIncreasing = false;
    }

    private void StartSizeEffect()
    {
        if (!AccessibilitySettings.isInventoryIncreasing)
        {
            if (AccessibilitySettings.demonBook.activeSelf == true || AccessibilitySettings.checkList.activeSelf == true)
            {
                return;

            }
            else
            {
                AccessibilitySettings.isInventoryIncreasing = true;
                StartCoroutine(SizeEffectCoroutine());

            }
          
        }
    }
}