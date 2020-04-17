using UnityEngine;
using System.Collections;

namespace Pathfinding
{

    public class AIDestinationSetter : VersionedMonoBehaviour
    {
        public Transform target;

        public Vector3 Target;
        public bool AI_Switch =false;
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
            if (AI_Switch && ai != null)
                ai.destination = Target;
            else
                ai.destination=ai.position;
        }
    }
}
