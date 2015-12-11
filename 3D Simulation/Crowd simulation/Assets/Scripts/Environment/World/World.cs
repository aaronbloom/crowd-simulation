using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Environment.World {
    public class World {

        public List<WorldObject> Objects { get; private set; }
        public List<Goal> Goals {
            get {
                return ObjectSublist<Goal>();
            }
        }
        public List<Entrance> Entrances {
            get {
                return ObjectSublist<Entrance>();
            }
        }
        public List<Exit> Exits {
            get {
                return ObjectSublist<Exit>();
            }
        }

        public List<Wall> Walls {
            get {
                return ObjectSublist<Wall>();
            }
        }

        public List<Toilet> Toilets {
            get {
                return ObjectSublist<Toilet>();
            }
        }

        public List<Collidable> Collidables {
            get {
                return Objects.OfType<Collidable>().ToList();
            }
        }

        public World() {
            Objects = new List<WorldObject>();
        }
        
        public List<T> ObjectSublist<T>() where T : WorldObject {
            return Objects.OfType<T>().ToList();
        }

        public bool AddObject(WorldObject worldObject) {
            bool alreadyOccupied = AlreadyOccupied(worldObject.GameObject.transform.position);
            if (!alreadyOccupied) {
                Objects.Add(worldObject);
            }
            return !alreadyOccupied;
        }

        public bool AlreadyOccupied(Vector3 location) {
            foreach (WorldObject worldObject in Objects) {
                if (worldObject.WithinBounds(location)) {
                    return true;
                }
            }
            return false;
        }

    }
}

