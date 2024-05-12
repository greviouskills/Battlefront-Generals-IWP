using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class TroopScript : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Ownership")]
    public Ownership owner;
    public TroopSync syncer;
    [Header("Default Stats")]
    public string TroopName;
    public string TroopType;
    public string Type;
    public float Health, Attack,FightingCapacity;
    private float MHealth, MAttack;
    public string TargetType;
    public TroopMovementScript movement;
    [Header("Balancing")]
    [SerializeField]
    private List<CounterStats>Counters = new List<CounterStats>();
    [Header("Combat")]
    public List<TroopScript> combatants = new List<TroopScript>();
    public List<TroopScript> combatlockers = new List<TroopScript>();
    public List<CityScript> CapturingCity = new List<CityScript>();

    public Renderer modelrenderer;
    [SerializeField] private Slider HPbar;
    [SerializeField] private ParticleSystem emitter;

    [Serializable]
    private class CounterStats
    {
        public string TroopType;
        public float AttackMulti;
    }
    void Start()
    {
       
        MHealth = Health;
        MAttack = Attack;
        //StartCoroutine(UpdateSystem());
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateStats()
    {
        FightingCapacity = Health / MHealth;
        Attack = MAttack * FightingCapacity;
        HPbar.value = FightingCapacity;
    }
    private float ReturnEffectiveDamage(string target)
    {
        float FightingDamage = Attack/(combatants.Count+CapturingCity.Count);
        foreach(var counter in Counters)
        {
            if(counter.TroopType == target)
            {
                FightingDamage *= counter.AttackMulti;
                break;
            }
        }
        return FightingDamage;
    }

    //IEnumerator UpdateSystem()
    //{
    //    while (true)
    //    {
    //        UpdateStats();
    //        AttackCombatants();
    //        yield return new WaitForSeconds(1f);

    //    }
    //}

    public void AttackCombatants()
    {
        if (combatants.Count > 0)
        {

            for (int i = 0; i < combatants.Count; i++)
            {
                if (combatants[i] != null)
                {
                   combatants[i].TakeDamage(ReturnEffectiveDamage(combatants[i].TroopType));
                }
            }
        }

        if(CapturingCity.Count > 0)
        {
            for (int i = 0; i < CapturingCity.Count; i++)
            {
                if (CapturingCity[i] != null)
                {
                    CapturingCity[i].TakeDamage(ReturnEffectiveDamage("City"),this);
                }
            }
        }


    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        emitter.Play();
        if(Health <= 0)
        {
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        foreach(var combatant in combatants)
        {
            combatant.RemoveCombatant(owner.ID);
        }
        foreach(var city in CapturingCity)
        {
            city.RemoveCombatant(owner.ID);
        }
        if (syncer.RemoveTroop(owner.ID))
        {
            Destroy(this.gameObject);
        }
        
    }

    public void RemoveCombatant( string ID)
    {
        for (int i = 0; i < combatants.Count; i++)
        {
            if (combatants[i].owner.ID == ID)
            {
                combatants.RemoveAt(i);
                break;
            }
        }
        for (int i = 0; i < combatlockers.Count; i++)
        {
            if (combatlockers[i].owner.ID == ID)
            {
                combatlockers.RemoveAt(i);
                if(combatlockers.Count == 0)
                {
                    movement.canmove = true;
                }
                break;
            }
        }
        return;
    }

    public void SetColor(Color color)
    {
        modelrenderer.material.color = color;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Troop"))
        {
            TroopScript temp = other.gameObject.GetComponent<TroopScript>();
            if (temp.Type == TargetType && temp.owner.ownerID != owner.ownerID)
            {
                combatlockers.Add(temp);
                movement.canmove = false;
            }
        }
        else if (other.gameObject.CompareTag("City"))
        {
            Debug.Log("touched city");
            CityScript temp = other.gameObject.GetComponent<CityScript>();
            if(temp.owner.ownerID != owner.ownerID && Type == "Ground")
            {
                CapturingCity.Add(temp);
            }
        }
    }

    public void removecity(string cityname)
    {
        for (int i = 0; i < CapturingCity.Count; i++)
        {
            if(CapturingCity[i].gameObject.name == cityname)
            {
                CapturingCity.RemoveAt(i);
            }
        }
    }
}