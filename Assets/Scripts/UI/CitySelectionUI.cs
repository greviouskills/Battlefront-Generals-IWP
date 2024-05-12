using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CitySelectionUI : MonoBehaviour
{
    [SerializeField]private Text cityname;
    private CitySelectionManager mgr;
    public string targetcity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Setup(string city,CitySelectionManager manager)
    {
        mgr = manager;
        cityname.text = city;
        targetcity = city;
    }
    public void OnButtonPressed()
    {
        mgr.removecity(targetcity);
    }
    public void selfdestruct()
    {
        Destroy(this.gameObject);
    }
}
