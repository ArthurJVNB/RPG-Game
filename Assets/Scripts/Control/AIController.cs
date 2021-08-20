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
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointDistanceTolerance = 0.3f;

        private GameObject player;
        private Fighter fighter;
        private Mover mover;

        #region AI Memory
        private Vector3 guardPosition;
        private int currentWaypointIndex = -1;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        #endregion

        private bool InChaseDistanceOfPlayer
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
            if (BehaveInPatrol()) return;
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
        }

        private bool BehaveInCombat()
        {
            if (InChaseDistanceOfPlayer && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                fighter.Attack(player);
                return true;
            }

            return false;
        }

        private bool BehaveInSuspicion()
        {
            if (timeSinceLastSawPlayer < suspicionTime)
            {
                GetComponent<ActionScheduler>().CancelCurrentAction();
                return true;
            }

            return false;
        }

        private bool BehaveInPatrol()
        {
            Vector3 nextPosition = guardPosition;

            if (patrolPath)
            {
                if (AtWaypoint())
                {
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            mover.StartMoveAction(nextPosition);
            return true;
        }

        private bool AtWaypoint()
        {
            if (currentWaypointIndex < 0) return true;
            if (Vector3.Distance(transform.position, GetCurrentWaypoint()) < waypointDistanceTolerance) return true;

            return false;
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetPosition(currentWaypointIndex);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
