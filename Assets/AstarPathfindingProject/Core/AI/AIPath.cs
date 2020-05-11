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
                return interpolator.valid ? interpolator.remainingDistance + (interpolator.position - position).magnitude : float.PositiveInfinity;
            }
        }

        public bool reachedDestination
        {
            get
            {
                if (!reachedEndOfPath) return false;
                if (remainingDistance +(destination - interpolator.endPoint).magnitude > endReachedDistance) return false;

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
            // Debug.Log("GetRemainingPath"); is not being used
            // buffer.Clear();
            // buffer.Add(position);
            // if (!interpolator.valid)
            // {
            //     stale = true;
            //     return;
            // }

            stale = false;
            // interpolator.GetRemainingPath(buffer);
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
            // Debug.Log("OnPathComplete"); is being used
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
                // OnTargetReached();
            }
        }

        protected override void ClearPath()
        {
            // Debug.Log("ClearPath"); not being used
            // CancelCurrentPathRequest();
            // interpolator.SetPath(null);
            // reachedEndOfPath = false;
        }

        protected override void MovementUpdateInternal(float deltaTime, out Vector3 nextPosition, out Quaternion nextRotation)
        {
            // Debug.Log("MovementUpdateInternal"); is being used
            float currentAcceleration = maxAcceleration;

            if (currentAcceleration < 0) currentAcceleration *= -maxSpeed;

            if (updatePosition)
            {
                simulatedPosition = Position;
                // Debug.Log("monster pos " + Position);
            }
            if (updateRotation) simulatedRotation = Rotation;

            var currentPosition = simulatedPosition;

            interpolator.MoveToCircleIntersection2D(currentPosition, pickNextWaypointDist, movementPlane);
            var dir = (steeringTarget - currentPosition);

            float distanceToEnd = dir.magnitude + Mathf.Max(0, interpolator.remainingDistance);

            var prevTargetReached = reachedEndOfPath;
            reachedEndOfPath = distanceToEnd <= endReachedDistance && interpolator.valid;
            if (!prevTargetReached && reachedEndOfPath) OnTargetReached();
            float slowdown;

            var forwards = (simulatedRotation * (orientation == OrientationMode.YAxisForward ? Vector3.up : Vector3.forward));

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

            // ApplyGravity(deltaTime);


            // Set how much the agent wants to move during this frame
            Vector3 delta2D = lastDeltaPosition = CalculateDeltaToMoveThisFrame((currentPosition), distanceToEnd, deltaTime);
            nextPosition = currentPosition +(delta2D);
            CalculateNextRotation(slowdown, out nextRotation);
        }

        protected virtual void CalculateNextRotation(float slowdown, out Quaternion nextRotation)
        {
            // Debug.Log("calculateNextRotation"); is being used
            if (lastDeltaTime > 0.00001f && enableRotation)
            {
                // Debug.Log(Here will not be called or error occur);
                Vector2 desiredRotationDirection;
                desiredRotationDirection = velocity2D;

                var currentRotationSpeed = rotationSpeed * Mathf.Max(0, (slowdown - 0.3f) / 0.7f);
                nextRotation = SimulateRotationTowards(desiredRotationDirection, currentRotationSpeed * lastDeltaTime);
            }
            else
            {
                // Debug.Log(Only here will be called);
                nextRotation = rotation;
            }
        }

        static NNConstraint cachedNNConstraint = NNConstraint.Default;


    }
}