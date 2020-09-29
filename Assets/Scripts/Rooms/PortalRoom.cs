using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalRoom : MonoBehaviour
{
    [SerializeField] private BlockVisual _room;
    [SerializeField] private GameObject _activePortalPrefab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == GameManager.Player)
        {
            if (_activePortalPrefab != null) { Instantiate(_activePortalPrefab, transform.position, Quaternion.identity); }
            GameManager.WinGame();
        }
    }
}
