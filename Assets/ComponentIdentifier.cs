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
        Player
    }
    public LevelComponentType obstacleType;

}
