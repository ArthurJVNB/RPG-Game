using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using System;
using RPG.Core;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 5f;

        private GameObject player;
        private Fighter fighter;
        private Mover mover;

        #region AI Memory
        private Vector3 guardPosition;
        private float timeSinceLastSawPlayer = Mathf.Infinity; 
        #endregion

        private bool InAttackRangeOfPlayer
        {
            get
            {
                return Vector3.Distance(transform.position, player.transform.position) < chaseDistance;
            }
        }

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
        }

        private void Update()
        {
            UpdateTimers();

            if (GetComponent<Health>().IsDead) return;
            if (BehaveInCombat()) return;
            if (BehaveInSuspicion()) return;
            if (BehaveInGuard()) return;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private bool BehaveInSuspicion()
        {
            if (timeSinceLastSawPlayer < suspicionTime)
            {
                // be suspicious
                GetComponent<ActionScheduler>().CancelCurrentAction();
                return true;
            }

            return false;
        }

        private bool BehaveInCombat()
        {
            if (InAttackRangeOfPlayer && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                fighter.Attack(player);
                return true;
            }

            return false;
        }

        private bool BehaveInGuard()
        {
            mover.StartMoveAction(guardPosition);
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
