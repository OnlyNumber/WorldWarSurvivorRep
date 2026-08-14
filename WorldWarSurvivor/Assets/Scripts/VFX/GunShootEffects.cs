using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunShootEffects : VisualEffects
{

    [SerializeField] private float timeOfExist;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float coneDirection;


    [SerializeField] private ParticleSystem FireEffect;
    [SerializeField] private GameObject projectile;


    [ContextMenu("ActivateEffect")]
    public override void ActivateEffect()
    {
        base.ActivateEffect();
        Debug.Log("Activate effect");
        FireEffect.Play(true);
        
        StartCoroutine(BurstFire());
    }

    private IEnumerator BurstFire()
    {
        for (int i = 0; i < 6; i++)
        {
            StartCoroutine(MoveProjectile());
            yield return new WaitForSeconds(0.06f);

        }
        
        FireEffect.Stop();
    }

    private IEnumerator MoveProjectile()
    {
        var CurrentProjectile = Instantiate(projectile);

        CurrentProjectile.transform.position = transform.position;
        CurrentProjectile.transform.rotation = transform.rotation;
        float currentFlyingTime = 0;

        var randInConeDirection = CurrentProjectile.transform.rotation.eulerAngles + new Vector3(Random.Range(-coneDirection, coneDirection), Random.Range(-coneDirection, 5f));
        CurrentProjectile.transform.rotation = Quaternion.Euler(randInConeDirection);

        do
        {
            CurrentProjectile.transform.position = CurrentProjectile.transform.position + CurrentProjectile.transform.forward * projectileSpeed * Time.deltaTime;
            currentFlyingTime += Time.deltaTime;

            yield return null;

        } while (currentFlyingTime < timeOfExist);

    }
}
