using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class HeatMapTile : WorldObject {

        public static int TileSize = 2;

        public HeatMapTile() : base() {
            this.Identifier = "HeatMapTile";
            this.InitialRotationOffSet = Quaternion.Euler(90, 0, 0);
            this.InitialPositionOffSet = new Vector3(0, 0.02f, 0);
            this.Size = new Vector3(TileSize, 0, TileSize);
            this.GridPlaceable = true;
        }

    }
}
