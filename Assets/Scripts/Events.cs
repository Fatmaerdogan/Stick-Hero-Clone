using System;
using UnityEngine;

public class Events 
{
    public static Action UpdateCase;
    public static Action StickSizeChange;
    public static Action StickFall;
    public static Action PlatformChange;
    public static Action ScoreUpdate;
    public static Action GameOver;
    public static Action GameStart;
    public static Action<GameObject> DeterminingCameraOffset;
    public static Action<Vector3> CaseChange;
    public static Action<GameObject,GameObject> CreateStartObject;
    public static Action<GameObject,GameObject> CreateStartStick;
}
public enum GameState
{
    START, INPUT, GROWING, NONE
}