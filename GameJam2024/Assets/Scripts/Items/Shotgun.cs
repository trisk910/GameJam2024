using UnityEngine;
using System.Collections;

public class Shotgun : MonoBehaviour
{
    [Header("Shotgun Settings")]
    public ParticleSystem shotgunParticles;
    public float damage = 10f;
    public float range = 20f;
    public float reloadTime = 2f;
    private int maxAmmo = 2;
    public int currentAmmo;
    public int maxShells = 10;
    public int currentShells;

    private bool isEquipped = false;
    private bool isReloading = false;

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

        currentAmmo = maxAmmo;
        currentShells = maxShells;
    }

    private void Update()
    {
        if (!isReloading)
        {
            if (Input.GetMouseButtonDown(0) && isEquipped  && currentAmmo > 0)
            {
                Shoot();
            }
            if (currentAmmo <= 0 && currentShells > 0)
            {
                StartCoroutine(Reload());
            }
        }
        Debug.Log("Shotgun Equipped: " + isEquipped);
    }

    public void Shoot()
    {
        if (currentAmmo > 0)
        {
            shotgunParticles.Play();

            RaycastHit hit;
            if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit, range))
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
                    //UnityEngine.Debug.Log("Hit Target");
                }
                else
                {
                    
                    if (surfaceHitParticle != null)
                    {
                        Instantiate(surfaceHitParticle, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                    //UnityEngine.Debug.Log("Hit nothing");
                }
            }

            currentAmmo--;
        }
    }
    private void AlignParticlesWithRay(Vector3 hitPoint)
    {
        Vector3 direction = (hitPoint - shootPoint.position).normalized;
        shotgunParticles.transform.position = shootPoint.position;
        shotgunParticles.transform.rotation = Quaternion.LookRotation(direction);
    }



    private IEnumerator Reload()
    {
        if (currentAmmo < maxAmmo && currentShells > 0)
        {
            isReloading = true;
            yield return new WaitForSeconds(reloadTime);

            // Calculate how many shells to reload
            int shellsNeeded = maxAmmo - currentAmmo;
            int shellsToReload = Mathf.Min(shellsNeeded, currentShells);

            currentAmmo += shellsToReload;
            currentShells -= shellsToReload;

            isReloading = false;
        }
    }

    public void SetEquipped(bool equipped)
    {
        isEquipped = equipped;
    }
    public bool CheckShotgunE()
    {
        return isEquipped;
    }

    public void AddAmmo(int shells)
    {
        currentShells += shells;
        if (currentShells > maxShells)
        {
            currentShells = maxShells;
        }
    }
}
