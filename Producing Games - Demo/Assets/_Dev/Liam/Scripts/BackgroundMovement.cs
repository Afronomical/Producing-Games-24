using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    public float scrollSpeed = 1.0f;
    public Material churchMaterial; // Assign the material of your church interior

    void Update()
    {
        // Calculate offset based on time and scroll speed
        float offsetX = Time.time * scrollSpeed;

        // Set the offset to create the scrolling effect
        churchMaterial.mainTextureOffset = new Vector2(offsetX, 0f);
    }
}

