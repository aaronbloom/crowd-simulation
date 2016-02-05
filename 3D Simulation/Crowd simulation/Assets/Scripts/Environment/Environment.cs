﻿using Assets.Scripts.Environment.Navigation;
using Assets.Scripts.Environment.World;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Environment {
    public class Environment {

        public World.World World { get; private set; }
        public Graph Graph { get; private set; }
        public Vector3 Bounds { get; private set; }
        public Plane[] Boundaries { get; private set; }
        public Vector3 Origin { get; private set; }
        public Vector3 EnvironmentCenter {
            get {
                return (Bounds / 2) + Origin;
            }
        }
        public Vector2 FloorDimentions {
            get {
                return new Vector2(Bounds.x - Origin.x, Bounds.z - Origin.z);
            }
        }


        public Environment(Vector3 bounds) {
            this.Origin = Vector3.zero;
            this.Bounds = bounds; //width, height, length
            CreateGroundArea(bounds);
            this.Boundaries = constructCuboidBoundary(this.Bounds, Origin);
            this.World = new World.World();
        }

        public void Build() {
            constructNavMesh();
        }

        public static Vector3 ConstrainVector(Vector3 position, Vector3 origin, Vector3 bounds, Vector3 halfObjectSize)
        {
            position.x = Mathf.Clamp(position.x, origin.x + halfObjectSize.x, origin.x + bounds.x - halfObjectSize.x);
            position.y = Mathf.Clamp(position.y, origin.y + halfObjectSize.z, origin.y + bounds.y - halfObjectSize.y);
            position.z = Mathf.Clamp(position.z, origin.z + halfObjectSize.z, origin.z + bounds.z - halfObjectSize.z);
            return position;
        }

        public void Place(WorldObject worldObject, Vector3 position)
        {
            var location = PositionToGridPosition(position, worldObject.Size);
            if (!World.AddObject(WorldObject.Initialise(worldObject, location)))
            {
                Debug.Log("Could not add new world object - Already occupied");
                worldObject.Destroy();
            }
        }

        public static Vector3 PositionToGridPosition(Vector3 position, Vector3 objectSize)
        {
            var gridPosition = position;
            gridPosition -= (objectSize / 2);
            gridPosition = new Vector3(
                Mathf.Round(gridPosition.x / objectSize.x) * objectSize.x,
                0, // so it sits at ground level
                Mathf.Round(gridPosition.z / objectSize.z) * objectSize.z);
            gridPosition += (objectSize / 2);
            Vector3 bounds = EnvironmentManager.Shared().CurrentEnvironment.Bounds;
            Vector3 origin = EnvironmentManager.Shared().CurrentEnvironment.Origin;
            return ConstrainVector(gridPosition, origin, bounds, objectSize / 2);
        }

        private void CreateGroundArea(Vector3 bounds) {
            Vector3 position = bounds * 0.5f;
            position.y = 0.1f; //set to ground level
            GameObject groundArea = (GameObject)BootStrapper.Initialise("GroundQuad", position, Quaternion.Euler(90, 0, 0));
            groundArea.transform.localScale = new Vector3(bounds.x, bounds.z, bounds.y); //swapped axis due to quad rotation
        }

        private void constructNavMesh() {
            this.Graph = Graph.ConstructGraph(this, 1f);
            foreach (var collidable in World.Collidables) {
                this.Graph.Cull(collidable);
            }
        }

        private Plane[] constructCuboidBoundary(Vector3 bounds, Vector3 origin) {
            Plane[] boundaries = new Plane[6];
            boundaries[0] = new Plane(Vector3.up, origin);                  //bottom (ground) plane
            boundaries[1] = new Plane(Vector3.forward, origin);             //front
            boundaries[2] = new Plane(Vector3.right, origin);               //left
            boundaries[3] = new Plane(Vector3.down, origin + bounds);       //top
            boundaries[4] = new Plane(Vector3.back, origin + bounds);       //back
            boundaries[5] = new Plane(Vector3.left, origin + bounds);       //right

            return boundaries;
        }
    }
}
