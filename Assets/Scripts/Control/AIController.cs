using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Combat;
using RPG.Movement;
using System;
using RPG.Core;
using UnityEngine.AI;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 5f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointDistanceTolerance = 0.3f;
        [SerializeField] private float waypointDwellTime = 10f;
        [Range(0, 1)]
        [SerializeField] private float patrolSpeedFraction = 0.27f;
        [Range(0, 1)]
        [SerializeField] private float chaseSpeedFraction = 0.8f;
        // [SerializeField] private float walkingSpeed = 1.558f;
        // [SerializeField] private float runningSpeed = 4.4f;


        private GameObject player;
        private Fighter fighter;
        private Mover mover;
        private NavMeshAgent agent;

        #region AI Memory
        private Vector3 guardPosition;
        private int currentWaypointIndex = -1;
        private float timeSinceLastSawPlayer = Mathf.Infinity;
        private float timeGuardingAtWaypoint = Mathf.Infinity;
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
            agent = GetComponent<NavMeshAgent>();

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
            // timeGuardingAtWaypoint += Time.deltaTime;
        }

        private bool BehaveInCombat()
        {
            if (InChaseDistanceOfPlayer && fighter.CanAttack(player))
            {
                timeSinceLastSawPlayer = 0;
                // agent.speed = runningSpeed;
                fighter.Attack(player, chaseSpeedFraction);
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
                    if (timeGuardingAtWaypoint > waypointDwellTime)
                    {
                        CycleWaypoint();
                        timeGuardingAtWaypoint = 0;
                    }
                    else
                    {
                        timeGuardingAtWaypoint += Time.deltaTime;
                    }
                }
                nextPosition = GetCurrentWaypoint();
            }

            // agent.speed = walkingSpeed;
            mover.StartMoveAction(nextPosition, patrolSpeedFraction);
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
