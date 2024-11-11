using UnityEngine;


public interface IDamageable
{
    float Health { get; set; }
    float MaxHealth { get; set; }

    void TakeDamage(float amount);
    void Heal(float amount);
    bool IsDead();
}