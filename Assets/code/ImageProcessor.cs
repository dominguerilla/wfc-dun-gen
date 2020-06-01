using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageProcessor : MonoBehaviour
{
    [SerializeField] private Sprite inputImage;
    [SerializeField] private RawImage displayInputImage;
    [SerializeField] private RawImage tileImage;

    private void Start()
    {
        if (inputImage)
        {
            UpdateDisplayInputImage(inputImage);
        }
    }

    public void UpdateDisplayInputImage(Sprite img) {
        displayInputImage.texture = img.texture;
        
    }

    public void ListColors()
    {
        Color[] displayColors = ((Texture2D) displayInputImage.texture).GetPixels();
        Debug.Log("COLORS: ");
        foreach (Color c in displayColors) {
            Debug.Log(c);
        }
    }

    public void DisplayFirstTile(int tileSize)
    {
        Color[] inputColors = ((Texture2D)displayInputImage.texture).GetPixels();
        int inputWidth = displayInputImage.texture.width; // Assuming a square input image!

        Color[] firstTile = SampleTileAt(inputColors, tileSize, inputWidth, 0, 0);

        Texture2D texture = new Texture2D(tileSize, tileSize);
        texture.SetPixels(firstTile);
        Debug.Log("texture pixels:");
        foreach (Color c in texture.GetPixels())
        {
            Debug.Log(c);
        }
        texture.filterMode = FilterMode.Point;
        texture.Apply();
        tileImage.texture = texture;
    }

    Color[] SampleTileAt(Color[] input, int tileSize, int inputWidth, int x, int y)
    {
        Color[] tile = new Color[tileSize * tileSize];
        int tileIndex = 0;

        for (int tile_y = y; tile_y < y + tileSize; tile_y++)
        {
            for (int tile_x = x; tile_x < x + tileSize; tile_x++)
            {
                int desiredX = (tile_x) % (inputWidth);
                int desiredY = (tile_y) % (inputWidth);
                Color pixel = GetColorAt(input, inputWidth, desiredX, desiredY);
                tile[tileIndex] = pixel;
                tileIndex++;
            }
        }
        return tile;
    }

    Color GetColorAt(Color[] input, int inputWidth, int x, int y)
    {
        int pixelIndex = (inputWidth * y) + x;
        return input[pixelIndex];
    }

    public void ResetTileImage()
    {
        tileImage.color = Color.white;
        tileImage.texture = null;
    }
}
