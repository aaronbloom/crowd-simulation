using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking.Match;
using Object = UnityEngine.Object;

namespace Assets.Scripts.WorldObjects {
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

        public Quaternion InitialRotationOffSet { get; protected set; }
        public Vector3 InitialPositionOffSet { get; protected set; }
        public float Size { get; protected set; }

        public GameObject GameObject { get; set; }

        protected WorldObject() {
            InitialPositionOffSet = Vector3.zero;
            InitialRotationOffSet = Quaternion.identity;
        }

        //basic withinbounds checker, override for complex world objects
        public bool WithinBounds(Vector3 position) {
            Vector3 gameObjectPosition = GameObject.transform.position;
            gameObjectPosition.y = 0;
            position.y = 0;
            return gameObjectPosition == position;
        }

        public void Destroy() {
            Object.Destroy(GameObject);
        }
    }
}