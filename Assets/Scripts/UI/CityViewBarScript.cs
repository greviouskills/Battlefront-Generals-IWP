using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CityViewBarScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Text building, status;
    private CityScript city;
    private CityViewScript mgr;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetType2(string name,CityScript target,CityViewScript manager)
    {
        city = target;
        building.text = name;
        mgr = manager;
    }

    public void SetType1(string name,string stat,CityScript target)
    {
        city = target;
        building.text = name;
        status.text = stat;
    }

    public void OnButtonPress()
    {
        city.BuildConstruction(building.text);
        mgr.ResetUI();
    }
}
