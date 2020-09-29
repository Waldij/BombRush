using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    private Map _map;
    [SerializeField] private Vector2Int _size;
    [SerializeField] private Vector2Int _start;
    [SerializeField] private bool _randomStart;

    [SerializeField] private int _pathLenght;
    [Range(0, 1), SerializeField] private float _probability;

    [SerializeField] private Vector2 _roomSize;
    [SerializeField] private List<BlockVisual> _roomPrefab;
    [SerializeField] private GameObject _roomsParent;

    public Vector2 RoomSize { get => _roomSize; }
    public Vector2Int Size { get => _size; private set => _size = value; }
    private void Start()
    {
        if (_randomStart)
        {
            var randStart = new Vector2Int(Random.Range(0, _size.x), Random.Range(0, _size.y));
            GameManager.Player.GetComponent<PlayerControls>().Teleport(randStart * _roomSize, randStart);
            _map = new Map(_size.x, _size.y, randStart.x, randStart.y, _probability);
        }
        else
        {
            GameManager.Player.GetComponent<PlayerControls>().Teleport(_start * _roomSize, _start);
            _map = new Map(_size.x, _size.y, _start.x, _start.y, _probability);
        }

        _map.SetPath(_pathLenght);
        _map.PlaceBombs();
        _map.SetValues();




        for (int x = 0; x < _map.MapArray.GetLength(0); x++)
        {
            for (int y = 0; y < _map.MapArray.GetLength(1); y++)
            {
                var position = new Vector3(x * _roomSize.x, y * _roomSize.y, 0f);
                var rand = Random.Range(0, _roomPrefab.Count);
                var room = Instantiate(_roomPrefab[rand], position, Quaternion.identity);

                if (x == 0) { room.SetWall(0); }
                if (y == 0) { room.SetWall(1); }
                if (x == _map.MapArray.GetLength(0) - 1) { room.SetWall(2); }
                if (y == _map.MapArray.GetLength(1) - 1) { room.SetWall(3); }


                room.Type = _map.MapArray[x, y].Type;
                room.Value = _map.MapArray[x, y].Value;
                room.transform.parent = _roomsParent.transform;
            }
        }
    }
}
