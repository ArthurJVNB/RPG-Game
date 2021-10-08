using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [SerializeField] Object sceneToLoad;
        [SerializeField] Transform spawnPoint;
        [Tooltip("For it to be found when another portal tries to find that one.")]
        [SerializeField] int id = -1;
        [Tooltip("ID of the portal it will try to find when the new scene is loaded.")]
        [SerializeField] int toId = -1;

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

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            GameObject player = GameObject.FindGameObjectWithTag(GameTags.player);
            player.transform.position = spawnPoint.position;
            player.transform.rotation = spawnPoint.rotation;
        }

        IEnumerator LoadSceneRoutine()
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            
            yield return SceneManager.LoadSceneAsync(sceneToLoad.name);

            FindPortal();
            UpdatePlayer();

            Destroy(gameObject);
        }

        private void FindPortal()
        {
            Portal[] portals = FindObjectsOfType<Portal>();
            foreach (Portal portal in portals)
            {
                if (portal.id == toId)
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