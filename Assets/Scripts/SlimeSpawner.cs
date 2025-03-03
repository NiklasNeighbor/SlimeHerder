using System.Collections.Generic;
using UnityEngine;

public class SlimeSpawner : MonoBehaviour
{

    [SerializeField] private int _slimesAmount;
    [SerializeField] private GameObject _slimePrefab;
    [Tooltip("x - bottom left, y - bottom right , z - top left, w - top right")]
    [SerializeField] private Vector4 _spawnBorders;

    private List<GameObject> _slimes = new ();
    void Start()
    {
        SpawnSlimes();
    }

    private void SpawnSlimes()
    {
        //Create slimes
        for (int i = 0; i < _slimesAmount; i++)
        {
            //Create a random position
            Vector2 spawnPosition = new(
                Random.Range(_spawnBorders.x, _spawnBorders.y),
                Random.Range(_spawnBorders.z, _spawnBorders.w));
            
            //Instantiate a slime and add it to slimes list
            _slimes.Add(Instantiate(_slimePrefab, spawnPosition, Quaternion.identity, transform));
        }
        
    }
}
