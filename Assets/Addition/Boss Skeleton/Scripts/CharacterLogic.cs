using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLogic : MonoBehaviour, IEntity
{
    public GameObject target = null;
    public GameObject player = null;
    public float speed = 2f;
    public float rayDistance = 1f;
    public float agressiveDistatnce = 5f;

    private float healthPoints = 1000;

    public float maxAngleVision;
    public float maxRadiusVision = 10f;


    private Rigidbody2D rb;
    private float rightPatrolBorder;
    private float leftPatrolBorder;

    public static Vector2 movingDirection = Vector2.right;

    public enum allAnimations { Staying, Running, Dying, Attack, GetHit };
    private static allAnimations currentAnimation = allAnimations.Staying;

    public static allAnimations CurrentAnimation { get => currentAnimation; set => currentAnimation = value; }
    public float HealthPoints { get => healthPoints; set => healthPoints = value; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectsWithTag("Player")[0];

        rightPatrolBorder = transform.position.x + 8;
        leftPatrolBorder = transform.position.x - 8;

        Physics2D.queriesStartInColliders = false;
        StartCoroutine(GoToMove());
    }
    IEnumerator GoToMove()
    {
        yield return new WaitForSeconds(Random.Range(0, 2));
        currentAnimation = allAnimations.Running;
    }


    void Update()
    {
        if (currentAnimation != allAnimations.Dying)
        {
            Vector2 a = (player.transform.position - transform.position).normalized;
            RaycastHit2D visionHit = Physics2D.Raycast(
                transform.position, 
                (player.transform.position - transform.position).normalized,
                maxRadiusVision);
            if (visionHit.collider != null)
            {
                if (visionHit.collider.gameObject.tag == "Player")
                {
                    if (player.transform.position.x > transform.position.x && movingDirection == Vector2.right)
                    {
                        target = player;
                    }
                    else if (player.transform.position.x < transform.position.x && movingDirection == Vector2.left)
                    {
                        target = player;
                    }
                }
                else
                {
                    target = null;
                }
            }
            else
            {
                target = null;
            }

            if (target == player)
            {
                if (player.transform.position.x > transform.position.x && movingDirection != Vector2.right)
                {
                    TurnAround();
                }
                else if (player.transform.position.x < transform.position.x && movingDirection != Vector2.left)
                {
                    TurnAround();
                }
            }


            if (CurrentAnimation == allAnimations.Running)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
            }

            if (transform.position.x >= Random.Range(rightPatrolBorder - 2, rightPatrolBorder) && target == null && movingDirection == Vector2.right)
            {
                TurnAround();
            }
            else if (transform.position.x <= Random.Range(leftPatrolBorder, leftPatrolBorder + 2) && target == null && movingDirection == Vector2.left)
            {
                TurnAround();
            }

            RaycastHit2D hit = Physics2D.Raycast(transform.position, movingDirection, rayDistance);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject == player)
                {
                    CurrentAnimation = allAnimations.Attack;
                    EndAttack();
                }
                else
                {
                    TurnAround();
                }
            }
            else
            {
                CurrentAnimation = allAnimations.Running;
            } 
        }
    }

    public void Damage(float damage)
    {
        healthPoints -= damage;
        Debug.Log(healthPoints);
        if(healthPoints > 0)
        {
            CurrentAnimation = allAnimations.GetHit;
        }
        else
        {
            CurrentAnimation = allAnimations.Dying;
        }

    }

    private void EndAttack()
    {
        Debug.Log("end attacs  " + (player.transform.position.x - transform.position.x));
        if (player.transform.position.x - transform.position.x < 1.7f )//&& player.transform.position.x - transform.position.x > 1.2f)
            {
                Debug.Log("aaa");
                player.GetComponent<IEntity>().Damage(1);
            }
    }

    private void EndGetHit()
    {
        CurrentAnimation = allAnimations.Staying;
    }

    private void EndDead()
    {
       CharacterAnim.anim.enabled = false;
    }
    private void TurnAround()
    {
        if (movingDirection == Vector2.right)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
            movingDirection = Vector2.left;
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            movingDirection = Vector2.right;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * rayDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * rayDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(player.transform.position.x - transform.position.x, player.transform.position.y - transform.position.y, 0));
        Gizmos.DrawWireSphere(transform.position, maxRadiusVision);
    }
}
