﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Environment.World.Objects;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.UserInterface {
    class WorldBuilderCursor {
        private WorldObject primaryCursor;
        private WorldObject secondCursor;
        private readonly Material cursorMaterial;
        private readonly Material invalidCursorMaterial;
        private readonly Vector3 cursorHeight = new Vector3(0, 0.2f, 0);

        private string currentItem;
        private Vector3 startPlacement;
        private bool startedPlacement = false;

        private readonly WorldBuilderPlacement worldBuilderPlacement;

        public WorldBuilderCursor() {
            worldBuilderPlacement = new WorldBuilderPlacement();
            cursorMaterial = Resources.Load("Materials/Cursor", typeof(Material)) as Material;
            invalidCursorMaterial = Resources.Load("Materials/InvalidCursor", typeof(Material)) as Material;
        }

        public void Update(Vector3 groundPosition) {
            if (primaryCursor != null) {
                if (secondCursor != null) { Object.Destroy(secondCursor.GameObject); }
                if (startedPlacement) {
                    UpdateSecondaryCursor(groundPosition);
                } else {
                    UpdatePrimaryCursor(groundPosition);
                }
            }
        }

        public void UpdatePrimaryCursor(Vector3 groundPosition) {
            if (primaryCursor.GameObject != null) {
                if (!primaryCursor.GridPlaceable) { // wall placement
                    Vector3 position;
                    Vector3 normal;
                    if (worldBuilderPlacement.WallPlacement(out normal, out position, primaryCursor.Size)) {
                        primaryCursor.GameObject.transform.position =
                            Environment.Environment.PositionToLocation(position,
                            primaryCursor.Size) + cursorHeight;
                        primaryCursor.LookTowardsNormal(normal);
                        SetCursorValid(primaryCursor);
                        RecalcBars();
                    } else {
                        primaryCursor.GameObject.transform.position =
                            Environment.Environment.PositionToLocation(groundPosition, primaryCursor.Size) + cursorHeight;
                        SetCursorInvalid(primaryCursor);
                    }
                } else { // floor placement
                    primaryCursor.GameObject.transform.position =
                        Environment.Environment.PositionToGridLocation(groundPosition, primaryCursor.Size) + cursorHeight;
                }
            }
        }

        public void UpdateSecondaryCursor(Vector3 groundPosition) { //secondary cursor for drag to place
            secondCursor = NewCursor(WorldObject.DetermineObject(currentItem), groundPosition);
            Vector3 mousePosition = groundPosition;

            Vector3 secondCursorPosition;
            var xDiff = startPlacement.x - mousePosition.x;
            var zDiff = startPlacement.z - mousePosition.z;
            if (Mathf.Abs(xDiff) > Mathf.Abs(zDiff)) {
                secondCursorPosition = startPlacement + new Vector3(-xDiff, 0, 0);
            }
            else {
                secondCursorPosition = startPlacement + new Vector3(0, 0, -zDiff);
            }

            secondCursor.GameObject.transform.position =
                Environment.Environment.PositionToLocation(secondCursorPosition,
                    primaryCursor.Size) + cursorHeight;
            secondCursor.GameObject.transform.rotation = primaryCursor.GameObject.transform.rotation;
        }

        public void RecalcBars()
        {
            List<Bar> bars = new List<Bar>(BootStrapper.EnvironmentManager.CurrentEnvironment.World.Bars);
            foreach (Bar bar in bars)
            {
                Vector3 barPosition = bar.GameObject.transform.position;
                Vector3 offsetX = new Vector3((bar.Size.x/2)+0.4f,0,0);
                Vector3 offsetZ = new Vector3(0,0,(bar.Size.z/2)+0.4f);
                int isLeftBlocked = BootStrapper.EnvironmentManager.CurrentEnvironment.World.SpaceAlreadyOccupied(barPosition - offsetX) ? 1 : 0;
                int isRightBlocked = BootStrapper.EnvironmentManager.CurrentEnvironment.World.SpaceAlreadyOccupied(barPosition + offsetX) ? 1 : 0;
                int isUpBlocked = BootStrapper.EnvironmentManager.CurrentEnvironment.World.SpaceAlreadyOccupied(barPosition - offsetZ) ? 1 : 0;
                int isDownBlocked = BootStrapper.EnvironmentManager.CurrentEnvironment.World.SpaceAlreadyOccupied(barPosition + offsetZ) ? 1 : 0;
                int sides = isLeftBlocked + isRightBlocked + isUpBlocked + isDownBlocked;
                string pattern = "" + isLeftBlocked + isRightBlocked + isUpBlocked + isDownBlocked;
                int left = 0, up = 90, right = 180, down = 270;
                switch (sides)
                {
                    case 4:
                        switch (pattern) {
                            case "1111":
                                tryUpdatePattern("1111", "bar/bar¬", bar, down);
                                break;
                        }
                        break;
                    case 3:
                        bar.ChangePrefab("bar/barI");
                        switch (pattern) {
                            case "0111":
                                tryUpdatePattern("0001", "bar/barI", bar, left);
                                break;
                            case "1011":
                                tryUpdatePattern("1011", "bar/barI", bar, up);
                                break;
                            case "1101":
                                tryUpdatePattern("1101", "bar/barI", bar, right);
                                break;
                            case "1110":
                                tryUpdatePattern("1110", "bar/barI", bar, down);
                                break;
                        }
                        break;
                    case 2:
                        switch (pattern) {
                            case "0011":
                                tryUpdatePattern("0011", "bar/barL", bar, left);
                                break;
                            case "0110":
                                tryUpdatePattern("0110", "bar/barL", bar, up);
                                break;
                            case "1100":
                                tryUpdatePattern("1100", "bar/barL", bar, right);
                                break;
                            case "1010":
                                // =
                                break;
                            case "0101":
                                // =
                                break;
                            case "1001":
                                tryUpdatePattern("1001", "bar/barL", bar, down);
                                break;
                        }
                        break;
                    case 1:
                        switch (pattern) {
                            case "1000":
                                tryUpdatePattern("1000", "bar/barU", bar, left);
                                break;
                            case "0100":
                                tryUpdatePattern("0100", "bar/barU", bar, right);
                                break;
                            case "0010":
                                tryUpdatePattern("0010", "bar/barU", bar, up);
                                break;
                            case "0001":
                                tryUpdatePattern("0001", "bar/barU", bar, down);
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

        public void SetPlacementObject(string objectName, Vector3 groundPosition) {
            currentItem = objectName;
            DestroyCursors();
            primaryCursor = NewCursor(WorldObject.DetermineObject(objectName), groundPosition);
        }

        public void StartPlaceWorldObject(Vector3 groundPosition) {
            if (currentItem != null) {
                if (UserWorldBuilder.NotOverUI()) {
                    if (!primaryCursor.GridPlaceable) {
                        Vector3 position;
                        Vector3 normal;
                        if (worldBuilderPlacement.WallPlacement(out normal, out position, primaryCursor.Size)) {
                            startPlacement = position;
                            startedPlacement = true;
                        } else {
                            startPlacement = groundPosition;
                            startedPlacement = true;
                        }
                    } else {
                        startPlacement = groundPosition;
                        startedPlacement = true;

                    }
                }
            }
        }

        public void EndPlaceWorldObject(Vector3 groundPosition) {
            if (currentItem != null) {
                if (UserWorldBuilder.NotOverUI()) {
                    if (startedPlacement) {

                        Vector3 endPlacement;
                        if (!primaryCursor.GridPlaceable) {
                            Vector3 position;
                            Vector3 normal;
                            if (worldBuilderPlacement.WallPlacement(out normal, out position, primaryCursor.Size)) {
                                endPlacement = position;
                                worldBuilderPlacement.PlaceLine(startPlacement, endPlacement, normal, currentItem);
                            }
                        } else {
                            endPlacement = groundPosition;
                            worldBuilderPlacement.PlaceLine(startPlacement, endPlacement, Vector3.forward, currentItem);
                        }
                    }
                }
            }
            startedPlacement = false;
        }

        public void DestroyCursors() {
            if (primaryCursor != null) {
                Object.Destroy(primaryCursor.GameObject);
            }
        }

        private void SetCursorValid(WorldObject cursor) {
            cursor.GameObject.GetComponent<Renderer>().material = cursorMaterial;
        }

        private void SetCursorInvalid(WorldObject cursor) {
            cursor.GameObject.GetComponent<Renderer>().material = invalidCursorMaterial;
        }

        private WorldObject NewCursor(WorldObject worldObject, Vector3 groundPosition) {
            var cursor = WorldObject.Initialise(worldObject, groundPosition);
            SetCursorValid(cursor);
            Collider collider = cursor.GameObject.GetComponent<Collider>();
            if (collider != null) collider.enabled = false;
            return cursor;
        }
    }
}
