using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] PlayerShooter shooter;
    [SerializeField] ParticleSystem hitEffectMetal;
    [SerializeField] ParticleSystem hitEffectHuman;
    [SerializeField] ParticleSystem muzzleEffect;
    [SerializeField] TrailRenderer bulletTrail;
    [SerializeField] float bulletSpeed;
    [SerializeField] float maxDistance;
    [SerializeField] int damage;
    
    public int bulletCapacity = 30;
    public int bulletRemain;
    public int bulletUsed;

    public void Fire()
    {
        muzzleEffect.Play();
        bulletUsed++;
        bulletRemain = bulletCapacity - bulletUsed;
        if (bulletRemain <= 0)
        {
            shooter.Reload();
        }

        RaycastHit hit;
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance))
        {
            LivingEntity hittable = hit.transform.GetComponent<LivingEntity>();
            ParticleSystem effectMetal = GameManager.Resource.Instantiate(hitEffectMetal, hit.point, Quaternion.LookRotation(hit.normal));
            effectMetal.transform.parent = hit.transform;
            
            StartCoroutine(ReleaseRoutine(effectMetal.gameObject));

            StartCoroutine(TrailRoutine(muzzleEffect.transform.position, hit.point));

            hittable?.Hit(hit, damage);
        }
        else
        {
            StartCoroutine(TrailRoutine(muzzleEffect.transform.position, Camera.main.transform.forward * maxDistance));
        }
    }

    IEnumerator ReleaseRoutine(GameObject effect)
    {
        yield return new WaitForSeconds(3f);
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
