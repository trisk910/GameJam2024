using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Shotgun : MonoBehaviour
{
    [Header("Shotgun Settings")]
    public ParticleSystem shotgunParticles;
    public float damage = 10f;
    public float range = 20f;
    public float chargeTime = 1.5f;
    public float rechargeTime = 2f;
    public int maxEnergy = 10;
    public int currentEnergy;

    public int rays = 6;
    public float spreadAngle = 5f;

    private float charge;
    private bool isCharging = false;
    private bool isEquipped = false;
    private bool isRecharging = false;

    [Header("Shotgun UI")]
    public Slider chargeSlider;

    [Header("Shotgun Effects")]
    public ParticleSystem damageableHitParticle;
    public ParticleSystem surfaceHitParticle;
    public Transform shootPoint;

    private void Start()
    {
        if (shotgunParticles == null)
        {
            shotgunParticles = GetComponent<ParticleSystem>();
        }

        currentEnergy = maxEnergy;

        if (chargeSlider != null)
        {
            chargeSlider.value = 0;
            chargeSlider.maxValue = chargeTime;
        }
    }

    private void Update()
    {
        if (isEquipped && !isRecharging)
        {
            if (Input.GetMouseButtonDown(0) && !isCharging && currentEnergy > 0)
            {
                isCharging = true;
                StartCoroutine(ChargeAndShoot());
            }
        }
        if (currentEnergy <= 0 && !isRecharging)
        {
            StartCoroutine(Recharge());
        }
    }

    private IEnumerator ChargeAndShoot()
    {
        charge = 0;
        while (charge < chargeTime)
        {
            charge += Time.deltaTime;
            if (chargeSlider != null)
            {
                chargeSlider.value = charge;
            }
            yield return null;
        }
        Shoot();
        if (chargeSlider != null)
        {
            chargeSlider.value = 0;
        }
        isCharging = false;
    }

    private void Shoot()
    {
        if (shotgunParticles != null)
        {
            shotgunParticles.Play();
        }

        for (int i = 0; i < rays; i++)
        {
            Vector3 randomDirection = shootPoint.forward;
            randomDirection.x += Random.Range(-spreadAngle, spreadAngle) * 0.01f;
            randomDirection.y += Random.Range(-spreadAngle, spreadAngle) * 0.01f;
            randomDirection.z += Random.Range(-spreadAngle, spreadAngle) * 0.01f;
            randomDirection.Normalize();

            RaycastHit hit;
            if (Physics.Raycast(shootPoint.position, randomDirection, out hit, range))
            {
                AlignParticlesWithRay(hit.point);
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageable.TakeDamage(damage);
                    if (damageableHitParticle != null)
                    {
                        Instantiate(damageableHitParticle, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                }
                else
                {
                    if (surfaceHitParticle != null)
                    {
                        Instantiate(surfaceHitParticle, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                }
            }
        }

        currentEnergy--;
    }


    private void AlignParticlesWithRay(Vector3 hitPoint)
    {
        Vector3 direction = (hitPoint - shootPoint.position).normalized;
        shotgunParticles.transform.position = shootPoint.position;
        shotgunParticles.transform.rotation = Quaternion.LookRotation(direction);
    }

    private IEnumerator Recharge()
    {
        isRecharging = true;
        yield return new WaitForSeconds(rechargeTime);
        currentEnergy = maxEnergy;
        //Ficar animacio de recarga
        isRecharging = false;
    }

    public void SetEquipped(bool equipped)
    {
        isEquipped = equipped;
    }

    public void AddEnergy(int energy)
    {
        currentEnergy += energy;
        if (currentEnergy > maxEnergy)
        {
            currentEnergy = maxEnergy;
        }
    }
}
