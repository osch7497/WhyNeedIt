using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PadLock", menuName = "Scriptable Objects/PadLock")]
public class PadLock : ScriptableObject
{
    public String RoomName;
    public GameObject Key;
}
