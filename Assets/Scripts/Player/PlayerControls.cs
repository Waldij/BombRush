using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public Vector2Int LogicPosition;


    [Range(0.1f, 5f), SerializeField] private float _timeDash;
    [SerializeField] private ControlType _controlType = ControlType.Arrow;
    [SerializeField] private MapGenerator _map;
    [SerializeField] private float _touchSensivity;
    public AudioSource SlideSound;
    public AudioSource WallSound;

    private float _speed;
    private Vector2 _step;
    private Touch _firstTouch;
    private Touch _lastTouch;
    private bool _drag;
    private bool _swipe;
    private bool _isMoving = false;
    private Vector3 _destination;
    private Vector3 position;
    private Animator _animator;
    private bool _allowMove = true;

    void Start()
    {
        GameManager.GetInstance().TimerEnded += DisableMoving;
        GameManager.GetInstance().GameLoose += DisableMoving;
        GameManager.GetInstance().GameWin += DisableWin;
        _animator = GetComponent<Animator>();
        _step = _map.RoomSize;
//#if UNITY_ANDROID && !UNITY_EDITOR
//        _swipeControls = true;
//#else
//        _swipeControls = false;
//#endif
    }
    private void Update()
    {
        if (Time.timeScale <= 0 || !_allowMove) { return; }
        SetAnimations();
        if (!_isMoving)
        {
            Vector3 direction = GetDirection();
            if (direction.magnitude > 0)
            {
                position = new Vector3(direction.x * _step.x, direction.y * _step.y, 0);
                Vector3 scale = new Vector3(Mathf.Sign(direction.x), transform.localScale.y, transform.localScale.z);
                transform.localScale = scale;
                if (!AllowMove(direction, out Vector2Int logicDirection)) 
                {
                    //бьемся в стену
                    WallSound.Play();
                    return; 
                }
                LogicPosition += logicDirection;
                var velocity = position / _timeDash;
                _speed = velocity.magnitude;
                _destination = transform.position + position;
                //конец свайпа
                SlideSound.Play();
                _isMoving = true;
            }
        }
    }
    void FixedUpdate()
    {
       if (Time.timeScale <= 0) { return; }
       if (_isMoving)
        {
            
            if (Vector3.Distance(_destination, transform.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, _destination, Time.fixedDeltaTime * _speed);
                //transform.position = _destination;
            }
            else
            {
                _isMoving = false;
            }

        }


    }

    public void Teleport(Vector2 position, Vector2Int logicPosition)
    {
        transform.position = position;
        var camera = Camera.main.transform;
        LogicPosition = logicPosition;
        Vector3 cameraPosition = new Vector3(position.x, position.y, camera.position.z);
        camera.position = cameraPosition;
    }

    private bool AllowMove(Vector3 direction, out Vector2Int logicDirection)
    {
        logicDirection = new Vector2Int((int)direction.x, (int)direction.y);
        Vector2Int logicDestination = logicDirection + LogicPosition;
        return !(logicDestination.x < 0 || logicDestination.y < 0 || logicDestination.x > _map.Size.x - 1 || logicDestination.y > _map.Size.y - 1);
    }
    private void SetAnimations()
    {
        _animator.SetBool("Dash", _isMoving);
    }
    private Vector2 ArrowControls()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            return Vector2.up;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return Vector2.left;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return Vector2.right;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            return Vector2.down;
        }
        return Vector2.zero;
    }
    private Vector2 SwipeControls()
    {
        if (!_drag & !_swipe)
        {
            if (Input.touchCount == 1)
            {
                _drag = true;
                _swipe = true;
                _firstTouch = Input.GetTouch(0);
            }
        }
        else
        {
            if (Input.touchCount == 1)
            {
                if (!_swipe) 
                {
                    return Vector2.zero; 
                }
                _lastTouch = Input.GetTouch(0);

            }
            else
            {
                var difference = (_lastTouch.position - _firstTouch.position);
                _lastTouch = new Touch();
                _swipe = false;
                _drag = false;
                if (difference.magnitude > _touchSensivity)
                {

                    difference = difference.normalized;


                    if (Mathf.Abs(difference.y) >= Mathf.Abs(difference.x))
                    {
                        if (difference.y > 0)
                        {
                            return Vector2.up;
                        }
                        else
                        {
                            return Vector2.down;
                        }

                    }
                    else
                    {
                        if (difference.x > 0)
                        {
                            return Vector2.right;
                        }
                        else
                        {
                            return Vector2.left;
                        }
                    }
                }

                _drag = false;
            }
        }
        return Vector2.zero;



    }

    private Vector2 GetDirection()
    {
        switch (_controlType)
        {
            case ControlType.Arrow:
                return ArrowControls();
            case ControlType.Swipe:
                return SwipeControls();
            default:
                return Vector2.zero;
        }
    }

    private void DisableMoving()
    {
        _allowMove = false;
        _animator.SetTrigger("Look");
    }
    private void DisableWin()
    {
        _allowMove = false;
        _animator.SetTrigger("Win");
    }
}
public enum ControlType
{
    Arrow, Swipe
}
