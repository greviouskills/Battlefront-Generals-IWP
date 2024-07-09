using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogManager : MonoBehaviour
{

    [SerializeField] private Texture2D originalTexture;
    private Texture2D modifiedTexture;

    [SerializeField] private PlayerData player;
    [SerializeField] private TerrainRecorder terrain;
    [SerializeField] private Renderer renderr;
    private List<Vector2Int> ExploredRegions = new List<Vector2Int>();
    // Start is called before the first frame update
    void Start()
    {
        if (originalTexture.format != TextureFormat.RGBA32 && originalTexture.format != TextureFormat.ARGB32)
        {
            // Convert the original texture to a supported format
            Texture2D tempTexture = new Texture2D(originalTexture.width, originalTexture.height, TextureFormat.RGBA32, false);
            tempTexture.SetPixels(originalTexture.GetPixels());
            tempTexture.Apply();
            originalTexture = tempTexture;
        }

        // Create a copy of the original texture without mipmaps
        modifiedTexture = new Texture2D(originalTexture.width, originalTexture.height, TextureFormat.RGBA32, false);

        // Copy the original texture to the modified texture
        Graphics.CopyTexture(originalTexture, modifiedTexture);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateMap(Vector2 location, int revealrange)
    {
        Vector2Int key = terrain.GenerateKey(location,128);
        if (!terrain.HasLocation(key))
        {
            EditTexture(key, revealrange);
        }
    }

    public void EditTexture(Vector2Int key, int revealrange)
    {
        for(int x = key.x - revealrange; x <= key.x + revealrange;)
        {
            if(x >= 0)
            {
                for (int y = key.y - revealrange; y <= key.y + revealrange;)
                {
                    if (y >= 0)
                    {
                        Vector2Int newkey = new Vector2Int(x, y);
                        if (!ExploredRegions.Contains(newkey))
                        {
                            Color newcolor = new Color(0, 0, 0, 0);
                            modifiedTexture.SetPixel(x, y, newcolor);
                            ExploredRegions.Add(newkey);
                        }
                    }
                    y++;
                }
            }
            x++;
        }
        modifiedTexture.Apply();

        renderr.material.mainTexture = modifiedTexture;
    }
}
