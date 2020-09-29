using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    private int _value;
    private Type _type;
    public Type Type { get => _type; set => _type = value; }
    public int Value { get => _value; set => _value = value; }
    public Block()
    {
        _type = Type.Empty;
        _value = 0;
    }

}
public enum Type
{
    Bomb,
    Path,
    Entrance,
    Exit,
    Empty
}
