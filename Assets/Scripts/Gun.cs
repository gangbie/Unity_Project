using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour                         
{
    protected GunData data;

    public PlayerShooter shooter;

    private string name;
    
    public LayerMask enemyLayer;
    public int bulletCapacity = 30;
    public int bulletRemain;
    public int bulletUsed;

    public string info;                                           
    public float bulletSpeed;                                     
    public float maxDistance;                                     
    public int damage;                                            
    public ParticleSystem hitEffectMetal;
    public ParticleSystem hitEffectHuman;
    public ParticleSystem muzzleEffect;
    public TrailRenderer bulletTrail;

    protected virtual void Awake()
    {
        shooter = GetComponentInParent<PlayerShooter>();
    }

    private void OnEnable()
    {
        GameManager.data.UpdateBullet(bulletCapacity);
    }

    public virtual void Fire()                                             
    {
        muzzleEffect.Play();
        bulletUsed++;
        bulletRemain = bulletCapacity - bulletUsed;
        GameManager.data.UpdateBullet(bulletRemain);

        if (bulletRemain <= 0)
        {
            shooter.Reload();
        }

        RaycastHit hit;
        var hitPosition = Vector3.zero;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        {
            if (((1 << hit.transform.gameObject.layer) & enemyLayer) != 0)
            {
                ParticleSystem effectHuman = GameManager.Resource.Instantiate(hitEffectHuman, hit.point, Quaternion.LookRotation(hit.normal));
                effectHuman.transform.parent = hit.transform;
                StartCoroutine(ReleaseRoutine(effectHuman.gameObject));
            }
            else
            {
                ParticleSystem effectMetal = GameManager.Resource.Instantiate(hitEffectMetal, hit.point, Quaternion.LookRotation(hit.normal));
                effectMetal.transform.parent = hit.transform;
                StartCoroutine(ReleaseRoutine(effectMetal.gameObject));
            }

            var target = hit.collider.GetComponent<IHittable>();

            if (target != null)
            {
                DamageMessage damageMessage;

                damageMessage.damager = shooter.gameObject;
                damageMessage.amount = damage;
                damageMessage.hitPoint = hit.point;
                damageMessage.hitNormal = hit.normal;
                target.Hit(gameObject, hit);
                target.ApplyDamage(damageMessage);
            }
            else
            {
                
            }
            hitPosition = hit.point;

        }
        else
        {
            hitPosition = Camera.main.transform.position + Camera.main.transform.forward * maxDistance;
        }

        StartCoroutine(TrailRoutine(muzzleEffect.transform.position, hitPosition));

    }

    IEnumerator ReleaseRoutine(GameObject effect)
    {
        yield return new WaitForSeconds(1.5f);
        GameManager.Resource.Destroy(effect);
    }

    IEnumerator TrailRoutine(Vector3 startPoint, Vector3 endPoint)
    {
        TrailRenderer trail = GameManager.Resource.Instantiate(bulletTrail, startPoint, Quaternion.identity, true);
        trail.Clear();

        float totalTime = Vector2.Distance(startPoint, endPoint) / bulletSpeed;

        float rate = 0;
        while (rate < 1)
        {
            trail.transform.position = Vector3.Lerp(startPoint, endPoint, rate);
            rate += Time.deltaTime / totalTime;

            yield return null;
        }
        GameManager.Resource.Destroy(trail.gameObject);
    }
}
