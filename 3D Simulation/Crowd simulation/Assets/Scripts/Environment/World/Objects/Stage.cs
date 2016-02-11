using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Stage : Goal {
        public static string Name = "Stage";

        public Stage() : base() {
            this.Identifier = Name;
            this.InitialRotationOffSet = Quaternion.Euler(90, 0, 0);
            this.InitialPositionOffSet = new Vector3(0, 0.01f, 0);
            this.Size = new Vector3(4, 0, 4);
        }

    }
}
