using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public bool isLightningBall;
    private SpriteRenderer spriteRenderer;
    public static event Action<Ball> OnBallDeath;
    public static event Action<Ball> OnLightningBallEnable;
    public static event Action<Ball> OnLightningBallDisable;

    public ParticleSystem lightningBallEffect;
    public float lightningBallDuration = 10;

    private void Awake() 
    {
        this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Die()
    {
       OnBallDeath?.Invoke(this);
       Destroy(gameObject, 1);
    }

    public void StartLightningBall()
    {
        if(!this.isLightningBall)
        {
            this.isLightningBall = true;
            this.spriteRenderer.enabled = false;
            lightningBallEffect.gameObject.SetActive(true);
            StartCoroutine(StopLightningBallAfterTime(this.lightningBallDuration));

            OnLightningBallEnable?.Invoke(this);
        }
    }

    private IEnumerator StopLightningBallAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StopLightningBall();
    }

    private void StopLightningBall()
    {
        if(this.isLightningBall)
        {
            this.isLightningBall = false;
            this.spriteRenderer.enabled = true;
            lightningBallEffect.gameObject.SetActive(false);

            OnLightningBallDisable?.Invoke(this);
        }
    }
}
