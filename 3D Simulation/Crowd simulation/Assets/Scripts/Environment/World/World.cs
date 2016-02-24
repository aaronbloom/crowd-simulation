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
        public List<Toilet> Toilets;
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
            Toilets = ObjectSublist<Toilet>();
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
                updateSublists();
            }
            return success;
        }

        public bool AlreadyOccupied(Vector3 location) {
            foreach (WorldObject worldObject in Objects) {
                if (worldObject.SamePosition(location)) {
                    return true;
                }
            }
            return false;
        }


        public bool AlreadyOccupied(WorldObject worldObject) {
            foreach (WorldObject otherWorldObject in Objects) {
                if (otherWorldObject.WithinBounds(worldObject)) {
                    return true;
                }
            }
            return false;
        }

    }
}

