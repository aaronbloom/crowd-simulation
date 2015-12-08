using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.WorldObjects {
    public class Wall : WorldObject, Collidable {

        public Wall() : base() {
            this.Identifier = "Wall";
            this.Size = 4;
        }

    }
}
