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
        public Vector3 Padding { get; protected set; }

        public GameObject GameObject { get; set; }

        protected WorldObject() {
            InitialPositionOffSet = Vector3.zero;
            InitialRotationOffSet = Quaternion.identity;
            Padding = Vector3.zero; //default
        }

        //basic withinbounds checker, simple AABB collision detection
        public bool WithinBounds(WorldObject worldObject) {
            Vector3 position = GameObject.transform.position;
            Vector3 halfSize = (this.Size + this.Padding) / 2;

            Vector3 otherPosition = worldObject.GameObject.transform.position;
            Vector3 otherHalfSize = (worldObject.Size + worldObject.Padding) / 2;

            var xDifference = MathHelper.Difference(position.x, otherPosition.x);
            var zDifference = MathHelper.Difference(position.z, otherPosition.z);

            var halfWidthSum = halfSize.x + otherHalfSize.x;
            var halfLengthSum = halfSize.z + otherHalfSize.z;

            return xDifference < halfWidthSum && zDifference < halfLengthSum; //AABB
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

        public static WorldObject Initialise(WorldObject worldObject, Vector3 position)
        {
            worldObject.GameObject = (GameObject)BootStrapper.Initialise(
                worldObject.Identifier,
                position + worldObject.InitialPositionOffSet,
                worldObject.InitialRotationOffSet
                );
            return worldObject;
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