using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField] private Transform _interactiveBoxToSpawn;
    [SerializeField] private bool _spawnOnce = true;
    [Space(5)]
    [SerializeField] private float _gizmosRadius;
    [SerializeField] private List<ObjectToSpawnDefinition> _objectsToSpawn = new List<ObjectToSpawnDefinition>();

    private bool _spawned = false;
    //private void OnCollisionEnter(Collision collision)
    //{

    //}

    private void OnTriggerEnter(Collider collision)
    {
        if (_spawned == true)
        {
            return;
        }

        if (collision.gameObject.tag == "Player")
        {
            _spawned = true;
            Debug.Log("Player In -> Spawn objects!");
            foreach (ObjectToSpawnDefinition objectInfo in _objectsToSpawn)
            {
                Instantiate(objectInfo._objectToSpawn, objectInfo._objectSpawnPosition, Quaternion.identity);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        foreach (ObjectToSpawnDefinition objectInfo in _objectsToSpawn)
        {
            Gizmos.DrawSphere(objectInfo._objectSpawnPosition,_gizmosRadius);
        }
    }

}

[Serializable]
public class ObjectToSpawnDefinition
{
    public GameObject _objectToSpawn;
    public Vector3 _objectSpawnPosition;
}
