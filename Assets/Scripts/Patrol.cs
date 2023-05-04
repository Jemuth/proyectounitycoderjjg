using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] public float monsterSpeed;
    [SerializeField] public float monsterTurningSpeed = 90;
    [SerializeField] public float waitTime = 2;
    [SerializeField] private Animator monsterAnimator;
    [SerializeField] private EnemyTypes m_enemyType;
    public Transform pathHolder;
    public enum EnemyTypes
    {
        Slow = 1,
        Medium = 2,
        Fast = 3
    }
    private void SetSpeedMultiplier(EnemyTypes p_enemyType)
    {
        switch (p_enemyType)
        {
            case EnemyTypes.Slow:
                monsterSpeed = 1;
                break;
            case EnemyTypes.Medium:
                monsterSpeed = 2;
                break;
            case EnemyTypes.Fast:
                monsterSpeed = 3.3f;
                break;
            
        }
    }
    private void Awake()
    {
        SetSpeedMultiplier(EnemyTypes.Slow);
    }

    private void Start()
    {
        monsterAnimator.SetBool("MonsterMoving", true);
        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }
        StartCoroutine (FollowPath(waypoints));
    }

    IEnumerator FollowPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);
        

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, monsterSpeed * Time.deltaTime);
            if (transform.position == targetWaypoint)
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                monsterAnimator.SetBool("MonsterMoving", false);
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine (TurnToFace (targetWaypoint));
            
            }
            yield return null;
        }
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2 (dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle (transform.eulerAngles.y, targetAngle, monsterTurningSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            monsterAnimator.SetBool("MonsterMoving", true);
            yield return null;
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;
        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawLine(previousPosition, waypoint.position);
            Gizmos.DrawSphere(waypoint.position, .3f);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
    }
    private void Update()
    {

        SetSpeedMultiplier(m_enemyType);

    }
}
