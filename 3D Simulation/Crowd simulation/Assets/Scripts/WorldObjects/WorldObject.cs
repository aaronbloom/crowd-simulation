using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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
    }
}