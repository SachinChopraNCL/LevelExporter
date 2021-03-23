using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentIdentifier : MonoBehaviour
{
    public enum LevelComponentType 
    {
        Plane,
        Obstacle,
        Wall, 
        Player,
        Coin,
        Checkpoint,
        Finishline,
        Jump,
        Speed
    }
    public LevelComponentType obstacleType;

}
