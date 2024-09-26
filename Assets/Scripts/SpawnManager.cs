using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private ShapeManager[] _shapes;

    public ShapeManager RandomShape()
    {
        int randomShape = Random.Range(0, _shapes.Length);
        ShapeManager shape = Instantiate(_shapes[randomShape], transform.position, Quaternion.identity) as ShapeManager;

        if (shape != null)
            return shape;
        else
            return null;
    }
}
