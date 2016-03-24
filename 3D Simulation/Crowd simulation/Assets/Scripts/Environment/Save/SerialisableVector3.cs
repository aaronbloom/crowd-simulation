using System;
using UnityEngine;

namespace Assets.Scripts.Environment.Save {
    [Serializable]
    public class SerialisableVector3
    {
        private readonly float x;
        private readonly float y;
        private readonly float z;

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
