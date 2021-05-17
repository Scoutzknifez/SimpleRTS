using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : Unit
{
    //
    public FighterState state = FighterState.IDLE;
    public float range = 15f;
    public float seekingRange = 20f;
    public float damage = 10f;

    // Ammo
    public int ammo = 1;
    public int maxAmmo = 1;
    public float reloadTime = 1f;
    public bool isReloading = false;

    // Targeting
    public Unit target;
    public Unit lastDamager;
    public Unit lastDamaged;

    // Movement
    public float movementWaitTimeAfterAttacking = .5f;

    // Animation Specific
    public bool hasAnimation;

    private new void Awake()
    {
        base.Awake();
        ammo = maxAmmo;
    }

    private void FixedUpdate()
    {
        if (hasCustomJob)
        {
            if (state == FighterState.GOING && GetAgent().hasPath)
            {
                return;
            }
            else
            {
                state = FighterState.IDLE;
                hasCustomJob = false;
            }
        }

        if (target == null || target.isDead())
        {
            UpdateTarget();
        }

        EvaluateGoal();
    }

    private void EvaluateGoal()
    {
        if (target != null)
        {
            // Check range
            if (inRangeOfTarget())
            {
                // Attack
                AttackTarget();
            }
            else
            {
                // Move 1 meter towards them
                if (!CanMove())
                    return;

                state = FighterState.GOING;
                MoveTo(target.transform.position);
            }
        }
    }

    bool inRangeOfTarget()
    {
        return Vector3.Distance(transform.position, target.transform.position) <= range;
    }

    void AttackTarget()
    {
        if(GetAgent().hasPath)
            GetAgent().ResetPath();

        LookAt(target.transform.position);

        if (isReloading)
            return;

        state = FighterState.FIGHTING;
        ac.isAttacking = true;
        target.TakeDamage(damage);
        ammo--;
        StartCoroutine(CanMoveAgain());

        if (ammo <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    IEnumerator CanMoveAgain()
    {
        yield return new WaitForSeconds(movementWaitTimeAfterAttacking);

        state = FighterState.IDLE;
    }

    bool CanMove()
    {
        return state == FighterState.GOING || state == FighterState.IDLE || state == FighterState.PATROLLING;
    }

    IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        ammo = maxAmmo;
        isReloading = false;
    }

    void UpdateTarget()
    {
        if (lastDamaged != null && !lastDamaged.isDead() && !OnSameTeamAs(lastDamaged))
        {
            SetTarget(lastDamaged);
        }
        else if (lastDamager != null && !lastDamager.isDead() && !OnSameTeamAs(lastDamager))// && Vector3.Distance(transform.position, target.gameObject.transform.position) <= seekingRange)
        {
            SetTarget(lastDamager);
        }
        else
        {
            // Search around to attack something
            Collider[] collidersAround = Physics.OverlapSphere(transform.position, seekingRange);

            foreach (Collider collider in collidersAround)
            {
                Unit unit = collider.GetComponent<Unit>();

                if (unit != null && !OnSameTeamAs(unit))
                {
                    SetTarget(unit);
                    break;
                }
            }
        }
    }

    void SetTarget(Unit _target)
    {
        if (OnSameTeamAs(_target))
            return;

        target = _target;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, seekingRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

public enum FighterState
{
    FIGHTING,
    IDLE,
    GOING,
    DEFENDING,
    PATROLLING
}