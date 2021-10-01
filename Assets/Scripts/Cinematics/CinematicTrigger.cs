using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        [SerializeField] bool triggerOnce = true;
        
        readonly string playerTag = "Player";
        bool triggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (triggered) return;

            if (other.CompareTag(playerTag))
            {
                GetComponent<PlayableDirector>().Play();
                
                if (triggerOnce) triggered = true;
            }
        }
    } 
}
