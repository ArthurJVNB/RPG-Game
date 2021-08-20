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

        private GameObject player;
        private Fighter fighter;

        #region AI Memory
        private Vector3 guardPosition;
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

            guardPosition = transform.position;
        }

        private void Update()
        {
            if (GetComponent<Health>().IsDead) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            if (InAttackRangeOfPlayer && fighter.CanAttack(player))
            {
                fighter.Attack(player);
                return true;
            }

            return false;
        }

        private bool InteractWithMovement()
        {
            GetComponent<Mover>().StartMoveAction(guardPosition);
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
