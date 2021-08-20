using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float WAYPOINT_GIZMOS_RADIUS = 0.3f;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetPosition(i), WAYPOINT_GIZMOS_RADIUS);
                Gizmos.DrawLine(GetPosition(i), GetNextPosition(i));
            }
        }

        private Vector3 GetNextPosition(int currentIndex)
        {
            int nextIndex = currentIndex + 1;
            if (nextIndex == transform.childCount)
            {
                nextIndex = 0;
            }

            return GetPosition(nextIndex);
        }

        private Vector3 GetPosition(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
