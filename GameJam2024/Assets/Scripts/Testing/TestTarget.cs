using UnityEngine;

public class TestTarget : MonoBehaviour, IDamageable
{
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public float initialHealth = 50f; // Initial health value

    private TestTargetSpawner spawner;

    private void Start()
    {
        MaxHealth = initialHealth;
        Health = MaxHealth;

        spawner = GetComponentInParent<TestTargetSpawner>();
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        UnityEngine.Debug.Log($"TestTarget took {damage} damage. Remaining health: {Health}");

        if (IsDead())
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        Health += amount;
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }

    public bool IsDead()
    {
        return Health <= 0;
    }

    private void Die()
    {
        UnityEngine.Debug.Log("TestTarget is destroyed!");

        if (spawner != null)
        {
            spawner.RespawnTarget();
        }

        Destroy(gameObject);
    }
}
