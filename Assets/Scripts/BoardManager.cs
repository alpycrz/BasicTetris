using System;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Transform _tilePrefab;
    [SerializeField] private int _height = 20;
    [SerializeField] private int _width = 10;
    private Transform[,] _grid;

    private void Awake() => _grid = new Transform[_width, _height];

    private void Start() => CreateGrid();

    private bool IsInsideBoard(int x, int y)
    {
        return (x >= 0 && x < _width && y >= 0);
    }

    private bool IsTileFilled(int x, int y, ShapeManager shape)
    {
        return (_grid[x, y] != null && _grid[x, y].parent != shape.transform);
    }

    public bool IsValidPosition(ShapeManager shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 position = VectorToInt(child.position);

            if (!IsInsideBoard((int)position.x, (int)position.y)) return false;
            
            if (position.y < _height)
            {
                if (IsTileFilled((int)position.x, (int)position.y, shape)) return false;
            }
        }
        return true;
    }

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

    public void PlaceShape(ShapeManager shape)
    {
        if (shape == null) return;

        foreach (Transform child in shape.transform)
        {
            Vector2 position = VectorToInt(child.position);
            _grid[(int)position.x, (int)position.y] = child;
        }
    }

    private bool IsRowFilled(int y)
    {
        for (int x = 0; x < _width; ++x)
        {
            if (_grid[x, y] == null)
            {
                return false;
            }
        }
        return true;
    }

    private void ClearRow(int y)
    {
        for (int x = 0; x < _width; ++x)
        {
            if (_grid[x, y] != null)
            {
                Destroy(_grid[x, y].gameObject);
            }
            _grid[x, y] = null;
        }
    }

    private void RowMoveDown(int y)
    {
        for (int x = 0; x < _width; ++x)
        {
            if (_grid[x, y] != null)
            {
                _grid[x, y -1] = _grid[x, y];
                _grid[x, y] = null;
                _grid[x, y - 1].position += Vector3.down;
            }
        }
    }

    private void AllRowsMoveDown(int startY)
    {
        for (int i = startY; i < _height; ++i)
        {
            RowMoveDown(i);
        }
    }

    public void ClearAllRows()
    {
        for (int y = 0; y < _height; y++)
        {
            if (IsRowFilled(y))
            {
                ClearRow(y);
                AllRowsMoveDown(y);
                y--;
            }
        }
        
    }

    public bool IsOutOfBounds(ShapeManager shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (child.transform.position.y >= _height -1)
            {
                return true;
            }
        }
        return false;
    }
    Vector2 VectorToInt(Vector2 vector)
    {
        return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
    }

}
