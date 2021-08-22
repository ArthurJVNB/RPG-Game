using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using RPG.Core;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] private Transform target;
        [SerializeField] private float maxSpeed = 5.662f;

        private NavMeshAgent agent;
        private Animator animator;
        private ActionScheduler scheduler;
        private Health health;

        public void StartMoveAction(Vector3 destination, float speedFraction = 1)
        {
            scheduler.StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction = 1)
        {
            agent.destination = destination;
            agent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            agent.isStopped = false;
        }

        public void Cancel()
        {
            agent.isStopped = true;
        }

        private void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            scheduler = GetComponent<ActionScheduler>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            agent.enabled = !health.IsDead;
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            Vector3 velocity = agent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            animator.SetFloat("forwardSpeed", localVelocity.z);
        }
    }

}