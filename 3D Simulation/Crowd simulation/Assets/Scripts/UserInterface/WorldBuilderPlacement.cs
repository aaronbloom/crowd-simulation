using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Environment;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.UserInterface {
    class WorldBuilderPlacement {

        public WorldBuilderPlacement() {}

        public bool WallPlacement(out Vector3 normal, out Vector3 position, WorldObject cursor) { //is the cursor over a wall
            RaycastHit hit;
            GameObject gameObject;
            if (UserWorldBuilder.Raycast(out hit, out gameObject)) {

                if (gameObject.name.Contains(Wall.IdentifierStatic)) {

                    Vector3 firstWallPosition = hit.transform.position;

                    normal = hit.normal;
                    if (normal.y == 0) { //only wall sides, not the tops
                        //perpendicular vector to normal (right vector from point of view of normal)
                        Vector3 crossRight = Vector3.Cross(Vector3.up, normal).normalized;

                        cursor.LookTowardsNormal(normal);
                        float requiredWidth = cursor.Size.x;

                        Vector3 leftMostWall = firstWallPosition;
                        Vector3 rightMostWall = firstWallPosition;

                        //Scans and checks existance of walls left and right of the selected wall
                        int count = 1;
                        Vector3 scanPosition = firstWallPosition;
                        for (int i = 0; i < requiredWidth - 1; i++) {
                            if (count >= requiredWidth) break;
                            scanPosition += crossRight;
                            if (
                                BootStrapper.EnvironmentManager.CurrentEnvironment.World.PointAlreadyOccupied(
                                    scanPosition)) {
                                count++;
                                rightMostWall = scanPosition;
                            } else {
                                scanPosition = firstWallPosition;
                                for (int j = 0; j < requiredWidth - 1; j++) {
                                    if (count >= requiredWidth) break;
                                    scanPosition -= crossRight;
                                    if (
                                        BootStrapper.EnvironmentManager.CurrentEnvironment.World
                                            .PointAlreadyOccupied(scanPosition)) {
                                        count++;
                                        leftMostWall = scanPosition;
                                    } else {
                                        break;
                                    }
                                }
                                break;
                            }
                        }

                        //if walls are sufficiently wide, then calculate position of wall placed world object
                        if (count >= requiredWidth) {

                            Vector3 centerWall = (leftMostWall + rightMostWall) / 2;
                            Vector3 requiredDepth = normal * (cursor.Size.z / 2);
                            Vector3 wallOffset = normal * (Wall.SizeStatic.z / 2);

                            position = centerWall + requiredDepth + wallOffset;
                            return true;
                        }
                    }
                }
            }
            position = Vector3.zero;
            normal = Vector3.zero;
            return false;
        }

        public void Place(WorldObject worldObject, Vector3 position, Vector3 wallNormal) {
            BootStrapper.EnvironmentManager.CurrentEnvironment.Place(worldObject, position);
            worldObject.LookTowardsNormal(wallNormal);
            RecalcBars();
        }

        public void Place(WorldObject worldObject, Vector3 position) {
            BootStrapper.EnvironmentManager.CurrentEnvironment.Place(worldObject, position);
            RecalcBars();
        }

        public WorldObject[] PlaceLine(Vector3 start, Vector3 end, Vector3 wallNormal, String objectName) {
            Vector3 step;
            var xDiff = start.x - end.x;
            var zDiff = start.z - end.z;
            int largerDiff;
            if (Mathf.Abs(xDiff) > Mathf.Abs(zDiff)) {
                step = new Vector3(-Mathf.Sign(xDiff), 0, 0);
                largerDiff = (int)Mathf.Abs(xDiff);
            } else {
                step = new Vector3(0, 0, -Mathf.Sign(zDiff));
                largerDiff = (int)Mathf.Abs(zDiff);
            }
            Vector3 position = start;
            WorldObject[] createdWorldObjects = new WorldObject[largerDiff + 1];
            for (int i = 0; i <= largerDiff; i++) {
                var currentWorldObject = WorldObject.DetermineObject(objectName);
                createdWorldObjects[i] = currentWorldObject;
                currentWorldObject.AdjustSizing(wallNormal);
                Place(currentWorldObject, position);
                position += step;
            }
            return createdWorldObjects;
        }

        public void PlacePerimeterWall(Vector3 origin, Vector3 bounds) {
            float spacing = Wall.SizeStatic.x / 2;
            for (var x = origin.x; x < bounds.x + origin.x; x+= spacing) {
                Place(WorldObject.DetermineObject(Wall.IdentifierStatic), new Vector3(x, 0, 0));
            }
            for (var x = origin.x; x < bounds.x + origin.x; x += spacing) {
                Place(WorldObject.DetermineObject(Wall.IdentifierStatic), new Vector3(x, 0, bounds.z + origin.z));
            }
            for (var z = origin.z; z < bounds.z + origin.z; z += spacing) {
                Place(WorldObject.DetermineObject(Wall.IdentifierStatic), new Vector3(0, 0, z));
            }
            for (var z = origin.z; z < bounds.z + origin.z; z += spacing) {
                Place(WorldObject.DetermineObject(Wall.IdentifierStatic), new Vector3(bounds.x + origin.x, 0, z));
            }
        }

        private int[] directionsBlocked(WorldObject obj)
        {
            float whiskerDepth = 0.1f;
            Vector3 position = obj.GameObject.transform.position;
            Vector3 offsetX = new Vector3((obj.Size.x / 2) + whiskerDepth, 0, 0);
            Vector3 offsetZ = new Vector3(0, 0, (obj.Size.z / 2) + whiskerDepth);

            int[] directionsBlocked = new int[4];
            directionsBlocked[0] = BootStrapper.EnvironmentManager.CurrentEnvironment.World.SpaceAlreadyOccupied(position - offsetX) ? 1 : 0;
            directionsBlocked[1] = BootStrapper.EnvironmentManager.CurrentEnvironment.World.SpaceAlreadyOccupied(position + offsetX) ? 1 : 0;
            directionsBlocked[2] = BootStrapper.EnvironmentManager.CurrentEnvironment.World.SpaceAlreadyOccupied(position - offsetZ) ? 1 : 0;
            directionsBlocked[3] = BootStrapper.EnvironmentManager.CurrentEnvironment.World.SpaceAlreadyOccupied(position + offsetZ) ? 1 : 0;

            return directionsBlocked;
        }

        int left = 0, back = 90, right = 180, forward = 270;
        public void RecalcBars()
        {
            List<Bar> bars = new List<Bar>(BootStrapper.EnvironmentManager.CurrentEnvironment.World.Bars);
            foreach (Bar bar in bars)
            {
                int[] directionsBlocked = this.directionsBlocked(bar);
                int sides = directionsBlocked.Sum();
                string pattern = string.Join("",directionsBlocked.Select(x => x.ToString()).ToArray());

                switch (sides)
                {
                    case 4:
                        switch (pattern) {
                            case "1111":
                                switch (bar.placementPattern)
                                {
                                    case "0111":
                                        tryUpdatePattern(pattern, "bar/bar¬", bar, right);
                                        break;
                                    case "1011":
                                        tryUpdatePattern(pattern, "bar/bar¬", bar, left);
                                        break;
                                    case "1101":
                                        tryUpdatePattern(pattern, "bar/bar¬", bar, right);
                                        break;
                                    case "1110":
                                        tryUpdatePattern(pattern, "bar/bar¬", bar, forward);
                                        break;
                                }
                                break;
                        }
                        break;
                    case 3:
                        switch (pattern) {
                            case "0111":
                                tryUpdatePattern(pattern, "bar/barI", bar, forward);
                                break;
                            case "1011":
                                tryUpdatePattern(pattern, "bar/barI", bar, back);
                                break;
                            case "1101":
                                tryUpdatePattern(pattern, "bar/barI", bar, right);
                                break;
                            case "1110":
                                tryUpdatePattern(pattern, "bar/barI", bar, left);
                                break;
                        }
                        break;
                    case 2:
                        switch (pattern) {
                            case "0011":
                                // =
                                break;
                            case "0110":
                                tryUpdatePattern(pattern, "bar/barL", bar, forward);
                                break;
                            case "1100":
                                // =
                                break;
                            case "1010":
                                tryUpdatePattern(pattern, "bar/barL", bar, left);
                                break;
                            case "0101":
                                tryUpdatePattern(pattern, "bar/barL", bar, right);
                                break;
                            case "1001":
                                tryUpdatePattern(pattern, "bar/barL", bar, back);
                                break;
                        }
                        break;
                    case 1:
                        switch (pattern) {
                            case "1000":
                                tryUpdatePattern(pattern, "bar/barU", bar, back);
                                break;
                            case "0100":
                                tryUpdatePattern(pattern, "bar/barU", bar, forward);
                                break;
                            case "0010":
                                tryUpdatePattern(pattern, "bar/barU", bar, left);
                                break;
                            case "0001":
                                tryUpdatePattern(pattern, "bar/barU", bar, right);
                                break;
                        }
                        break;
                }
            }
        }

        private void tryUpdatePattern(string pattern, string prefab, Bar bar, int yVal)
        {
            if (bar.IsNewPlacementPattern(pattern))
            {
                bar.placementPattern = pattern;
                bar.ChangePrefab(prefab);
                bar.GameObject.transform.rotation = rotateToY(bar, yVal);
            }
        }

        private Quaternion rotateToY(WorldObject obj, int yVal)
        {
            return Quaternion.Euler(obj.GameObject.transform.rotation.eulerAngles.x, yVal, obj.GameObject.transform.rotation.eulerAngles.z);
        }

    }
}
