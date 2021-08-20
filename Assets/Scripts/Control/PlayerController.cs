using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Movement;
using RPG.Combat;
using RPG.Core;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover mover;
        private Fighter fighter;
        private ActionScheduler scheduler;

        private void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            scheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            if (GetComponent<Health>().IsDead) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in hits)
            {
                // Código semelhante ao do professor:
                // CombatTarget target = hit.transform.GetComponent<CombatTarget>();

                // if (!fighter.CanAttack(target)) continue;

                // if (Input.GetMouseButtonDown(0))
                // {
                //     fighter.Attack(target);
                // }

                // return true;

                // Minha forma de raciocinar:
                if (hit.transform.TryGetComponent<CombatTarget>(out CombatTarget target) && fighter.CanAttack(target.gameObject))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        fighter.Attack(target.gameObject);
                    }
                    return true;
                }
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);

            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(hit.point);
                }

                return true;
            }

            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
