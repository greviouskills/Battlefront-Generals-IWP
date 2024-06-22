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
    [SerializeField] private float HPRegen = 2000;
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
    public List<TroopTrain> trainingqueue = new List<TroopTrain>();
    private bool buildingconstruction;
    private bool trainingtroop;
    public TroopSync troopsync;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Slider hpbar;
    [Serializable]
    public class ResourceStats
    {
        public string Resource;
        public float Production;
    }

    [Serializable]
    public class TroopTrain
    {
        public string Troop;
        public float Wait;

        public TroopTrain(string troop, float wait)
        {
            Troop = troop;
            Wait = wait;
        }
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

    public void traintroop(string troop,float wait)
    {
        trainingqueue.Add(new TroopTrain (troop, wait));
        if (trainingtroop == false)
        {
            StartCoroutine(Train());
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
        population -= (int)damage/50;
        if(troop.owner.ownerID != owner.ownerID)
        {
            if (HP <= 0)
            {
                HP = population / 100;
                manager.ChangeCityOwner(this.gameObject.name, troop.owner.ownerID, troop.owner.ownername, new Vector3(troop.modelrenderer.material.color.r, troop.modelrenderer.material.color.g,troop.modelrenderer.material.color.b));
                Combatants.Clear();
                trainingqueue.Clear();
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
        population += (int)(population * 0.00002);
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

    IEnumerator Train()
    {
        
        trainingtroop = true;
        yield return new WaitForSeconds(trainingqueue[0].Wait);
        troopsync.SendSpawnTroops(trainingqueue[0].Troop, owner.ownername, owner.ownerID, this.transform.position);
        trainingqueue.RemoveAt(0);
        
        if (trainingqueue.Count > 0)
        {
            StartCoroutine(Train());
        }
        else
        {
            trainingtroop = false;
        }
    }

    public bool HasBuilding(string building)
    {
        foreach(string build in Constructed)
        {
            if(build == building)
            {
                return true;
            }
        }
        return false;
    }
}
