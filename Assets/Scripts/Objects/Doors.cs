using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    private Animator _animator;
    private void Start()
    {
        _animator = GetComponent<Animator>();   
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Player)
        {
            _animator.SetTrigger("Open");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Player)
        {
            _animator.SetTrigger("Close");
        }
    }
}
