using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SlimeSpawner : MonoBehaviour
{
    public static SlimeSpawner Instance { get; private set; }
    
    [SerializeField] private int _slimesAmount;
    [SerializeField] private GameObject _slimePrefab;
    [Tooltip("x - bottom left, y - bottom right , z - top left, w - top right")]
    [SerializeField] private Vector4 _spawnBorders;

    private List<GameObject> _slimes = new ();

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogError($"Two {name} could not exist");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

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

    public void RemoveSlime(GameObject slime) => _slimes.Remove(slime);

    public int GetSlimesCount() => _slimes.Count;
    
    
}
