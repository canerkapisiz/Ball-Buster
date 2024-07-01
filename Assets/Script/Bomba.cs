using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Bomba : MonoBehaviour
{
    [SerializeField] private int sayi;
    [SerializeField] private TextMeshProUGUI sayiText;
    [SerializeField] private GameManager gameManager;

    List<Collider2D> colliders = new List<Collider2D>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(sayi.ToString()))
        {
            GucUygula();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(sayi.ToString()))
        {
            GucUygula();
        }
    }

    void GucUygula()
    {
        var contactFilter2D = new ContactFilter2D
        {
            useTriggers = true
        };

        Physics2D.OverlapBox(transform.position, transform.localScale * 15, 20f, contactFilter2D, colliders);

        gameManager.PatlamaEfekti(transform.position);
        gameObject.SetActive(false);

        foreach (var item in colliders)
        {
            Debug.Log(item.gameObject.name);
            if (item.gameObject.CompareTag("kutu"))
            {
                item.gameObject.GetComponent<Kutu>().EfektOynat();
            }
            else
            {
                item.gameObject.GetComponent<Rigidbody2D>().AddForce(90 * new Vector2(0, 6), ForceMode2D.Force);
            }
        }
    }
}
