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
        //int Vote = 0;
        //Color TerColor = new Color();
        //int r = 0;
        //int g = 0;
        //int b = 0;
        //for (int x = -5; x<= 5; x += 5)
        //{
        //    for (int y = -5; y <= 5; y += 5)
        //    {
        //        TerColor = GetColor(new Vector2(key.x + x, key.y + y));
        //        r = Mathf.RoundToInt(TerColor.r * 255);
        //        g = Mathf.RoundToInt(TerColor.g * 255);
        //        b = Mathf.RoundToInt(TerColor.b * 255);
        //        if (r == 11 && g == 10 && b == 49)
        //        {
        //            Vote++;
        //        }
        //    }
        //}
        //Debug.Log("Voted "+ Vote );
        //if (Vote > 0)
        //{
        //    Debug.Log("on water");
        //    return true;
        //}
        //else
        //{
        //    return false;
        //}
        Color TerColor = GetColor(key);
        int r = Mathf.RoundToInt(TerColor.r * 25);
        int g = Mathf.RoundToInt(TerColor.g * 25);
        int b = Mathf.RoundToInt(TerColor.b * 25);

        Debug.Log(r + ", " + g + ", " + b);
        if (r == 1 && g == 1 && b == 5)
        {
            Debug.Log("on water");
            return true;
        }
        return false;
    }
}
