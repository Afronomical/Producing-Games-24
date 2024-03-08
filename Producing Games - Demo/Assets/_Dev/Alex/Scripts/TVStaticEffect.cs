using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVStaticEffect : MonoBehaviour
{
    private int height = 256;
    private int width = 256;
    private float scale = 80;

    [Header("Panning Variables")]
    public float offsetX = 100f;

    private Renderer renderer;
    private Texture2D texture;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();

        texture = new Texture2D(width, height);
    }

    private Texture2D UpdateTexture()
    {
        for(int x = 0; x < width; ++x)
        {
            for(int y = 0; y < height; ++y)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();

        return texture;
    }

    private Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        TVStatic();
    }

    private void TVStatic()
    {
        if (offsetX > 1000) offsetX = 0;
        else offsetX += 100;
        renderer.material.mainTexture = UpdateTexture();
    }
}
