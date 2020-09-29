using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLampIndicator : MonoBehaviour
{
    [SerializeField] private BlockVisual _room;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Player)
        {
            UIController.SetLamps(_room.Value);
        }
    }
}
