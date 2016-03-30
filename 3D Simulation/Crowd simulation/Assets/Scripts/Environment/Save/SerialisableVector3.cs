using System;
using UnityEngine;

namespace Assets.Scripts.Environment.Save {

    /// <summary>
    /// A serialisable version of a 3d vector
    /// </summary>
    [Serializable]
    public class SerialisableVector3
    {
        private readonly float x;
        private readonly float y;
        private readonly float z;

        /// <summary>
        /// Create new Serialisable Vector 3 from <paramref name="vec"/>
        /// </summary>
        /// <param name="vec">the Vector3 to copy</param>
        public SerialisableVector3(Vector3 vec) {
            x = vec.x;
            y = vec.y;
            z = vec.z;
        }

        /// <summary>
        /// Transforms back to a normal Vector3
        /// </summary>
        /// <returns>The values in a Vector3</returns>
        public Vector3 Vector3() {
            return new Vector3(x,y,z);
        }
    }
}
