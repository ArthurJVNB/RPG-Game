using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            NorthGate,
            SouthGate
        }

        [SerializeField] UnityEngine.Object sceneToLoad;
        [SerializeField] Transform spawnPoint;
        [Tooltip("Where this portal leads.")]
        [SerializeField] DestinationIdentifier destination;

        Portal otherPortal;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(GameTags.player))
            {
                //    SceneManager.LoadSceneAsync(sceneToLoad.name);
                //    SceneManager.sceneLoaded += OnSceneLoaded;

                StartCoroutine(LoadSceneRoutine());
            }
        }

        // Old
        //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        //{
        //    SceneManager.sceneLoaded -= OnSceneLoaded;

        //    GameObject player = GameObject.FindGameObjectWithTag(GameTags.player);
        //    player.transform.position = spawnPoint.position;
        //    player.transform.rotation = spawnPoint.rotation;
        //}

        private IEnumerator LoadSceneRoutine()
        {
            transform.SetParent(null, false);
            DontDestroyOnLoad(gameObject);
            
            yield return SceneManager.LoadSceneAsync(sceneToLoad.name);

            GetDestinationPortal();
            UpdatePlayer();

            Destroy(gameObject);
        }

        private void GetDestinationPortal()
        {
            Portal[] portals = FindObjectsOfType<Portal>();
            foreach (Portal portal in portals)
            {
                if (portal != this && portal.destination == destination)
                {
                    otherPortal = portal;
                    return;
                }
            }
        }

        private void UpdatePlayer()
        {
            GameObject player = GameObject.FindGameObjectWithTag(GameTags.player);

            if (player.TryGetComponent(out NavMeshAgent agent))
                agent.Warp(otherPortal.spawnPoint.position);
            else
                player.transform.position = otherPortal.spawnPoint.position;

            player.transform.rotation = otherPortal.spawnPoint.rotation;
        }
    }
}