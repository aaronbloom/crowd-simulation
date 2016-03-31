using Assets.Scripts.Environment.Navigation;
using Assets.Scripts.Environment.Save;
using Assets.Scripts.Environment.World.Objects;
using Assets.Scripts.UserInterface;
using UnityEngine;

namespace Assets.Scripts.Environment {

    /// <summary>
    /// A class to represent the environment
    /// </summary>
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

        /// <summary>
        /// Creates a new Environment with the bounds: <paramref name="bounds"/>
        /// </summary>
        /// <param name="bounds">the boundaries of the environment</param>
        public Environment(Vector3 bounds) {
            this.Origin = Vector3.zero;
            this.Bounds = bounds; //width, height, length
            CreateGroundArea(bounds);
            this.Boundaries = constructCuboidBoundary(this.Bounds, Origin);
            this.World = new World.World();
        }

        /// <summary>
        /// Constrains position to within environment
        /// </summary>
        /// <param name="position">position to constrain</param>
        /// <param name="origin">origin of environment</param>
        /// <param name="bounds">bounds of environment</param>
        /// <param name="objectSize">object size to constrain</param>
        /// <returns>constrained Vector</returns>
        public static Vector3 ConstrainVector(Vector3 position, Vector3 origin, Vector3 bounds, Vector3 objectSize) {
            var halfObjectSize = objectSize / 2;
            position.x = Mathf.Clamp(position.x, origin.x + halfObjectSize.x, origin.x + bounds.x - halfObjectSize.x);
            position.y = Mathf.Clamp(position.y, origin.y, origin.y + bounds.y - halfObjectSize.y);
            position.z = Mathf.Clamp(position.z, origin.z + halfObjectSize.z, origin.z + bounds.z - halfObjectSize.z);
            return position;
        }

        /// <summary>
        /// Constrains position to within environment
        /// </summary>
        /// <param name="position">position to constrain</param>
        /// <param name="objectSize">object size to constrain</param>
        /// <returns>constrained Vector</returns>
        public static Vector3 ConstrainVectorToEnvironment(Vector3 position, Vector3 objectSize) {
            Vector3 bounds = EnvironmentManager.Shared().CurrentEnvironment.Bounds;
            Vector3 origin = EnvironmentManager.Shared().CurrentEnvironment.Origin;
            return ConstrainVector(position, origin, bounds, objectSize);
        }

        /// <summary>
        /// constrains position to within environment on the ground
        /// </summary>
        /// <param name="position">position to constrain</param>
        /// <param name="objectSize">object size to constrain</param>
        /// <returns>constrained Vector</returns>
        public static Vector3 PositionToLocation(Vector3 position, Vector3 objectSize) {
            var location = new Vector3(position.x, objectSize.y / 2, position.z);
            return ConstrainVectorToEnvironment(location, objectSize);
        }

        /// <summary>
        /// Snaps a position to a grid location
        /// </summary>
        /// <param name="position">position to snap</param>
        /// <param name="objectSize">grid fidelity</param>
        /// <returns>snapped vector</returns>
        public static Vector3 PositionToGridLocation(Vector3 position, Vector3 objectSize) {
            var gridPosition = position;
            gridPosition -= (objectSize / 2);
            gridPosition = new Vector3(
                Mathf.Round(gridPosition.x / objectSize.x) * objectSize.x,
                0, // so it sits at ground level
                Mathf.Round(gridPosition.z / objectSize.z) * objectSize.z);
            gridPosition += (objectSize / 2);
            return ConstrainVectorToEnvironment(gridPosition, objectSize);
        }

        /// <summary>
        /// Sets up Environment with a perimeter wall
        /// </summary>
        public void Setup() {
            new WorldBuilderPlacement().PlacePerimeterWall(Origin, Bounds);
        }

        /// <summary>
        /// Constructs the nav mesh
        /// </summary>
        public void Build() {
            constructNavMesh();
        }

        /// <summary>
        /// Places the <paramref name="worldObject"/> at <paramref name="position"/>
        /// </summary>
        /// <param name="worldObject">the object to place</param>
        /// <param name="position">the position to place it</param>
        public void Place(WorldObject worldObject, Vector3 position) {
            Vector3 location;
            if (worldObject.GridPlaceable) {
                location = PositionToGridLocation(position, worldObject.Size);
            } else {
                location = PositionToLocation(position, worldObject.Size);
            }
            World.AddObject(WorldObject.Initialise(worldObject, location, Vector3.zero));
        }

        /// <summary>
        /// Draws graph debugging lines
        /// </summary>
        public void OnDrawGizmos() {
            if (Graph != null)
                Graph.DrawGraphGizmo();
        }

        /// <summary>
        /// Saves the environment to a file
        /// </summary>
        public void SaveEnvironment() {
            var savedEnvironment = new SaveableEnvironment(this.Bounds);
            savedEnvironment.SaveWorldObjects(World.Objects);
            SystemSaveFolder.WriteObjectToFolder(SystemSaveFolder.WorldSaveName, savedEnvironment);
        }

        /// <summary>
        /// Creates a ground with the bounds given by <paramref name="bounds"/>
        /// </summary>
        /// <param name="bounds">the size of the ground</param>
        private static void CreateGroundArea(Vector3 bounds) {
            Vector3 position = bounds * 0.5f;
            position.y = 0; //set to ground level
            GameObject groundArea = (GameObject)BootStrapper.Initialise("GroundQuad", position, Quaternion.Euler(90, 0, 0));
            groundArea.transform.localScale = new Vector3(bounds.x, bounds.z, bounds.y); //swapped axis due to quad rotation
        }

        /// <summary>
        /// creates a boundary for flocking behaviour
        /// </summary>
        /// <param name="bounds">bounds for the flocking behaviour</param>
        /// <param name="origin">centre of the environment</param>
        /// <returns>the planes for the flocking boids to avoid</returns>
        private static Plane[] constructCuboidBoundary(Vector3 bounds, Vector3 origin) {
            Plane[] boundaries = new Plane[6];
            boundaries[0] = new Plane(Vector3.up, origin);                  //bottom (ground) plane
            boundaries[1] = new Plane(Vector3.forward, origin);             //front
            boundaries[2] = new Plane(Vector3.right, origin);               //left
            boundaries[3] = new Plane(Vector3.down, origin + bounds);       //top
            boundaries[4] = new Plane(Vector3.back, origin + bounds);       //back
            boundaries[5] = new Plane(Vector3.left, origin + bounds);       //right

            return boundaries;
        }

        /// <summary>
        /// constructs a node lattice graph that fills the environment floor
        /// </summary>
        private void constructNavMesh() {
            this.Graph = Graph.ConstructGraph(this, 1f);
            foreach (var collidable in World.Collidables) {
                this.Graph.Cull(collidable);
            }
        }
    }
}
