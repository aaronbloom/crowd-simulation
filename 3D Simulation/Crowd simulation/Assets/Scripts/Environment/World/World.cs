using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Environment.World {
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
        public List<Collidable> Collidables;

        public World() {
            Objects = new List<WorldObject>();
        }

        public List<T> ObjectSublist<T>() where T : WorldObject {
            return Objects.OfType<T>().ToList();
        }

        //MUST be called if list contents are changed!
        public void updateSublists() {
            Goals = ObjectSublist<Goal>();
            Entrances = ObjectSublist<Entrance>();
            Walls = ObjectSublist<Wall>();
            FemaleToilets = ObjectSublist<ToiletFemale>();
            MaleToilets = ObjectSublist<ToiletMale>();
            Stages = ObjectSublist<Stage>();
            Bars = ObjectSublist<Bar>();

            Collidables = Objects.OfType<Collidable>().ToList();
        }

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

        public bool RemoveObject(WorldObject worldObject) {
            bool success = Objects.Remove(worldObject);
            if (success) {
                worldObject.Destroy();
                updateSublists();
            }
            return success;
        }

        public bool PointAlreadyOccupied(Vector3 location) {
            foreach (WorldObject worldObject in Objects) {
                if (worldObject.SamePosition(location)) {
                    return true;
                }
            }
            return false;
        }

        public bool SpaceAlreadyOccupied(Vector3 location)
        {
            return Objects.Any(worldObject => worldObject.WithinBounds(location));
        }


        public bool AlreadyOccupied(WorldObject worldObject) {
            foreach (WorldObject otherWorldObject in Objects) {
                if (otherWorldObject.WithinBounds(worldObject)) {
                    return true;
                }
            }
            return false;
        }

        public bool IsValidWorld() {
            return Entrances.Count > 0 &&
                   MaleToilets.Count > 0 &&
                   FemaleToilets.Count > 0 &&
                   Bars.Count > 0 &&
                   Stages.Count > 0;
        }

    }
}

