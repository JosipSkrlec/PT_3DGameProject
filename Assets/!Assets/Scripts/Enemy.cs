using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _health;
    [Space(5)]
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _playerLayer;

    #region Patrolling fields
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
    [SerializeField] private float _timeToEqualPosition = 0.1f;
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
            Debug.Log("MOVE ENEMY To PLAYER POS!");
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
            _animator.SetTrigger("Attack");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Take damage " + damage + " to enemy!");
        _health -= damage;

        EnemyBarController.Instance.SetupUIEnemy(this.transform.name, _enemyMaxHealth, _health);


        if (_health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _sightRange);
    }
}
