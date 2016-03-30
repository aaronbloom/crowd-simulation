using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Environment.World {

    /// <summary>
    /// The purpose of World is to store all the world objects within a particular Environment, and allow you to query them
    /// </summary>
    public class World {

        public List<WorldObject> Objects { get; private set; }
        public List<Goal> Goals;
        public List<Entrance> Entrances;
        public List<Wall> Walls;
        public List<ToiletFemale> FemaleToilets;
        public List<ToiletMale> MaleToilets;
        public List<Goal> MaleGoalToilets;
        public List<Goal> FemaleGoalToilets;
        public List<Stage> Stages;
        public List<Bar> Bars;
        public List<ICollidable> Collidables;

        /// <summary>
        /// Creates new World object
        /// </summary>
        public World() {
            Objects = new List<WorldObject>();
        }
        
        /// <summary>
        /// Returns all objects of type <typeparamref name="T"/> which are stored in this World
        /// </summary>
        /// <typeparam name="T">The type of world object you wish to return</typeparam>
        /// <returns>The objects within world of type <typeparamref name="T"/></returns>
        public List<T> ObjectSublist<T>() where T : WorldObject {
            return Objects.OfType<T>().ToList();
        }

        /// <summary>
        /// Updates all the cached sublists so queries of this class are executed faster.
        /// </summary>
        /// <remarks>MUST be called if list contents are changed!</remarks>
        public void updateSublists() {
            Goals = ObjectSublist<Goal>();
            Entrances = ObjectSublist<Entrance>();
            Walls = ObjectSublist<Wall>();
            FemaleToilets = ObjectSublist<ToiletFemale>();
            MaleToilets = ObjectSublist<ToiletMale>();
            Stages = ObjectSublist<Stage>();
            Bars = ObjectSublist<Bar>();

            Collidables = Objects.OfType<ICollidable>().ToList();
        }

        /// <summary>
        /// Adds a WorldObject to the world
        /// </summary>
        /// <remarks>Updates sublists too</remarks>
        /// <param name="worldObject">The WorldObject to be added</param>
        /// <returns>true if the object could be added, false if there was already an object at it's position</returns>
        public bool AddObject(WorldObject worldObject) {
            bool alreadyOccupied = AlreadyOccupied(worldObject);
            if (!alreadyOccupied) {
                Objects.Add(worldObject);
                updateSublists();
            } else {
                Object.Destroy(worldObject.GameObject);
            }
            return !alreadyOccupied;
        }

        /// <summary>
        /// Removes the WorldObject from the world
        /// <remarks>Updates sublists too</remarks>
        /// </summary>
        /// <param name="worldObject">Object to remove</param>
        /// <returns>if the object was successfully removed</returns>
        public bool RemoveObject(WorldObject worldObject) {
            bool success = Objects.Remove(worldObject);
            if (success) {
                worldObject.Destroy();
                updateSublists();
            }
            return success;
        }

        /// <summary>
        /// Sees if there's a WorldObject occupying the position: <paramref name="location"/>
        /// </summary>
        /// <param name="location">The position to check</param>
        /// <returns>true if there's already a WorldObject at this point</returns>
        public bool PointAlreadyOccupied(Vector3 location) {
            return Objects.Any(worldObject => worldObject.SamePosition(location));
        }

        /// <summary>
        /// Sees if there's a WorldObject occupying the area: <paramref name="location"/>
        /// </summary>
        /// <param name="location">The position to check</param>
        /// <returns>true if there's already a WorldObject in this area</returns>
        public bool SpaceAlreadyOccupied(Vector3 location)
        {
            return Objects.Any(worldObject => worldObject.WithinBounds(location));
        }

        /// <summary>
        /// Sees if there's a WorldObject occupying the area of WorldObject: <paramref name="worldObject"/>
        /// </summary>
        /// <param name="location">The position to check</param>
        /// <returns>true if there's already a WorldObject in this area</returns>
        public bool AlreadyOccupied(WorldObject worldObject) {
            return Objects.Any(otherWorldObject => otherWorldObject.WithinBounds(worldObject));
        }

        /// <summary>
        /// Checks if the world has one of every WorldObject
        /// </summary>
        /// <returns>True if world contains every type of WorldObject</returns>
        public bool IsValidWorld() {
            return (Entrances.Count > 0) &&
                   (MaleToilets.Count > 0) &&
                   (FemaleToilets.Count > 0) &&
                   (Bars.Count > 0) &&
                   (Stages.Count > 0);
        }

    }
}

