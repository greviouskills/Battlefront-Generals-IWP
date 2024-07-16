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
    [Header("Misc")]
    public bool Selected = false;
    public Renderer modelrenderer,boatrenderer;
    [SerializeField] private Slider HPbar;
    [SerializeField] private GameObject hpcanvas,selectionindicator;
    [SerializeField] private ParticleSystem emitter;

    public bool UnderAttack;
    private bool UnderAtkCD;
    private int combatantcount;
    [Serializable]
    private class CounterStats
    {
        public string TroopType;
        public float AttackMulti;
    }
    void Start()
    {
        StartCoroutine(Heal());
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
    private float ReturnEffectiveDamage(string target,string targettype)
    {
        float FightingDamage = Attack/(combatantcount+CapturingCity.Count);
        if (TargetType != targettype)
        {
            return 0;
        }
        foreach (var counter in Counters)
        {
            if(counter.TroopType == target)
            {
                FightingDamage *= counter.AttackMulti;
                break;
            }
        }
        return FightingDamage;
    }

    private int GetNumOfActiveCombatants()
    {
        int count = 0;
        foreach(var target in combatants)
        {
            if(target.Type == TargetType)
            {
                count++;
            }
        }
        return count;
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
        if (Health <= 0)
        {
            DestroySelf();
            return;
        }

        combatantcount = GetNumOfActiveCombatants();
        if (combatants.Count > 0)
        {

            for (int i = 0; i < combatants.Count; i++)
            {
                if (combatants[i] != null)
                {
                   combatants[i].TakeDamage(ReturnEffectiveDamage(combatants[i].TroopType,combatants[i].Type));
                }
            }
        }

        if(CapturingCity.Count > 0)
        {
            for (int i = 0; i < CapturingCity.Count; i++)
            {
                if (CapturingCity[i] != null)
                {
                    CapturingCity[i].TakeDamage(ReturnEffectiveDamage("City","Ground"),this);
                }
            }
        }


    }

    public void TakeDamage(float damage)
    {
        if (!UnderAttack)
        {
            StartCoroutine(UnderAttackCoolD());
        }
        UnderAtkCD = true;
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
        if (boatrenderer != null) boatrenderer.material.color = color;
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

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("City"))
        {
            Debug.Log("left city");
            CityScript temp = other.gameObject.GetComponent<CityScript>();
            removecity(temp.gameObject.name);
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

    private IEnumerator Heal()
    {
        yield return new WaitForSeconds(1f);

        if(Health < MHealth && combatantcount == 0 && CapturingCity.Count == 0 && movement.Waypoints.Count == 0)
        {
            Health += MHealth / 100;
        }
        if(Health > MHealth)
        {
            Health = MHealth;
        }
        StartCoroutine(Heal());
    }

    public void SetBarActive(bool active)
    {
        hpcanvas.SetActive(active);
    }

    public void select(bool selected)
    {
        Selected = selected;
        selectionindicator.SetActive(selected);
    }

    IEnumerator UnderAttackCoolD()
    {
        UnderAttack = true;
        while (UnderAtkCD)
        {
            UnderAtkCD = false;
            yield return new WaitForSeconds(3);
        }
        UnderAttack = false;
    }
}
