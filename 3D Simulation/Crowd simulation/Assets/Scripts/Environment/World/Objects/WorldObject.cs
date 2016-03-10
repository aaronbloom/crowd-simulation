using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Environment.World.Objects {
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
        public Quaternion InitialRotationOffSet { get; protected set; }
        public Vector3 InitialPositionOffSet { get; protected set; }
        public Vector3 Size { get; protected set; }
        public Vector3 FacingDirection { get; protected set; } //The normalised direction vector for the object front face
        public int FrontPadding { get; protected set; } //Amount from front to pad the object size, int 4 => 4 spaces in front of face

        public GameObject GameObject { get; set; }

        protected WorldObject() {
            InitialPositionOffSet = Vector3.zero;
            InitialRotationOffSet = Quaternion.identity;
            FacingDirection = Vector3.forward;
            FrontPadding = 0;
        }

        //basic withinbounds checker, simple AABB collision detection
        public bool WithinBounds(WorldObject worldObject) {
            Vector3 position = GameObject.transform.position;
            Vector3 halfSize = this.Size / 2;

            Vector3 otherPosition = worldObject.GameObject.transform.position;
            Vector3 otherHalfSize = worldObject.Size / 2;

            var xDifference = MathHelper.Difference(position.x, otherPosition.x);
            var zDifference = MathHelper.Difference(position.z, otherPosition.z);

            var halfWidthSum = halfSize.x + otherHalfSize.x;
            var halfLengthSum = halfSize.z + otherHalfSize.z;

            return xDifference < halfWidthSum && zDifference < halfLengthSum; //AABB
        }

        //Return true if arg within WorldObject bounds
        public bool WithinBounds(Vector3 targetPosition) {
            Vector3 position = GameObject.transform.position;
            Vector3 halfSize = this.Size / 2;

            bool x = targetPosition.x <= (position.x + halfSize.x) && targetPosition.x >= (position.x - halfSize.x);
            bool z = targetPosition.z <= (position.z + halfSize.z) && targetPosition.z >= (position.z - halfSize.z);

            return x && z;
        }

        public bool SamePosition(Vector3 position) {
            Vector3 gameObjectPosition = GameObject.transform.position;
            gameObjectPosition.y = 0;
            position.y = 0;
            return gameObjectPosition == position;
        }

        public void Destroy() {
            Object.Destroy(GameObject);
        }

        public void LookTowardsNormal(Vector3 normal) {
            this.GameObject.transform.forward = normal;
            this.GameObject.transform.rotation *= this.InitialRotationOffSet;
            AdjustSizing(normal);
        }

        public void AdjustSizing(Vector3 direction) {
            float dotProduct = Vector3.Dot(this.FacingDirection, direction);

            if (dotProduct == 0) { //perpendicular
                this.Size = new Vector3(this.Size.z, this.Size.y, this.Size.x); //swap dimensions
            }

            this.FacingDirection = direction;
        }

        //A position in-front of the front of the object, inc. any front padding
        public Vector3 FrontPosition() {
            return this.GameObject.transform.position
                + Vector3.Scale(this.FacingDirection, this.Size/2) //The front face
                + this.FrontPadding * this.FacingDirection; //The front inc. front padding
        }

        public static WorldObject Initialise(WorldObject worldObject, Vector3 position)
        {
            worldObject.GameObject = (GameObject)BootStrapper.Initialise(
                worldObject.Identifier,
                position + worldObject.InitialPositionOffSet,
                worldObject.InitialRotationOffSet
                );
            return worldObject;
        }

        public void ChangePrefab(string identifer)
        {
            GameObject toDestroy = this.GameObject;
            var rotation = this.GameObject.transform.rotation;
            var position = this.GameObject.transform.position;
            Object.Destroy(toDestroy);
            this.GameObject = (GameObject) BootStrapper.Initialise(
                identifer,
                position,
                rotation);
        }

        public static WorldObject DetermineObject(string objectName) {
            switch (objectName) {
                case Wall.IdentifierStatic:
                    return new Wall();
                case Entrance.IdentifierStatic:
                    return new Entrance();
                case Toilet.IdentifierStatic:
                    return new Toilet();
                case Stage.IdentifierStatic:
                    return new Stage();
                case Bar.IdentifierStatic:
                    return new Bar();
            }
            return null;
        }
    }
}