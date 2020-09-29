using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockVisual : MonoBehaviour
{
    [SerializeField] private Type _type = Type.Empty;
    [SerializeField] private List<GameObject> _flags;
    [SerializeField] private int _value;

    [SerializeField] private List<GameObject> _walls;
    public void SetWall(int index)
    {
        try
        {
            _walls[index].SetActive(true);
        }
        catch
        {
            Debug.LogWarning("error with walls");
        }
    }

    public Type Type
    { 
        get
        {
            return _type;
        }
        set
        {
            _type = value;
            SwitchType();
        }
    }
    public int Value
    {
        get
        {
            return _value;
        }
        set
        {
            _value = value;
//          SwitchValue();
        }
    }
    private void Start()
    {
        SwitchType();
    }
    private void SwitchType()
    {
        for (int  i = 0; i < _flags.Count; i++)
        {
            _flags[i].SetActive(i == (int)_type);
        }
    }
}
