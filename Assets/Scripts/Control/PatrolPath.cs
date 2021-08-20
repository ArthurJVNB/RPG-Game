using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float WAYPOINT_GIZMOS_RADIUS = 0.3f;

        public int WaypointCount
        {
            get { return transform.childCount; }
        }

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.DrawSphere(GetPosition(i), WAYPOINT_GIZMOS_RADIUS);
                Gizmos.DrawLine(GetPosition(i), GetNextPosition(i));
            }
        }

        public int GetNextIndex(int currentIndex)
        {
            int nextIndex = currentIndex + 1;
            if (nextIndex == transform.childCount)
            {
                nextIndex = 0;
            }

            return nextIndex;
        }

        public Vector3 GetNextPosition(int currentIndex)
        {
            return GetPosition(GetNextIndex(currentIndex));
        }

        public Vector3 GetPosition(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
