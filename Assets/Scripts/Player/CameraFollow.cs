using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [Range(1f, 40f), SerializeField] private float _smooth;
    private Vector3 _offset;
    private Camera _camera;
    public bool Following = true;
    [SerializeField] private GameObject _explosionPrefab;
    public AudioSource Boom;

    private void Start()
    {
        GameManager.GetInstance().TimerEnded += DefeatCinematic;
        GameManager.GetInstance().GameLoose += DefeatCinematic;
        GameManager.GetInstance().GameWin += WinCinematic;
        _offset = transform.position - _target.position;
        _camera = GetComponent<Camera>();
    }
    private void FixedUpdate()
    {
        if (Following)
        {
            Vector3 position = _offset;
            position.x = _target.position.x;
            position.y = Mathf.Lerp(transform.position.y, _target.position.y, 1 / _smooth);
            transform.position = position;
        }
    }
    private void DefeatCinematic()
    {
        StartCoroutine(LooseCinema());
    }
    private void WinCinematic()
    {
        StartCoroutine(WinCinema());
    }
    private IEnumerator LooseCinema()
    {
        float counter = 0f;
        yield return new WaitForSecondsRealtime(1f);
        while (_camera.orthographicSize >= 7.5)
        {
            counter += Time.deltaTime;
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, 7f, counter);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1f);
        var shaker = GetComponent<CameraShake>();
        shaker.Shake(0.7f);
        var position = transform.position;
        position.z = 0f;
        GameManager.Player.SetActive(false);
        //взрыв
        Boom.Play();
        Instantiate(_explosionPrefab, position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(2f);
        UIController.ShowDefeatMenu();

    }

    private IEnumerator WinCinema()
    {
        float counter = 0f;
        yield return new WaitForSecondsRealtime(1f);
        while (_camera.orthographicSize >= 7.5)
        {
            counter += Time.deltaTime;
            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, 7f, counter);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(1f);
        UIController.ShowWinMenu();

    }
}
