using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.WorldObjects {
    public class Goal : WorldObject {

        public Goal() : base() {
            this.Identifier = "Goal";
            this.InitialRotationOffSet = Quaternion.Euler(90, 0, 0);
            this.InitialPositionOffSet = new Vector3(0, -1.89f, 0);
            this.Size = 4;
        }

    }
}
