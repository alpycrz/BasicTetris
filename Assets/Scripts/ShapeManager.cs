using System;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    [SerializeField] private bool _canRotate = true;
    
    public void MoveLeft() => transform.Translate(Vector3.left, Space.World);
    public void MoveRight() => transform.Translate(Vector3.right, Space.World);
    public void MoveDown() => transform.Translate(Vector3.down, Space.World);
    public void MoveUp() => transform.Translate(Vector3.up, Space.World);

    public void RotateLeft()
    {
        if (_canRotate) transform.Rotate(0,0,-90);
    }    
    public void RotateRight()
    {
        if (_canRotate) transform.Rotate(0,0,90);
    }
}
