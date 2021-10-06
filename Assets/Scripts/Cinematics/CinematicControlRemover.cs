using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using RPG.Core;
using RPG.Control;

namespace RPG.Cinematics
{
    [RequireComponent(typeof(PlayableDirector))]
    public class CinematicControlRemover : MonoBehaviour
    {
        const string playerTag = "Player";

        PlayableDirector director;
        GameObject player;

        private void Awake()
        {
            director = GetComponent<PlayableDirector>();
            player = GameObject.FindGameObjectWithTag(playerTag);
        }

        private void OnEnable()
        {
            director.played += DisablePlayerControl;
            director.stopped += EnablePlayerControl;
        }

        private void OnDisable()
        {
            director.played -= DisablePlayerControl;
            director.stopped -= EnablePlayerControl;
        }

        private void EnablePlayerControl(PlayableDirector _)
        {
            player.GetComponent<PlayerController>().enabled = true;
        }

        private void DisablePlayerControl(PlayableDirector _)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }
    }
}