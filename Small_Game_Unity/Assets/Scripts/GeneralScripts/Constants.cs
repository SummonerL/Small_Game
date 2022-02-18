using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants {

    public static int POOL_COUNT = 8; // number of objects to instantiate when pooling
    public static float PLAYER_PROXIMITY_RADIUS = 0.7f; // sphere around player for object proximity detection
    public static float BUBBLE_POSITION_VERTICAL_BUFFER = 0.2f; // how far above an object should the interactive bubble 
    public static float WORLD_SPACE_CANVAS_SCALE = 0.004f; // used to scale world spaced UI elements to an appropriate size across all camera angles

    // LeanTween constants
    public static float INTERACTION_BUBBLE_ENTRY_TIME = 0.2f;
    public static float INTERACTION_BUBBLE_EXIT_TIME = 0.1f;
    public static float INTERACTION_BUBBLE_WIGGLE_TIME = 1.0f;
    public static float INTERACTION_BUBBLE_WIGGLE_ROTATION_DEGREES = 5.0f;

    public enum DIRECTIONS
    {
        LEFT,
        RIGHT
    }
}