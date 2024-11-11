using UnityEngine;
using System.Collections;

public class Shotgun : MonoBehaviour
{
    [Header("Shotgun Settings")]
    public ParticleSystem shotgunParticles;
    public float damage = 10f;
    public float reloadTime = 2f;
    private int maxAmmo = 2;
    public int currentAmmo;
    public int maxShells = 10; 
    public int currentShells;

    private bool isEquipped = false;
    private bool isReloading = false;

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
        if (isEquipped && !isReloading)
        {
            if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
            {
                Shoot();
            }
            if (/*Input.GetKeyDown(KeyCode.R) || */(currentAmmo <= 0 && currentShells > 0))
            {
                StartCoroutine(Reload());
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }

    public void Shoot()
    {
        if (currentAmmo > 0 && shotgunParticles != null)
        {
            shotgunParticles.Play();
            currentAmmo--;
        }
    }

    private IEnumerator Reload()
    {
        if (currentAmmo < maxAmmo && currentShells > 0)
        {
            isReloading = true;

            yield return new WaitForSeconds(reloadTime);

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
        UnityEngine.Debug.Log("Shotgun equipped: " + equipped);
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
