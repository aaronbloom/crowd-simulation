using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class HeatMapTile : WorldObject {

        public const int TileSize = 2;

        private const string IdentifierStatic = "HeatMapTile";
        private static readonly Quaternion initialRotationOffSet = Quaternion.Euler(90, 0, 0);
        private static readonly Vector3 initialPositionOffSet = new Vector3(0, 0.02f, 0);

        public HeatMapTile() {
            this.Identifier = IdentifierStatic;
            this.InitialRotationOffSet = initialRotationOffSet;
            this.InitialPositionOffSet = initialPositionOffSet;
            this.Size = new Vector3(TileSize, 0, TileSize);
            this.GridPlaceable = true;
        }

    }
}
