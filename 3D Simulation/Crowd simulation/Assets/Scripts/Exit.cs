﻿using UnityEditor;
using UnityEngine;

namespace Assets.Scripts {

    /// <summary>
    /// Safe exit for quit button
    /// </summary>
    public class Exit : MonoBehaviour {
        public void ExitApp() {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}
