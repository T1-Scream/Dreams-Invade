using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class KillerMovement : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float velocity = -35f;
    public float rotateSpeed;
    public float nextWayPointDistance;
    public LayerMask playerLayer;
    public float sightRange;
    public float attackRange;
    private bool inAttackRange;
    private bool dead = false;

    private Rigidbody rb;
    private Animator killerAnimator;

    //FOV
    public float viewRadius;
    public float viewAngle;

    //Stare
    public int stareValue = 0;
    public float stareCDTime = 2f;
    private float nextStareTime = 0f;
    private float time;
    private float delayTime = 10f;
    private bool playOnce = false;

    //A*
    private Path path;
    private Seeker seeker;
    private int curWayPoint = 0;
    private bool inSideFOV;
    private float lastRepath = float.NegativeInfinity;
    private float repathRate = 0.5f;

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, viewRadius);

    //    Vector3 fovLine1 = Quaternion.AngleAxis(viewAngle, transform.up) * transform.forward * viewRadius;
    //    Vector3 fovLine2 = Quaternion.AngleAxis(-viewAngle, transform.up) * transform.forward * viewRadius;
    //    Gizmos.color = Color.cyan;
    //    Gizmos.DrawRay(transform.position, fovLine1);
    //    Gizmos.DrawRay(transform.position, fovLine2);

    //    if (!inSideFOV)
    //        Gizmos.color = Color.black;
    //    else
    //        Gizmos.color = Color.red;

    //    Gizmos.DrawRay(transform.position + Vector3.up, (target.position + - new Vector3(0,-.4f,0) +  Vector3.down - transform.position).normalized * viewRadius);
    //}

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponentInChildren<Rigidbody>();
        killerAnimator = GetComponentInChildren<Animator>();
        viewRadius = sightRange;
    }

    void UpdatePlayerPath()
    {
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error) {
            path = p;
            curWayPoint = 1;
        }
    }

    void MoveToTarget()
    {
        if (Time.time > lastRepath + repathRate && seeker.IsDone()) {
            lastRepath = Time.time;
            UpdatePlayerPath();
        }

        if (path == null) return;

        if (curWayPoint >= path.vectorPath.Count) return;

        Vector3 direction = (path.vectorPath[curWayPoint] - rb.position).normalized;
        Vector3 force = direction * speed * Time.fixedDeltaTime;
        rb.velocity = new Vector3(force.x, velocity, force.z);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotateSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);

        float distance = Vector3.Distance(rb.position, path.vectorPath[curWayPoint]);
        if (distance < nextWayPointDistance) {
            curWayPoint++;
            return;
        }
    }

    void ChasePlayer(bool boolean)
    {
        killerAnimator.SetBool("isWalking", boolean);

        if (boolean)
            MoveToTarget();

        if (stareValue >= 100) {
            FindObjectOfType<AudioManager>().Play("Chase", 0f);
            speed = 235;
            MoveToTarget();

            if (inAttackRange) AttackPlayer();
        }
    }

    void LookPlayer()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(target.position
            - transform.position), rotateSpeed * Time.deltaTime);

        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    void StarePlayer()
    {
        if (stareValue >= 100) return;

        if (stareValue < 100 && Time.time > nextStareTime) {
            stareValue += 2;
            nextStareTime = Time.time + stareCDTime;
        }

        if (stareValue >= 30 && stareValue <= 32 && !playOnce) {
            FindObjectOfType<AudioManager>().Play("Stare30", 0f);
            playOnce = true;
        }
        else if (stareValue >= 60 && stareValue <= 62 && playOnce) {
            FindObjectOfType<AudioManager>().Play("Stare60", 0f);
            playOnce = !playOnce;
        }
        else if (stareValue >= 90 && stareValue <= 92 && !playOnce) {
            FindObjectOfType<AudioManager>().Play("Stare90", 0f);
            playOnce = true;
        }
    }

    void AttackPlayer()
    {
        dead = true;
        FindObjectOfType<GameManager>().GameOver(dead);
        FindObjectOfType<AudioManager>().Stop("Chase");
    }

    void KillerAI() //                              prototype: Michael myers
    {
        time += 1f * Time.deltaTime;

        if (inSideFOV) { //                         All action need in FOV
            LookPlayer();
            StarePlayer();
            if (stareValue >= 100)
                ChasePlayer(true);
            else
                ChasePlayer(false);
        }
        else // keep moving
            ChasePlayer(true);

        if (time >= delayTime) {
            stareValue += 1;
            time = 0f;
        }
    }

    bool InFOV(Transform Killer, Transform Target, float angle, float radius)
    {
        Vector3 dir = (Target.position - Killer.position).normalized;
        dir.y *= 0;

        RaycastHit hit;
        int layerMask = 1 << 11; // ignore door collider

        if (Physics.Raycast(Killer.position + Vector3.up, (Target.position + -new Vector3(0, -.4f, 0) + Vector3.down - Killer.position).normalized, out hit, radius, ~layerMask)) {
            if (hit.collider.gameObject.CompareTag("Player")) {
                float eyeAngle = Vector3.Angle(Killer.forward, dir);
                if (eyeAngle <= angle)
                    return true;
            }
        }

        return false;
    }

    void BooleanPool()
    {
        // Raycast check
        inSideFOV = InFOV(transform, target, viewAngle, viewRadius);
        inAttackRange = Physics.CheckSphere(transform.position, attackRange, playerLayer);
    }

    public void KillerRampage(int value)
    {
        if (stareValue <= value)
            stareValue = value;
    }
    private void FixedUpdate()
    {
        BooleanPool();
        KillerAI();
    }
}
