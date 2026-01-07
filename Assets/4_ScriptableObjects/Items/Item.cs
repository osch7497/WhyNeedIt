using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite itemImage;
    public Vector3 rotationOffset;
    public Quaternion HandleRotation => Quaternion.Euler(rotationOffset);
    public float LightingValue = 0.9f;
}