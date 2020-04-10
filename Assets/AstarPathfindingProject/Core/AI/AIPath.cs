using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Pathfinding
{
    using Pathfinding.RVO;
    using Pathfinding.Util;

    [AddComponentMenu("Pathfinding/AI/AIPath (2D,3D)")]
    public partial class AIPath : AIBase, IAstarAI
    {
        public float maxAcceleration = -2.5f;

        [UnityEngine.Serialization.FormerlySerializedAs("turningSpeed")]
        public float rotationSpeed = 360;

        public float slowdownDistance = 0.6F;

        public float pickNextWaypointDist = 2;

        public float endReachedDistance = 0.2F;

        public bool alwaysDrawGizmos;


        public bool slowWhenNotFacingTarget = true;

        public CloseToDestinationMode whenCloseToDestination = CloseToDestinationMode.Stop;

        public bool constrainInsideGraph = false;

        protected Path path;

        protected PathInterpolator interpolator = new PathInterpolator();

        #region IAstarAI implementation

        public override void Teleport(Vector3 newPosition, bool clearPath = true)
        {
            reachedEndOfPath = false;
            base.Teleport(newPosition, clearPath);
        }

        public float remainingDistance
        {
            get
            {
                return interpolator.valid ? interpolator.remainingDistance + movementPlane.ToPlane(interpolator.position - position).magnitude : float.PositiveInfinity;
            }
        }

        public bool reachedDestination
        {
            get
            {
                if (!reachedEndOfPath) return false;
                if (remainingDistance + movementPlane.ToPlane(destination - interpolator.endPoint).magnitude > endReachedDistance) return false;

                if (orientation != OrientationMode.YAxisForward)
                {
                    float yDifference;
                    movementPlane.ToPlane(destination - position, out yDifference);
                    var h = LocalScale.y * height;
                    if (yDifference > h || yDifference < -h * 0.5) return false;
                }

                return true;
            }
        }

        public bool reachedEndOfPath { get; protected set; }

        public bool hasPath
        {
            get
            {
                return interpolator.valid;
            }
        }

        public bool pathPending
        {
            get
            {
                return waitingForPathCalculation;
            }
        }

        public Vector3 steeringTarget
        {
            get
            {
                return interpolator.valid ? interpolator.position : position;
            }
        }

        float IAstarAI.radius { get { return radius; } set { radius = value; } }

        float IAstarAI.height { get { return height; } set { height = value; } }

        float IAstarAI.maxSpeed { get { return maxSpeed; } set { maxSpeed = value; } }

        bool IAstarAI.canSearch { get { return canSearch; } set { canSearch = value; } }

        bool IAstarAI.canMove { get { return canMove; } set { canMove = value; } }

        #endregion

        public void GetRemainingPath(List<Vector3> buffer, out bool stale)
        {
            buffer.Clear();
            buffer.Add(position);
            if (!interpolator.valid)
            {
                stale = true;
                return;
            }

            stale = false;
            interpolator.GetRemainingPath(buffer);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (path != null) path.Release(this);
            path = null;
            interpolator.SetPath(null);
        }

        public virtual void OnTargetReached()
        {
        }
        protected override void OnPathComplete(Path newPath)
        {
            ABPath p = newPath as ABPath;

            if (p == null) throw new System.Exception("This function only handles ABPaths, do not use special path types");

            waitingForPathCalculation = false;

            p.Claim(this);

            if (p.error)
            {
                p.Release(this);
                return;
            }

            if (path != null) path.Release(this);

            path = p;

            if (path.vectorPath.Count == 1) path.vectorPath.Add(path.vectorPath[0]);
            interpolator.SetPath(path.vectorPath);

            var graph = path.path.Count > 0 ? AstarData.GetGraph(path.path[0]) as ITransformedGraph : null;
            movementPlane = graph != null ? graph.transform : (orientation == OrientationMode.YAxisForward ? new GraphTransform(Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(-90, 270, 90), Vector3.one)) : GraphTransform.identityTransform);

            reachedEndOfPath = false;

            interpolator.MoveToLocallyClosestPoint((GetFeetPosition() + p.originalStartPoint) * 0.5f);
            interpolator.MoveToLocallyClosestPoint(GetFeetPosition());

            interpolator.MoveToCircleIntersection2D(position, pickNextWaypointDist, movementPlane);

            var distanceToEnd = remainingDistance;
            if (distanceToEnd <= endReachedDistance)
            {
                reachedEndOfPath = true;
                OnTargetReached();
            }
        }

        protected override void ClearPath()
        {
            CancelCurrentPathRequest();
            interpolator.SetPath(null);
            reachedEndOfPath = false;
        }

        protected override void MovementUpdateInternal(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
        {
            float currentAcceleration = maxAcceleration;

            if (currentAcceleration < 0) currentAcceleration *= -maxSpeed;

            if (updatePosition)
            {
                simulatedPosition = Position;
            }
            if (updateRotation) simulatedRotation = Rotation;

            var currentPosition = simulatedPosition;

            interpolator.MoveToCircleIntersection2D(currentPosition, pickNextWaypointDist, movementPlane);
            var dir = movementPlane.ToPlane(steeringTarget - currentPosition);

            float distanceToEnd = dir.magnitude + Mathf.Max(0, interpolator.remainingDistance);

            var prevTargetReached = reachedEndOfPath;
            reachedEndOfPath = distanceToEnd <= endReachedDistance && interpolator.valid;
            if (!prevTargetReached && reachedEndOfPath) OnTargetReached();
            float slowdown;

            var forwards = movementPlane.ToPlane(simulatedRotation * (orientation == OrientationMode.YAxisForward ? Vector3.up : Vector3.forward));

            if (interpolator.valid && !isStopped)
            {
                slowdown = distanceToEnd < slowdownDistance ? Mathf.Sqrt(distanceToEnd / slowdownDistance) : 1;

                if (reachedEndOfPath && whenCloseToDestination == CloseToDestinationMode.Stop)
                {
                    velocity2D -= Vector2.ClampMagnitude(velocity2D, currentAcceleration * deltaTime);
                }
                else
                {
                    velocity2D += MovementUtilities.CalculateAccelerationToReachPoint(dir, dir.normalized * maxSpeed, velocity2D, currentAcceleration, rotationSpeed, maxSpeed, forwards) * deltaTime;
                }
            }
            else
            {
                slowdown = 1;
                velocity2D -= Vector2.ClampMagnitude(velocity2D, currentAcceleration * deltaTime);
            }

            velocity2D = MovementUtilities.ClampVelocity(velocity2D, maxSpeed, slowdown, slowWhenNotFacingTarget && enableRotation, forwards);

            ApplyGravity(deltaTime);


            // Set how much the agent wants to move during this frame
            var delta2D = lastDeltaPosition = CalculateDeltaToMoveThisFrame(movementPlane.ToPlane(currentPosition), distanceToEnd, deltaTime);
            nextPosition = currentPosition + movementPlane.ToWorld(delta2D, verticalVelocity * lastDeltaTime);
            CalculateNextRotation(slowdown, out nextRotation);
        }

        protected virtual void CalculateNextRotation(float slowdown, out Quaternion nextRotation)
        {
            if (lastDeltaTime > 0.00001f && enableRotation)
            {
                Vector2 desiredRotationDirection;
                desiredRotationDirection = velocity2D;

                var currentRotationSpeed = rotationSpeed * Mathf.Max(0, (slowdown - 0.3f) / 0.7f);
                nextRotation = SimulateRotationTowards(desiredRotationDirection, currentRotationSpeed * lastDeltaTime);
            }
            else
            {
                nextRotation = rotation;
            }
        }

        static NNConstraint cachedNNConstraint = NNConstraint.Default;
        protected override Vector3 ClampToNavmesh(Vector3 position, out bool positionChanged)
        {
            if (constrainInsideGraph)
            {
                cachedNNConstraint.tags = seeker.traversableTags;
                cachedNNConstraint.graphMask = seeker.graphMask;
                cachedNNConstraint.distanceXZ = true;
                var clampedPosition = AstarPath.active.GetNearest(position, cachedNNConstraint).position;

                var difference = movementPlane.ToPlane(clampedPosition - position);
                float sqrDifference = difference.sqrMagnitude;
                if (sqrDifference > 0.001f * 0.001f)
                {
                    velocity2D -= difference * Vector2.Dot(difference, velocity2D) / sqrDifference;

                    positionChanged = true;
                    return position + movementPlane.ToWorld(difference);
                }
            }

            positionChanged = false;
            return position;
        }

#if UNITY_EDITOR
        [System.NonSerialized]
        int gizmoHash = 0;

        [System.NonSerialized]
        float lastChangedTime = float.NegativeInfinity;

        protected static readonly Color GizmoColor = new Color(46.0f / 255, 104.0f / 255, 201.0f / 255);

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (alwaysDrawGizmos) OnDrawGizmosInternal();
        }

        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            if (!alwaysDrawGizmos) OnDrawGizmosInternal();
        }

        void OnDrawGizmosInternal()
        {
            var newGizmoHash = pickNextWaypointDist.GetHashCode() ^ slowdownDistance.GetHashCode() ^ endReachedDistance.GetHashCode();

            if (newGizmoHash != gizmoHash && gizmoHash != 0) lastChangedTime = Time.realtimeSinceStartup;
            gizmoHash = newGizmoHash;
            float alpha = alwaysDrawGizmos ? 1 : Mathf.SmoothStep(1, 0, (Time.realtimeSinceStartup - lastChangedTime - 5f) / 0.5f) * (UnityEditor.Selection.gameObjects.Length == 1 ? 1 : 0);

            if (alpha > 0)
            {
                if (!alwaysDrawGizmos) UnityEditor.SceneView.RepaintAll();
                Draw.Gizmos.Line(position, steeringTarget, GizmoColor * new Color(1, 1, 1, alpha));
                Gizmos.matrix = Matrix4x4.TRS(position, transform.rotation * (orientation == OrientationMode.YAxisForward ? Quaternion.Euler(-90, 0, 0) : Quaternion.identity), Vector3.one);
                Draw.Gizmos.CircleXZ(Vector3.zero, pickNextWaypointDist, GizmoColor * new Color(1, 1, 1, alpha));
                Draw.Gizmos.CircleXZ(Vector3.zero, slowdownDistance, Color.Lerp(GizmoColor, Color.red, 0.5f) * new Color(1, 1, 1, alpha));
                Draw.Gizmos.CircleXZ(Vector3.zero, endReachedDistance, Color.Lerp(GizmoColor, Color.red, 0.8f) * new Color(1, 1, 1, alpha));
            }
        }
#endif

        protected override int OnUpgradeSerializedData(int version, bool unityThread)
        {
            base.OnUpgradeSerializedData(version, unityThread);
            if (version < 1) rotationSpeed *= 90;
            return 2;
        }
    }
}
