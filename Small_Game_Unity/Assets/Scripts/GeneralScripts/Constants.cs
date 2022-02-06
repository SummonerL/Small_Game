using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants {
    public static int POOL_COUNT = 8; // number of objects to instantiate when pooling
    public static float PLAYER_PROXIMITY_RADIUS = 0.7f; // sphere around player for object proximity detection
    public static float BUBBLE_POSITION_VERTICAL_BUFFER = 0.2f; // how far above an object should the interactive bubble 
    public static float WORLD_SPACE_CANVAS_SCALE = 0.004f; // used to scale world spaced UI elements to an appropriate size across all camera angles
}