using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CityViewScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Bar1, Bar2;
    [SerializeField] private Transform UiParent;
    private CityScript targetcity;

    [SerializeField] private Text Population, Cityname, Owner;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUi(CityScript city, bool CanSpy, bool isowner)
    {
        targetcity = city;

        foreach (Transform child in UiParent)
        {
            // Destroy the child GameObject
            Destroy(child.gameObject);
        }

        if (isowner || CanSpy)
        {
            foreach (string building in city.Constructed)
            {
                GameObject obj = Instantiate(Bar1, UiParent);
                obj.GetComponent<CityViewBarScript>().SetType1(building, "Built", city);
            }
        }
        if (isowner)
        {
            foreach (string building in city.Constructing)
            {
                GameObject obj = Instantiate(Bar1, UiParent);
                obj.GetComponent<CityViewBarScript>().SetType1(building, "Building..", city);
            }
            foreach (string building in city.Constructable)
            {
                GameObject obj = Instantiate(Bar2, UiParent);
                obj.GetComponent<CityViewBarScript>().SetType2(building, city,this);
            }
        }
    }

    public void ResetUI()
    {
        foreach (Transform child in UiParent)
        {
            // Destroy the child GameObject
            Destroy(child.gameObject);
        }
        foreach (string building in targetcity.Constructed)
        {
            GameObject obj = Instantiate(Bar1, UiParent);
            obj.GetComponent<CityViewBarScript>().SetType1(building, "Built", targetcity);
        }
        foreach (string building in targetcity.Constructing)
        {
            GameObject obj = Instantiate(Bar1, UiParent);
            obj.GetComponent<CityViewBarScript>().SetType1(building, "Building..", targetcity);
        }
        foreach (string building in targetcity.Constructable)
        {
            GameObject obj = Instantiate(Bar2, UiParent);
            obj.GetComponent<CityViewBarScript>().SetType2(building, targetcity, this);
        }
    }
}
