using UnityEngine;
using System.Collections;

namespace Pathfinding
{

    public class AIDestinationSetter : VersionedMonoBehaviour
    {
        public Transform target;
        public GameObject targetObj;
        public Vector3 Target;
        public bool AI_Switch =false;
        IAstarAI ai;

        void OnEnable()
        {
            ai = GetComponent<IAstarAI>();
            // if (ai != null) ai.onSearchPath += FakeFixedUpdate;
        }

        void OnDisable()
        {
            // if (ai != null) ai.onSearchPath -= FakeFixedUpdate;
        }

        public void FakeFixedUpdate()
        {
            // Debug.Log("Target is " + target)
            if (AI_Switch && ai != null)
                ai.destination = Target;
            else
                ai.destination=ai.position;
        }
    }
}
