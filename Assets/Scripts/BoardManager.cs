using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Transform _tilePrefab;
    [SerializeField] private int _height = 20;
    [SerializeField] private int _width = 10;

    private void Start() => CreateGrid();

    private void CreateGrid()
    {
        for (int y = 0; y < _height; y++)
        for (int x = 0; x < _width; x++)
        { 
            Transform tile = Instantiate(_tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
            tile.name = "Tile_" + x + "_" + y;
            tile.SetParent(transform);
        }
    }

}
