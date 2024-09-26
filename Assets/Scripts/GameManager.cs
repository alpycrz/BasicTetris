using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [Range(0.02f,1f)] [SerializeField] private float _moveTime = 0.25f;
    [Range(0.02f,1f)] [SerializeField] private float _rotateTime = 0.2f;
    [Range(0.02f,1f)] [SerializeField] private float _fallTime = 0.5f;
    [Range(0.02f,1f)] [SerializeField] private float _fastFallTime = 0.5f;
    private float _fallCounter;
    private float _fastFallCounter;
    private float _moveCounter;
    private float _rotateCounter;
    private SpawnManager _spawnManager;
    private BoardManager _boardManager;
    private ShapeManager _activeShape;
    private bool _gameOver = false;

    private void Start()
    {
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>();
        _boardManager = GameObject.FindObjectOfType<BoardManager>();

        if (_spawnManager)
        {
            if (_activeShape == null)
            {
                _activeShape = _spawnManager.RandomShape();
                _activeShape.transform.position = VectorToInt(_activeShape.transform.position);
            }
        }
    }

    private void Update()
    {
        if(!_boardManager || !_spawnManager || !_activeShape || _gameOver) return;

        InputControl();
    }

    private void InputControl()
    {
        // Shapes moves to right
        if ((Input.GetKey("right") && Time.time > _moveCounter) || Input.GetKeyDown("right"))
        {
            _activeShape.MoveRight();
            _moveCounter = Time.time + _moveTime;

            if (!_boardManager.IsValidPosition(_activeShape)) _activeShape.MoveLeft();
        }
        
        // Shapes moves to left
        else if ((Input.GetKey("left") && Time.time > _moveCounter) || Input.GetKeyDown("left"))
        {
            _activeShape.MoveLeft();
            _moveCounter = Time.time + _moveTime;

            if (!_boardManager.IsValidPosition(_activeShape)) _activeShape.MoveRight();
        }
        
        // Shapes rotate right
        else if (Input.GetKeyDown("up") && Time.time > _rotateCounter)
        {
            _activeShape.RotateRight();
            _rotateCounter = Time.time + _rotateTime;
            
            if (!_boardManager.IsValidPosition(_activeShape)) _activeShape.RotateLeft();
        }
        
        // Shapes falls down
        else if ((Input.GetKey("down") && Time.time > _fastFallCounter) || Time.time > _fallCounter)
        {
            _fallCounter = Time.time + _fallTime;
            _fastFallCounter = Time.time + _fastFallTime;
            
            if (_activeShape)
            {
                _activeShape.MoveDown();

                if (!_boardManager.IsValidPosition(_activeShape))
                {
                    if (_boardManager.IsOutOfBounds(_activeShape))
                    {
                        _activeShape.MoveUp();
                        _gameOver = true;
                    }
                    else
                    {
                        ShapePlaced();
                    }
                }
            }
        }
    }

    private void ShapePlaced()
    {
        _moveCounter = Time.time;
        _fastFallCounter = Time.time;
        _rotateCounter = Time.time;
        
        _activeShape.MoveUp();
        _boardManager.PlaceShape(_activeShape);

        if (_spawnManager) _activeShape = _spawnManager.RandomShape();
        
        _boardManager.ClearAllRows();
    }

    Vector2 VectorToInt(Vector2 vector)
    {
        return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
    }
}
