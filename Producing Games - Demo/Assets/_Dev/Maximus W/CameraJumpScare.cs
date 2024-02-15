using UnityEngine;

public class CameraJumpScare : MonoBehaviour
{
    public GameObject darkFigurePrefab;
    public GameObject flyByPrefab;
    public Camera mainCamera;
    public float jumpScareChance = 0.1f;
    public float flyByChance = 0.05f;
    public float jumpScareDuration = 2f;
    public float maxDistanceFromCamera = 10f;

    private GameObject darkFigureInstance;
    private GameObject flyByInstance;
    private Vector3 originalPosition;
    private bool isJumpScareActive = false;
    private bool isFlyByActive = false;
    private float jumpScareTimer = 0f;
    private float flyByTimer = 0f;

    private void Start()
    {
        originalPosition = transform.position;
    }

    private void Update()
    {
        if (!isJumpScareActive && IsLookingAtCamera() && Random.value < jumpScareChance)
        {
            TriggerJumpScare();
        }

        if (!isFlyByActive && IsLookingAtCamera() && Random.value < flyByChance)
        {
            TriggerFlyBy();
        }

        if (isJumpScareActive)
        {
            jumpScareTimer += Time.deltaTime;
            if (jumpScareTimer >= jumpScareDuration)
            {
                ResetJumpScare();
            }
        }

        if (isFlyByActive)
        {
            flyByTimer += Time.deltaTime;
            if (flyByTimer >= jumpScareDuration)
            {
                ResetFlyBy();
            }
        }
    }

    private bool IsLookingAtCamera()
    {
        Vector3 cameraDirection = mainCamera.transform.position - transform.position;
        float angle = Vector3.Angle(cameraDirection, transform.forward);
        if (angle < 90f)
        {
            RaycastHit hit;
            if (Physics.Raycast(mainCamera.transform.position, cameraDirection, out hit))
            {
                if (hit.collider.CompareTag("MainCamera"))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void TriggerJumpScare()
    {
        Vector3 randomPosition = mainCamera.transform.position + Random.onUnitSphere * maxDistanceFromCamera;
        darkFigureInstance = Instantiate(darkFigurePrefab, randomPosition, Quaternion.identity);
        isJumpScareActive = true;
        jumpScareTimer = 0f;
    }

    private void ResetJumpScare()
    {
        Destroy(darkFigureInstance);
        isJumpScareActive = false;
        transform.position = originalPosition;
    }

    private void TriggerFlyBy()
    {
        flyByInstance = Instantiate(flyByPrefab, mainCamera.transform.position, Quaternion.identity);
        isFlyByActive = true;
        flyByTimer = 0f;
    }

    private void ResetFlyBy()
    {
        Destroy(flyByInstance);
        isFlyByActive = false;
    }
}

