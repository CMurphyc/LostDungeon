using UnityEngine;
using System.Collections;

namespace Pathfinding
{

    public class AIDestinationSetter : VersionedMonoBehaviour
    {
        public Transform target;
        IAstarAI ai;

        void OnEnable()
        {
            ai = GetComponent<IAstarAI>();
            if (ai != null) ai.onSearchPath += Update;
        }

        void OnDisable()
        {
            if (ai != null) ai.onSearchPath -= Update;
        }

        void Update()
        {
            if (target != null && ai != null) ai.destination = target.position;
        }
    }
}
