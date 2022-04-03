using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants {

    public static int POOL_COUNT = 8; // number of objects to instantiate when pooling
    
    public static float PLAYER_PROXIMITY_RADIUS = 0.7f; // sphere around player for object proximity detection
    
    // UI constants
    public static float BUBBLE_POSITION_VERTICAL_BUFFER = 0.2f; // how far above an object is the interactive bubble (world space)
    public static float DIALOGUE_BOX_POSITION_VERTICAL_BUFFER = 5.0f; // how far above an object is the dialogue box (screen space)
    public static float WORLD_SPACE_CANVAS_SCALE = 0.004f; // used to scale world spaced UI elements to an appropriate size across all camera angles
    
    // Dialogue Constants
    public static float TYPEWRITER_SPEED_SLOW = 24.0f;
    public static float TYPEWRITER_SPEED_NORMAL = 34.0f;
    public static float TYPEWRITER_SPEED_FAST = 44.0f;
    public static int DIALOGUE_ROW_CHARACTER_LIMIT = 40;
    public static int DIALOGUE_MAX_ROW_COUNT_LIMIT = 3;

    // Story Constants
    public static string NO_STORY_DIALOGUE_DEFAULT_TEXT = "I'm not sure about that...";

    // LeanTween constants
    public static float INTERACTION_BUBBLE_ENTRY_TIME = 0.2f;
    public static float INTERACTION_BUBBLE_EXIT_TIME = 0.1f;
    public static float INTERACTION_BUBBLE_WIGGLE_TIME = 1.0f;
    public static float INTERACTION_BUBBLE_WIGGLE_ROTATION_DEGREES = 5.0f;
    public static float INTERACTION_BUBBLE_UNTARGETED_FADE_OPACITY = 0.4f;
    public static float INTERACTION_BUBBLE_UNTARGETED_FADE_TIME = 0.1f;
    public static float INTERACTION_BUBBLE_UNTARGETED_ROTATE_BACK_TIME = 0.1f;
    public static float DIALOGUE_BOX_ENTRY_TIME = 0.2f;
    public static float DIALOGUE_BOX_EXIT_TIME = 0.1f;
    public static float DIALOGUE_PROGRESSION_DOT_SHOW_TIME = .05f;
    public static float DIALOGUE_PROGRESSION_DOT_DELAY_TIME = .3f;
    public static float DIALOGUE_PROGRESSION_DOT_FADE_TIME = .5f;

    public enum DIRECTIONS
    {
        LEFT,
        RIGHT
    }
}