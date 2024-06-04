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
    [SerializeField] private GameObject Button;
    [SerializeField] private TroopBuilderPanel builderpanel;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUi(CityScript city, bool CanSpy, bool isowner)
    {
        Button.SetActive(false);
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
                obj.GetComponent<CityViewBarScript>().SetType1(building, "Built");
            }
            
        }
        if (isowner)
        {
            Button.SetActive(true);
            foreach (string building in city.Constructing)
            {
                GameObject obj = Instantiate(Bar1, UiParent);
                obj.GetComponent<CityViewBarScript>().SetType1(building, "Building..");
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
            obj.GetComponent<CityViewBarScript>().SetType1(building, "Built");
        }
        foreach (string building in targetcity.Constructing)
        {
            GameObject obj = Instantiate(Bar1, UiParent);
            obj.GetComponent<CityViewBarScript>().SetType1(building, "Building..");
        }
        foreach (string building in targetcity.Constructable)
        {
            GameObject obj = Instantiate(Bar2, UiParent);
            obj.GetComponent<CityViewBarScript>().SetType2(building, targetcity, this);
        }
    }

    public void OnButtonPressed()
    {
        builderpanel.gameObject.SetActive(true);
        builderpanel.city = targetcity;
        builderpanel.UpdateUi();
        this.gameObject.SetActive(false);
    }
}
