using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HotBarEffect : MonoBehaviour
{
    public RectTransform panel; 
    public float increaseSpeed = 1f;  
    public float decreaseSpeed = 1f;  
    public float waitDuration = 5f;
    public Slider scaleSlider;

    private Vector3 originalSize;
    private bool isIncreasing = false;
    private CanvasGroup panelCanvasGroup;
    private float originalAspect;

    private void Start()
    {
        scaleSlider.onValueChanged.AddListener(SetPanelScale);
        originalSize = panel.localScale;
        originalAspect = panel.localScale.x / panel.localScale.y;
        panelCanvasGroup = panel.GetComponent<CanvasGroup>();
        if (panelCanvasGroup == null)
        {
           
            panelCanvasGroup = panel.gameObject.AddComponent<CanvasGroup>();
        }

      
        panelCanvasGroup.alpha = 0.5f;

        InventoryHotbar.instance.OnItemPickedUp += StartSizeEffect;
        InventoryHotbar.instance.OnItemSelected += StartSizeEffect;
    }

    private void OnDestroy()
    {
        InventoryHotbar.instance.OnItemPickedUp -= StartSizeEffect;
        InventoryHotbar.instance.OnItemSelected -= StartSizeEffect;
    }

    private void SetPanelScale(float scaleValue)
    {
        float newValx = scaleValue * originalAspect;
        float newValy = scaleValue;

        panel.localScale = new Vector3(newValx, newValy, 1f);
        originalSize = panel.localScale;
    }

    

    private IEnumerator SizeEffectCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 targetSize = originalSize * 1.2f;

        while (elapsedTime < 1f)
        {
            panel.localScale = Vector3.Lerp(originalSize, targetSize, elapsedTime);
            panelCanvasGroup.alpha = Mathf.Lerp(0.5f, 1f, elapsedTime); //Increase alpha
            elapsedTime += Time.deltaTime * increaseSpeed;
            yield return null;
        }

        panel.localScale = targetSize;

        float timer = 0f;
        while (timer < waitDuration)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                timer = 0f;
            }

            timer += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        while (elapsedTime < 1f)
        {
            panel.localScale = Vector3.Lerp(targetSize, originalSize, elapsedTime);
            panelCanvasGroup.alpha = Mathf.Lerp(1f, 0.5f, elapsedTime); //Decrease alpha
            elapsedTime += Time.deltaTime * decreaseSpeed;
            yield return null;
        }

        panel.localScale = originalSize;
        panelCanvasGroup.alpha = 0.5f; 

        isIncreasing = false;
    }

    private void StartSizeEffect()
    {
        if (!isIncreasing)
        {
            isIncreasing = true;
            StartCoroutine(SizeEffectCoroutine());
        }
    }
}