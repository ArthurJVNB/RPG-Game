using UnityEngine;
using RPG.Movement;
using RPG.Core;

namespace RPG.Combat
{
    [RequireComponent(typeof(Mover))]
    [RequireComponent(typeof(ActionScheduler))]
    [RequireComponent(typeof(Animator))]
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float weaponCooldown = 0.8f;
        [SerializeField] float weaponDamage = 5f;

        Mover mover;
        ActionScheduler scheduler;
        Animator animator;
        Health target;
        float timeSinceLastAttack = Mathf.Infinity;
        float currentSpeedFraction;

        private bool IsInRange
        {
            get
            {
                return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
            }
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
            mover.Cancel();
            StopAttackAnimation();
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