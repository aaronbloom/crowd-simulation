﻿using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Stage : Goal {

        public Stage() : base() {
            this.Identifier = "Stage";
            this.InitialRotationOffSet = Quaternion.Euler(0, 0, 0);
            this.InitialPositionOffSet = new Vector3(0, 0, 0);
            this.Size = new Vector3(4, 2, 4);
        }

    }
}
