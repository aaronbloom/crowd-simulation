using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Environment.Save {
    [Serializable]
    public class SerialisableVector3
    {
        private float x;
        private float y;
        private float z;

        public SerialisableVector3(Vector3 vec) {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }

        public Vector3 Vector3() {
            return new Vector3(x,y,z);
        }
    }
}
