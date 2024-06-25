using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainRecorder : MonoBehaviour
{
    public Texture2D map;
    private Dictionary<Vector2, Color>mapterrain = new Dictionary<Vector2, Color>();
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(map.width + ", " + map.height);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Color GetColor(Vector2 key)
    {
        int x = Mathf.RoundToInt((key.x + 750) * 0.3413f);
        int z = Mathf.RoundToInt((key.y + 750) * 0.3413f);
        Vector2 RefKey = new Vector2(x, z);
        if (mapterrain.ContainsKey(RefKey))
        {
            Debug.Log("pulled from dictonary");
            return mapterrain[RefKey];
        }
        else
        {
            Color TerColor = map.GetPixel(x, z);
            //float r = Mathf.RoundToInt(TerColor.r * 255);
            //float g = Mathf.RoundToInt(TerColor.g * 255);
            //float b = Mathf.RoundToInt(TerColor.b * 255);
            mapterrain.Add(RefKey, TerColor);
            return mapterrain[RefKey];
        }
    }

    public bool CheckForWater(Vector2 key)
    {

        Color TerColor = GetColor(key);
        int r = Mathf.RoundToInt(TerColor.r * 255);
        int g = Mathf.RoundToInt(TerColor.g * 255);
        int b = Mathf.RoundToInt(TerColor.b * 255);
        if (r == 11 && g == 10 && b == 49)
        {
            Debug.Log("on water");
            return true;
        }
        return false;
    }
}
