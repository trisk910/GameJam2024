using UnityEngine;


public interface IDamageable
{
    float Health { get; set; }
    float MaxHealth { get; set; }
    void Heal(float amount);
    bool IsDead();
    void TakeDamage(float amount);
}