using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Map
{
    private int _sizeX;
    private int _sizeY;

    private int _entranceX;
    private int _entranceY;

    private int _exitX;
    private int _exitY;

    //вероятность
    private float _probability;
    
    private int _pathLength;

    //Block's map
    private Block[,] _mapArray;
    public Block[,] MapArray { get => _mapArray; private set => _mapArray = value; }

    public Map(int sizeX, int sizeY, int entranceX, int entranceY, float probability)
    {
        this._sizeX = sizeX;
        this._sizeY = sizeY;
        this._entranceX = entranceX;
        this._entranceY = entranceY;
        this._probability = probability;
        SetArray();
    }

    private void SetArray()
    {
        _mapArray = new Block[_sizeX, _sizeY];
        for (int x = 0; x < _mapArray.GetLength(0); x++)
        {
            for (int y = 0; y < _mapArray.GetLength(1); y++)
            {
                MapArray[x, y] = new Block();
            }
        }
    }

    public void SetPath(int pathlength)
    {
        int currentPositionX = _entranceX;
        int currentPositionY = _entranceY;
        Direction direction = GetRandomDirection();

        //Устанавливаем начало и конец пути в массиве
        _mapArray[currentPositionX, currentPositionY].Type = Type.Entrance;

        //Алгоритм 
        for (int i = 0; i < pathlength / 2; i++)
        {
            var cachedDirection = ReverseDirection(direction);
            do
            {
                if (!CheckDirection(currentPositionX, currentPositionY, Direction.Up) &&
                    !CheckDirection(currentPositionX, currentPositionY, Direction.Down) &&
                    !CheckDirection(currentPositionX, currentPositionY, Direction.Left) &&
                    !CheckDirection(currentPositionX, currentPositionY, Direction.Right))
                {
                    direction = cachedDirection;
                    break;
                }
                direction = GetRandomDirection();
            } while (!CheckDirection(currentPositionX, currentPositionY, direction));
            MakePath(ref currentPositionX, ref currentPositionY, direction);
        }

        _mapArray[currentPositionX, currentPositionY].Type = Type.Exit;
    }
    
    public void PlaceBombs()
    {
        float rand;
        foreach (var block in _mapArray)
        {
            rand = UnityEngine.Random.Range(0f, 1f);
            
            if (rand <= _probability && block.Type == Type.Empty)
            {
                block.Type = Type.Bomb;
            }
        }

    }

    public void SetValues()
    {
        for (int x = 0; x < _sizeX; x++)
        {
            for (int y = 0; y < _sizeY; y++)
            {
                if (_mapArray[x, y].Type == Type.Bomb)
                {
                    SetCrossValues(x, y);
                }
            }
        }
    }

    private void SetCrossValues(int centerX, int centerY)
    {
        //просто увеличиваем значения по кресту
        _mapArray[centerX, centerY].Value++;
        try
        {
            _mapArray[centerX + 1, centerY].Value++;
        }
        catch { Debug.Log("Right overflow"); }
        try
        {
            _mapArray[centerX - 1, centerY].Value++;
        }
        catch { Debug.Log("Left overflow"); }
        try
        {
            _mapArray[centerX, centerY + 1].Value++;
        }
        catch { Debug.Log("Down overflow"); }
        try
        {
            _mapArray[centerX, centerY - 1].Value++;
        }
        catch { Debug.Log("Up overflow"); }
    }
    private bool CheckDirection(int positionX, int positionY, Direction direction)
    {
        switch (direction)
        {
            case Direction.Down:
                if (positionY + 1 >= _sizeY - 1)
                {
                    return false;
                }
                else if (!IsPath(_mapArray[positionX, positionY + 1]) && !IsPath(_mapArray[positionX, positionY + 2]))
                {
                    return true;
                }
                break;
            case Direction.Up:
                if (positionY - 1 <= 0)
                {
                    return false;
                }
                else if (!IsPath(_mapArray[positionX, positionY - 1]) && !IsPath(_mapArray[positionX, positionY - 2]))
                {
                    return true;
                }
                break;
            case Direction.Left:
                if (positionX - 1 <= 0)
                {
                    return false;
                }
                else if (!IsPath(_mapArray[positionX - 1, positionY]) && !IsPath(_mapArray[positionX - 2, positionY]))
                {
                    return true;
                }
                break;
            case Direction.Right:
                if (positionX + 1 >= _sizeX - 1)
                {
                    return false;
                }
                else if (!IsPath(_mapArray[positionX + 1, positionY]) && !IsPath(_mapArray[positionX + 2, positionY]))
                {
                    return true;
                }
                break;
            default:
                throw new ArgumentException();
        }

        return false;
    }
    
    private void MakePath(ref int positionX, ref int positionY, Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                _mapArray[positionX, positionY - 1].Type = Type.Path;
                _mapArray[positionX, positionY - 2].Type = Type.Path;
                positionY -= 2;
                break;
            case Direction.Down:
                _mapArray[positionX, positionY + 1].Type = Type.Path;
                _mapArray[positionX, positionY + 2].Type = Type.Path;
                positionY += 2;
                break;
            case Direction.Right:
                _mapArray[positionX + 1, positionY].Type = Type.Path;
                _mapArray[positionX + 2, positionY].Type = Type.Path;
                positionX += 2;
                break;
            case Direction.Left:
                _mapArray[positionX - 1, positionY].Type = Type.Path;
                _mapArray[positionX - 2, positionY].Type = Type.Path;
                positionX -= 2;
                break;
            default:
                throw new ArgumentException();
        }
    }

    private Direction GetRandomDirection()
    {
        int value = UnityEngine.Random.Range(0, 4);
        switch (value)
        {
            case 0:
                return Direction.Up;
            case 1:
                return Direction.Down;
            case 2:
                return Direction.Left;
            case 3:
                return Direction.Right;
            default:
                throw new ArgumentException();
        }
    }

    private Direction ReverseDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.Down:
                return Direction.Up;
            case Direction.Up:
                return Direction.Down;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            default:
                throw new ArgumentException();
        }
    }

    private bool IsPath(Block block) { return (block.Type == Type.Path || block.Type == Type.Entrance); }
    
}
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
