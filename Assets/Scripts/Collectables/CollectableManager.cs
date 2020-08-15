using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableManager : MonoBehaviour
{
    #region Singleton

    private static CollectableManager _instance;
    public static CollectableManager Instance => _instance;

    private void Awake() 
    {
        if(_instance != null )
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    
    #endregion

    public List<Collectable> AvailableBuffs;
    public List<Collectable> AvailableDebuffs;

    [Range(0, 100)]
    public float BuffChance;

    [Range(0, 100)]
    public float DebuffsChance;
}
