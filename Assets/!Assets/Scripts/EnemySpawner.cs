using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField] private Transform _interactiveBoxToSpawn;
    [SerializeField] private bool _spawnAtStart = false;
    [SerializeField] private bool _spawnOnce = true;
    [Space(5)]
    [SerializeField] private float _gizmosRadius;
    [SerializeField] private List<ObjectToSpawnDefinition> _objectsToSpawn = new List<ObjectToSpawnDefinition>();

    private bool _spawned = false;
    //private void OnCollisionEnter(Collision collision)
    //{

    //}

    private void Start()
    {
        if (_spawnAtStart == true)
        {
            _spawned = true;
            SpawnObjects();
        }
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (_spawned == true)
        {
            return;
        }

        if (collision.gameObject.tag == "Player")
        {
            if (_spawnOnce == true)
            {
                _spawned = true;
            }
            SpawnObjects();
        }
    }

    private void SpawnObjects()
    {
        Debug.Log("Player In -> Spawn objects!");
        foreach (ObjectToSpawnDefinition objectInfo in _objectsToSpawn)
        {
            GameObject go =  Instantiate(objectInfo._objectToSpawn, objectInfo._objectSpawnPosition, Quaternion.identity);
            go.GetComponent<EnemyController>().SetupSettings(objectInfo._enemyName, objectInfo._enemyDifficulty);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        foreach (ObjectToSpawnDefinition objectInfo in _objectsToSpawn)
        {
            Gizmos.DrawSphere(objectInfo._objectSpawnPosition, _gizmosRadius);
        }
    }

}

[Serializable]
public class ObjectToSpawnDefinition
{
    public GameObject _objectToSpawn;
    public string _enemyName;
    [Tooltip("Configured in EnemySettingsManager -> scene GameObject")]public EnemyDifficulty _enemyDifficulty;
    public Vector3 _objectSpawnPosition;
}