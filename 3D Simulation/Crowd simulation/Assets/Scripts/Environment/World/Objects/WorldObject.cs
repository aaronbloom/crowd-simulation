using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Environment.World.Objects {

    /// <summary>
    /// World object class
    /// </summary>
    public abstract class WorldObject {

        private string _identifer = String.Empty;

        public string Identifier {
            get { return _identifer; }
            set {
                if (_identifer == String.Empty) {
                    _identifer = value;
                }
            }
        }

        public bool GridPlaceable { get; protected set; }
        public Vector3 CursorHeight { get; protected set; }
        public Quaternion InitialRotationOffSet { get; protected set; }
        public Vector3 InitialPositionOffSet { get; protected set; }
        public Vector3 Size { get; protected set; }
        public Vector3 FacingDirection { get; protected set; } //The normalised direction vector for the object front face
        public int FrontPadding { get; protected set; } //Amount from front to pad the object size, int 4 => 4 spaces in front of face
        public Vector3 InitialWallNormal { get; set; }

        public GameObject GameObject { get; set; }

        /// <summary>
        /// World object constructor
        /// </summary>
        protected WorldObject() {
            InitialPositionOffSet = Vector3.zero;
            InitialRotationOffSet = Quaternion.identity;
            FacingDirection = Vector3.forward;
            FrontPadding = 0;
            CursorHeight = new Vector3(0, 0.1f, 0);
        }

        /// <summary>
        /// Basic withinbounds checker, simple AABB collision detection
        /// </summary>
        /// <param name="worldObject">Another world object</param>
        /// <returns>If <paramref name="worldObject"/> is overlapping</returns>
        public bool WithinBounds(WorldObject worldObject) {
            Vector3 position = GameObject.transform.position;
            Vector3 halfSize = this.Size / 2;

            Vector3 otherPosition = worldObject.GameObject.transform.position;
            Vector3 otherHalfSize = worldObject.Size / 2;

            var xDifference = Mathf.Abs(position.x - otherPosition.x);
            var zDifference = Mathf.Abs(position.z - otherPosition.z);

            var halfWidthSum = halfSize.x + otherHalfSize.x;
            var halfLengthSum = halfSize.z + otherHalfSize.z;

            return (xDifference < halfWidthSum) && (zDifference < halfLengthSum); //AABB
        }

        /// <summary>
        /// Return true if <paramref name="targetPosition"/> within WorldObject bounds
        /// </summary>
        /// <param name="targetPosition">A position</param>
        /// <returns>Is <paramref name="targetPosition"/> within bounds</returns>
        public bool WithinBounds(Vector3 targetPosition) {
            Vector3 position = GameObject.transform.position;
            Vector3 halfSize = this.Size / 2;

            bool x = (targetPosition.x <= (position.x + halfSize.x)) && (targetPosition.x >= (position.x - halfSize.x));
            bool z = (targetPosition.z <= (position.z + halfSize.z)) && (targetPosition.z >= (position.z - halfSize.z));

            return x && z;
        }

        /// <summary>
        /// Is a <paramref name="position"/> exactly the same as this objects position
        /// </summary>
        /// <param name="position">Another position</param>
        /// <returns>Are they the same position</returns>
        public bool SamePosition(Vector3 position) {
            Vector3 gameObjectPosition = GameObject.transform.position;
            gameObjectPosition.y = 0;
            position.y = 0;
            return gameObjectPosition == position;
        }

        /// <summary>
        /// Destroy this world objects game object
        /// </summary>
        public void Destroy() {
            Object.Destroy(GameObject);
        }

        /// <summary>
        /// Rotate the game object to face a direction
        /// </summary>
        /// <param name="normal">Facing direction</param>
        public void LookTowardsNormal(Vector3 normal) {
            this.GameObject.transform.forward = normal;
            this.GameObject.transform.rotation *= this.InitialRotationOffSet;
            AdjustSizing(normal);
        }

        /// <summary>
        /// Adjust the object size based upon facing direction
        /// </summary>
        /// <param name="direction">Facing direction</param>
        public void AdjustSizing(Vector3 direction) {
            float dotProduct = Vector3.Dot(this.FacingDirection, direction);

            if (dotProduct == 0) { //perpendicular
                this.Size = new Vector3(this.Size.z, this.Size.y, this.Size.x); //swap dimensions
            }

            this.FacingDirection = direction;
        }

        /// <summary>
        /// A position in-front of the front of the object
        /// </summary>
        /// <returns>Front of object position</returns>
        public Vector3 FrontPosition() {
            return this.GameObject.transform.position
                   + Vector3.Scale(this.FacingDirection, this.Size/2); //The front face
        }

        /// <summary>
        /// Initialise the game object into the environment
        /// </summary>
        /// <param name="worldObject">The world object to initialise</param>
        /// <param name="position">Position within the environment</param>
        /// <param name="wallNormal">The facing direction</param>
        /// <returns>Initialised WorldObject</returns>
        public static WorldObject Initialise(WorldObject worldObject, Vector3 position, Vector3 wallNormal) {
            worldObject.InitialWallNormal = Vector3.zero;
            worldObject.GameObject = (GameObject)BootStrapper.Initialise(
                worldObject.Identifier,
                position + worldObject.InitialPositionOffSet,
                worldObject.InitialRotationOffSet
                );
            return worldObject;
        }

        /// <summary>
        /// Changes the GameObejct model
        /// </summary>
        /// <param name="identifer">Replacement GameObject identifer</param>
        public void ChangePrefab(string identifer) {
            GameObject toDestroy = this.GameObject;
            var rotation = this.GameObject.transform.rotation;
            var position = this.GameObject.transform.position;
            Object.Destroy(toDestroy);
            this.GameObject = (GameObject) BootStrapper.Initialise(
                identifer,
                position,
                rotation);
        }

        /// <summary>
        /// Creates new world object based upon identifier
        /// </summary>
        /// <param name="objectName">World object identifier</param>
        /// <returns>A new instance of world object</returns>
        public static WorldObject DetermineObject(string objectName) {
            switch (objectName) {
                case Wall.IdentifierStatic:
                    return new Wall();
                case Entrance.IdentifierStatic:
                    return new Entrance();
                case ToiletMale.IdentifierStatic:
                    return new ToiletMale();
                case ToiletFemale.IdentifierStatic:
                    return new ToiletFemale();
                case Stage.IdentifierStatic:
                    return new Stage();
                case Bar.IdentifierStatic:
                    return new Bar();
            }
            return null;
        }
    }
}