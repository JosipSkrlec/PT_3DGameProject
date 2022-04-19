using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _health; // 100.0f
    [SerializeField] private float _enemySpeed; // 0.5f
    [SerializeField] private float _enemyDamage; // 25f
    [Space(5)]
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _playerLayer;
    [Space(5)]
    [SerializeField] private float _attackRadius;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Transform _attackPoint;

    #region Patrolling fields
    [SerializeField] private float _timeToEqualPosition = 0.1f; // TODO - check and fix this!
    [SerializeField] private Vector3 _walkPoint;
    [SerializeField] private float _walkPointRange;
    bool walkPointSet;
    #endregion

    #region Attacking fields
    [SerializeField] private float _timeBetweenAttacks;
    [SerializeField] private GameObject _projectile;
    bool alreadyAttacked;
    #endregion

    #region Enemy States
    [SerializeField] private float _sightRange;
    [SerializeField] private float _attackRange;
    [SerializeField] private bool _playerInSightRange;
    [SerializeField] private bool _playerInAttackRange;
    #endregion

    #region Private fields
    private float _enemyMaxHealth;
    private bool _equalThePosition;
    #endregion

    bool onetime = false;

    private void Awake()
    {
        _player = GameObject.Find("Player").transform; // TODO - do reference on a player!
        _agent = GetComponent<NavMeshAgent>();
        _enemyMaxHealth = _health;
        _equalThePosition = false;
    }

    private void Start()
    {
        // TODO - setup the variables!!
        _agent.speed = _enemySpeed;


    }

    private void Update()
    {
        //Check for sight and attack range overlap!
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _playerLayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _playerLayer);

        if (_playerInSightRange == false && _playerInAttackRange == false)
        {
            Patroling();
            _equalThePosition = false;

        }
        if (_playerInSightRange == true && _playerInAttackRange == false)
        {
            ChasePlayer();
            _equalThePosition = true;

            //if (onetime == false)
            //{
            //    onetime = true;
            //    _equalThePosition = true;

            //}
        }
        if (_playerInAttackRange == true && _playerInSightRange == true)
        {
            AttackPlayer();
        }

    }

    private void FixedUpdate()
    {
        if (_equalThePosition == true)
        {
            _equalThePosition = false;
            //Debug.Log("MOVE ENEMY To PLAYER POS!");
            //Vector3.Lerp(transform.position, _player.position,_timeToEqualPosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, _player.transform.position.z, _timeToEqualPosition* Time.deltaTime));
            //transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Lerp(transform.position.z, _player.position.z, 5.0f));
            //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z * 5.0f * Time.deltaTime);
        }
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            _agent.SetDestination(_walkPoint);

        Vector3 distanceToWalkPoint = transform.position - _walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-_walkPointRange, _walkPointRange);
        float randomX = Random.Range(-_walkPointRange, _walkPointRange);

        _walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(_walkPoint, -transform.up, 2f, _groundLayer))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        _equalThePosition = true;
        _animator.SetTrigger("Walk");
        _agent.SetDestination(_player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        _agent.SetDestination(transform.position);
        transform.LookAt(_player);

        if (!alreadyAttacked)
        {
            ///Attack code here
            //Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code
            ///
            _animator.SetTrigger("Attack");

            Collider[] colliders = Physics.OverlapSphere(_attackPoint.position, _attackRadius, _layerMask);
            foreach (Collider coll in colliders)
            {
                if (coll != null)
                {
                    Debug.Log(coll.gameObject.name);
                    StartCoroutine(TakeDamageAfterDelay(0.5f, coll.GetComponent<Rigidbody>()));
                    //coll.GetComponent<Rigidbody>().AddForce(transform.forward * 150f);
                }
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _timeBetweenAttacks);
        }
    }

    private IEnumerator TakeDamageAfterDelay(float delay, Rigidbody playerRB)
    {
        yield return new WaitForSeconds(delay);

        PlayerController player = playerRB.GetComponent<PlayerController>();

        player.TakeDamageToPlayer(_enemyDamage);

        // TODO - show on UI!


        // TODO - do different, this is just testing for mass changed!
        //enemyRB.AddForce(transform.forward * 150f);
        //enemyRB.mass -= 0.25f;

        //if (enemyRB.mass <= 0.1)
        //{
        //    Debug.Log("Destroying " + enemyRB.gameObject.name);
        //    Destroy(enemyRB.gameObject);
        //}
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamageToEnemy(float damage)
    {
        Debug.Log("Take damage " + damage + " to enemy!");
        _health -= damage;

        EnemyUIController.Instance.SetupUIEnemy(this.transform.name, _enemyMaxHealth, _health);

        if (_health <= 0) Invoke(nameof(Die), 0.5f);
    }

    private void Die()
    {
        // TODO - do enemy die not destroy!
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.DrawSphere(_attackPoint.position, _attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sightRange);

    }
}
