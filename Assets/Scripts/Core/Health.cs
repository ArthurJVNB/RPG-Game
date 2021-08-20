using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float health = 100f;
        public bool IsDead { get; private set; }

        public void TakeDamage(float damage)
        {
            // health -= damage;
            // if (health < 0) health = 0;

            // Ficará com o que for maior, ou seja, se health-damage for menor que zero, usa zero, pois é o maior.
            health = Mathf.Max(health - damage, 0);

            if (health == 0 && !IsDead)
            {
                Die();
            }
        }

        private void Die()
        {
            if (IsDead) return;

            IsDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }
}