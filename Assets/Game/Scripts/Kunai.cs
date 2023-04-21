using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kunai : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] GameObject hitVFX;
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.right * 5f;
        OnInit();
    }

    // Update is called once per frame
    public void OnInit()
    {
        rb.velocity = transform.right * 4f;
        Invoke(nameof(OnDespawn), 4f);
    }
    public void OnDespawn()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            collision.GetComponent<Character>().OnHit(30f);
            Instantiate(hitVFX,transform.position,transform.rotation);
            OnDespawn();
        }
    }
}
