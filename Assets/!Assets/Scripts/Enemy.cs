using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _health;
    [Space(5)]
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _groundLayer, _playerLayer;

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
    [SerializeField] private float _sightRange, _attackRange;
    [SerializeField] private bool _playerInSightRange, _playerInAttackRange;
    #endregion

    private void Awake()
    {
        _player = GameObject.Find("Player").transform; // TODO - do reference on a player!
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _playerLayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _playerLayer);

        if (!_playerInSightRange && !_playerInAttackRange) Patroling();
        if (_playerInSightRange && !_playerInAttackRange) ChasePlayer();
        if (_playerInAttackRange && _playerInSightRange) AttackPlayer();
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

    public void TakeDamage(int damage)
    {
        _health -= damage;

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
