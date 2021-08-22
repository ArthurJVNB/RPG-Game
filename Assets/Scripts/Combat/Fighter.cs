using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponCooldown = 0.8f;
        [SerializeField] private float weaponDamage = 5f;

        private Mover mover;
        private ActionScheduler scheduler;
        private Animator animator;
        private Health target;
        private float timeSinceLastAttack = Mathf.Infinity;
        private float currentSpeedFraction;

        private bool IsInRange
        {
            get
            {
                return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
            }
        }

        public bool CanAttack(GameObject gameObject)
        {
            if (gameObject == null) return false;

            Health targetToTest = gameObject.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }


        public void Attack(GameObject target, float speedFraction = 1)
        {
            scheduler.StartAction(this);
            currentSpeedFraction = speedFraction;
            this.target = target.GetComponent<Health>();
        }

        public void Cancel()
        {
            target = null;
            StopAttackAnimation();
        }

        private void Start()
        {
            mover = GetComponent<Mover>();
            scheduler = GetComponent<ActionScheduler>();
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (target == null) return;
            if (target.IsDead) return;

            if (IsInRange)
            {
                mover.Cancel();
                AttackBehaviour();
            }
            else
            {
                mover.MoveTo(target.transform.position, currentSpeedFraction);
            }
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform);
            if (timeSinceLastAttack > weaponCooldown)
            {
                PlayAttackAnimation();
                timeSinceLastAttack = 0;
            }
        }

        // Animation things bellow!
        private void PlayAttackAnimation()
        {
            animator.ResetTrigger("stopAttack");
            // This will trigger the Hit() event.
            animator.SetTrigger("attack");
        }

        private void StopAttackAnimation()
        {
            animator.SetTrigger("stopAttack");
            animator.ResetTrigger("attack");
        }

        // Animation Event
        private void Hit()
        {
            if (!target) return;

            target.TakeDamage(weaponDamage);
        }
    }
}