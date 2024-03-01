using UnityEngine;
using System.Collections;

public class CameraJumpScare : MonoBehaviour
{
    [System.Serializable]
    public class JumpScareModelData
    {
        public GameObject jumpScareModel; // Reference to the jump scare model
        [HideInInspector] public Vector3 initialPosition; // Initial position of the jump scare model
        [HideInInspector] public Quaternion initialRotation; // Initial rotation of the jump scare model
    }

    public JumpScareModelData[] jumpScareModels; // Array of jump scare models
    public GameObject screenPrefab; // The prefab for the security camera screen

    [Range(0f, 1f)] public float jumpScareEventProbability = 0.1f; // Probability of the jump scare event happening

    public float jumpScareDuration = 3f; // Duration of the jump scare in seconds

    private bool isJumpScareActive = false; // Flag to indicate if a jump scare is currently active
    private GameObject activeJumpScareModel; // Currently active jump scare model

    void Start()
    {
        // Save the initial positions and rotations of all jump scare models
        foreach (JumpScareModelData modelData in jumpScareModels)
        {
            modelData.initialPosition = modelData.jumpScareModel.transform.position;
            modelData.initialRotation = modelData.jumpScareModel.transform.rotation;
        }
    }

    void Update()
    {
        // Check if the screen prefab is active and a jump scare is not already active
        if (screenPrefab.activeSelf && !isJumpScareActive)
        {
            // Randomly determine if a jump scare should be activated
            if (Random.value < jumpScareEventProbability)
            {
                // Activate a random jump scare model
                ActivateRandomJumpScareModel();
            }
        }
    }

    void ActivateRandomJumpScareModel()
    {
        // Choose a random jump scare model
        JumpScareModelData chosenModel = jumpScareModels[Random.Range(0, jumpScareModels.Length)];

        // Activate the chosen jump scare model
        chosenModel.jumpScareModel.SetActive(true);
        activeJumpScareModel = chosenModel.jumpScareModel;

        // Start the coroutine to deactivate the model after a certain duration
        StartCoroutine(DeactivateJumpScareModel());
    }

    IEnumerator DeactivateJumpScareModel()
    {
        // Set isJumpScareActive flag to true
        isJumpScareActive = true;

        // Wait for the specified duration
        yield return new WaitForSeconds(jumpScareDuration);

        // Deactivate the currently active jump scare model
        activeJumpScareModel.SetActive(false);

        // Reset the position and rotation of the jump scare model
        ResetJumpScareModel(activeJumpScareModel);

        // Reset isJumpScareActive flag to false
        isJumpScareActive = false;
    }

    void ResetJumpScareModel(GameObject jumpScareModel)
    {
        // Find the jump scare model data corresponding to the given model
        JumpScareModelData modelData = System.Array.Find(jumpScareModels, data => data.jumpScareModel == jumpScareModel);

        // Reset the position and rotation of the jump scare model to its initial values
        jumpScareModel.transform.position = modelData.initialPosition;
        jumpScareModel.transform.rotation = modelData.initialRotation;
    }
}
