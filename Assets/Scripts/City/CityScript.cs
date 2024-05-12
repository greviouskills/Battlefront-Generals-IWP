using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class CityScript : MonoBehaviour
{
    [Header("Ownership")]
    public Ownership owner;
    public CityManager manager;
    [Header("Default City Stats")]
    public int population;
    public float money;
    public float manpower;
    [SerializeField] private float HPRegen = 250;
    [SerializeField]
    public List<ResourceStats> Resources = new List<ResourceStats>();

    [Header("City Combat")]
    [SerializeField]
    private float MHP;
    public float HP;
    public float Resistance;
    [Header("City Current Stats")]
    public List<TroopScript> Combatants = new List<TroopScript>();
    public List<string> Constructing = new List<string>();
    public List<string> Constructed = new List<string>();
    public List<string> Constructable = new List<string>();

    private bool buildingconstruction;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Slider hpbar;
    [Serializable]
    public class ResourceStats
    {
        public string Resource;
        public float Production;
    }
    // Start is called before the first frame update
    void Start()
    {
        MHP = population/10;
        HP = MHP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision obj)
    {
        Debug.Log("triggered");
        if (obj.collider.CompareTag("Troop"))
        {
            TroopScript troop = obj.collider.GetComponent<TroopScript>();
            if(troop.owner.ownerID != owner.ownerID)
            {
                Combatants.Add(troop);
            }
        }
    }
    private void OnCollisionExit(Collision obj)
    {
        if (obj.collider.CompareTag("Troop"))
        {
            TroopScript troop = obj.collider.GetComponent<TroopScript>();
            if (troop.owner.ownerID != owner.ownerID)
            {
                for(int i = 0; i < Combatants.Count; i++)
                {
                    if (Combatants[i].owner.ID == troop.owner.ID)
                    {
                        Combatants.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }

    public void ChangeCityOwnership(string ID,string name ,Color color)
    {
        this.GetComponent<Renderer>().material.color = color;
        owner.ownerID = ID;
        owner.ownername = name;
        StopCoroutine(Construct());
    }

    public void BuildConstruction(string building)
    {
        Constructing.Add(building);
        for(int i = 0;i< Constructable.Count;i++)
        {
            if(Constructable[i] == building)
            {
                Constructable.RemoveAt(i);
            }
            
        }
        if(buildingconstruction ==false)
        {
            StartCoroutine(Construct());
        }

    }

    IEnumerator Construct()
    {
        buildingconstruction = true;
        yield return new WaitForSeconds(90f);
        Constructed.Add(Constructing[0]);
        Constructing.RemoveAt(0);
        manager.SendUpdateCityBuildings(this.gameObject.name, Constructed, Constructable);
        if (Constructing.Count > 0)
        {
            StartCoroutine(Construct());
        }
        else
        {
            buildingconstruction = false;
        }
    }

    public void UpdateBuildings(List<string> buildings, List<string> constructable)
    {
        Constructed.Clear();
        Constructed = buildings;
        Constructable.Clear();
        Constructable = constructable;
    }

    public void DismantleBuilding(string building)
    {
        for(int i = 0; i < Constructed.Count; i++)
        {
            if(Constructed[i] == building)
            {
                Constructable.Add(Constructed[i]);
                Constructed.RemoveAt(i);
            }
        }
    }

    public void TakeDamage(float damage,TroopScript troop)
    {
        canvas.SetActive(true);
        HP -= damage;
        population -= (int)damage/5;
        if(troop.owner.ownerID != owner.ownerID)
        {
            if (HP <= 0)
            {
                HP = population / 100;
                manager.ChangeCityOwner(this.gameObject.name, troop.owner.ownerID, troop.owner.name, new Vector3(troop.modelrenderer.material.color.r, troop.modelrenderer.material.color.g,troop.modelrenderer.material.color.b));
                Combatants.Clear();
            }
            troop.TakeDamage(population * Resistance);
            hpbar.value = HP / MHP;
        }
        else
        {
            troop.removecity(this.gameObject.name);
        }
       
    }

    public void CityIncrement()
    {
        population += (int)(population * 0.0022);
        MHP = population / 10;
        if (HP < MHP && Combatants.Count == 0)
        {
            HP += population / HPRegen;
        }
        if (HP > MHP)
        {
            HP = MHP;
        }
        if(HP == MHP)
        {
            canvas.SetActive(false);
        }
    }

    public void RemoveCombatant(string ID)
    {
        for (int i = 0; i < Combatants.Count; i++)
        {
            if (Combatants[i].owner.ID == ID)
            {
                Combatants.RemoveAt(i);
                break;
            }
        }
    }

}
