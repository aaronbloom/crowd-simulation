﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Environment;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.UserInterface {
    class WorldBuilderPlacement {

        public WorldBuilderPlacement() {}

        public bool WallPlacement(out Vector3 position, Vector3 cursorSize) { //is the cursor over a wall
            RaycastHit hit;
            GameObject gameObject;
            if (UserWorldBuilder.Raycast(out hit, out gameObject)) {

                if (gameObject.name.Contains(Wall.IdentifierStatic)) {

                    Vector3 firstWallPosition = hit.transform.position;

                    Vector3 normal = hit.normal;
                    if (normal.y == 0) { //only wall sides, not the tops
                        //perpendicular vector to normal (right vector from point of view of normal)
                        Vector3 crossRight = Vector3.Cross(Vector3.up, normal).normalized;

                        float requiredWidth = cursorSize.x;

                        Vector3 leftMostWall = firstWallPosition;
                        Vector3 rightMostWall = firstWallPosition;

                        //Scans and checks existance of walls left and right of the selected wall
                        int count = 1;
                        Vector3 scanPosition = firstWallPosition;
                        for (int i = 0; i < requiredWidth - 1; i++) {
                            if (count >= requiredWidth) break;
                            scanPosition += crossRight;
                            if (
                                BootStrapper.EnvironmentManager.CurrentEnvironment.World.AlreadyOccupied(
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
                                            .AlreadyOccupied(scanPosition)) {
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
                            Vector3 requiredDepth = normal * (cursorSize.z / 2);
                            Vector3 wallOffset = normal * (Wall.SizeStatic.z / 2);

                            position = centerWall + requiredDepth + wallOffset;
                            return true;
                        }
                    }
                }
            }
            position = Vector3.zero;
            return false;
        }

        public void Place(WorldObject worldObject, Vector3 position) {
            BootStrapper.EnvironmentManager.CurrentEnvironment.Place(worldObject, position);
        }

        public WorldObject[] PlaceLine(Vector3 start, Vector3 end, String objectName) {
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
    }
}