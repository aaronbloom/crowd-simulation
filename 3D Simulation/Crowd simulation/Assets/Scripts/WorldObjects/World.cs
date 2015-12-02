﻿using Assets.Scripts.WorldObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts {
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

        public World() {
            Objects = new List<WorldObject>();
        }
        
        public List<T> ObjectSublist<T>() where T : WorldObject {
            return Objects.OfType<T>().ToList();
        }

    }
}
