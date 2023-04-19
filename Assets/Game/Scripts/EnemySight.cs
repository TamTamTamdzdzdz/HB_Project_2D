using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour
{
    public Enemy enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            enemy.SetTarget(enemy.GetComponent<Character>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        enemy.SetTarget(null );
    }

}
