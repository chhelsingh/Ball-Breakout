using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    private SpriteRenderer sr;
    public int HitPoints = 1;
    public ParticleSystem DestroyEffect;
    public static event Action<Brick> OnBrickDestruction;

    private void Awake() 
    {
        this.sr = this.GetComponent<SpriteRenderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        ApplyCollisionLogic(ball);
    }

    private void ApplyCollisionLogic(Ball ball)
    {
        this.HitPoints--;

        if(this.HitPoints <= 0)
        {
            BrickManager.Instance.RemainingBricks.Remove(this);
            OnBrickDestruction?.Invoke(this);
            OnBrickDestroy();
            SpawanDestroyEffect();
            Destroy(this.gameObject);
        }
        else
        {
            this.sr.sprite = BrickManager.Instance.Sprites[this.HitPoints -1];
        }        
    }

    private void OnBrickDestroy()
    {
        float buffSpawnChance = UnityEngine.Random.Range(0, 100f);
        float deBuffSpawnChance = UnityEngine.Random.Range(0, 100f);
        bool alreadySpawned = false;

        if(buffSpawnChance <= CollectableManager.Instance.BuffChance)
        {
            alreadySpawned = true;
            
            Collectable newBuff = this.SpawnCollectable(true);
        }

        if(deBuffSpawnChance <= CollectableManager.Instance.DebuffsChance && !alreadySpawned)
        {
            Collectable newBuff = this.SpawnCollectable(false);
        }

    }

    private Collectable SpawnCollectable(bool isBuff)
    {
        List<Collectable> collection;

        if(isBuff)
        {
            collection = CollectableManager.Instance.AvailableBuffs;
        }
        else
        {
            collection = CollectableManager.Instance.AvailableDebuffs;
        }

        int buffIndex = UnityEngine.Random.Range(0, collection.Count);
        Collectable prefab = collection[buffIndex];
        Collectable newCollectable = Instantiate(prefab, this.transform.position, Quaternion.identity) as Collectable;

        return newCollectable;
    }

    private void SpawanDestroyEffect()
    {
        Vector3 brickPos = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPos.x, brickPos.y, brickPos.z - 0.2f);
        GameObject effect = Instantiate(DestroyEffect.gameObject, spawnPosition, Quaternion.identity);

        MainModule mm = effect.GetComponent<ParticleSystem>().main;
        mm.startColor = this.sr.color;
        Destroy(effect, DestroyEffect.main.startLifetime.constant);
    }

    public void Init(Transform containerTransform, Sprite sprite, Color color, int hitpoints)
    {
        this.transform.SetParent(containerTransform);
        this.sr.sprite = sprite;
        this.sr.color = color;
        this.HitPoints = hitpoints;
    }
}
