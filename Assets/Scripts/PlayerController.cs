using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _defenceSpawnTime;
    [SerializeField] private GameObject _defencePrefab;
    [SerializeField] private float _defenceLifeTime;

    private float _defenceSpawnCooldown;
    private bool _canSpawnDefence = true;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnDefense();
        }

        if (!_canSpawnDefence)
        {
            _defenceSpawnCooldown -= Time.deltaTime;
            if (_defenceSpawnCooldown <= 0)
                _canSpawnDefence = true;

        }
    }

    private void SpawnDefense()
    {
        if(!_canSpawnDefence)
            return;
        
        _canSpawnDefence = false;
        _defenceSpawnCooldown = _defenceSpawnTime;

        if (Camera.main != null)
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject defence = Instantiate(_defencePrefab, position, Quaternion.identity);
            StartCoroutine(DestroyGameObjectCoroutine(defence, _defenceLifeTime));
        }
    }

    private IEnumerator DestroyGameObjectCoroutine(GameObject gameObjectToDestroy, float time)
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        Destroy(gameObjectToDestroy);
    }
}
