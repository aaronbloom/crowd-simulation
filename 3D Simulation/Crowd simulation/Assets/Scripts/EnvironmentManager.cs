﻿using UnityEngine;
using System.Collections;

public class EnvironmentManager {

    private static EnvironmentManager shared;
    public Environment CurrentEnvironment { get; private set; }

    // Setup the static instance of this EnvironmentManager
    public EnvironmentManager() {
        initSingleton();
        CurrentEnvironment = new Environment();
    }

    private void initSingleton() {
        // Set the 'shared' variable as this environment manager
        if (shared == null) {
            shared = this;
        } else {
            throw new System.InvalidOperationException("Singleton already setup");
        }
    }

    // Return the static instance of this EnvironmentManager
    public static EnvironmentManager Shared() {
        return shared;
    }

}
