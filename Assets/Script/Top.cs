using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Top : MonoBehaviour
{
    [SerializeField] private int sayi;
    [SerializeField] private TextMeshProUGUI sayiText;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ParticleSystem birlesmeEfekt;
    [SerializeField] private SpriteRenderer rend;

    bool birincil;
    [SerializeField] private bool varsayilanTop;

    void Start()
    {
        sayiText.text = sayi.ToString();

        if (varsayilanTop)
        {
            birincil = true;
        }
    }

    void DurumuAyarla()
    {
        birincil = true;
    }

    public void BirincilDurumuDegistir()
    {
        Invoke("DurumuAyarla", 2f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(sayi.ToString())  && birincil)
        { 
            birlesmeEfekt.Play();
            gameManager.TopBirlestirmSes();
            collision.gameObject.SetActive(false);
            sayi += sayi;
            gameObject.tag = sayi.ToString();
            sayiText.text = sayi.ToString();
            switch (sayi)
            {
                case 4:
                    rend.sprite = gameManager.spriteObjeleri[1];
                    break;
                case 8:
                    rend.sprite = gameManager.spriteObjeleri[2];
                    break;
                case 16:
                    rend.sprite = gameManager.spriteObjeleri[3];
                    break;
                case 32:
                    rend.sprite = gameManager.spriteObjeleri[4];
                    break;
                case 64:
                    rend.sprite = gameManager.spriteObjeleri[5];
                    break;
                case 128:
                    rend.sprite = gameManager.spriteObjeleri[6];
                    break;
                case 256:
                    rend.sprite = gameManager.spriteObjeleri[7];
                    break;
                case 512:
                case 1024:
                case 2048:
                    rend.sprite = gameManager.spriteObjeleri[8];
                    break;
            }

            if (gameManager.topHedefiVarMi)
            {
                gameManager.GorevSayiKontrol(sayi);
            }
            birincil = false;
            Invoke("DurumuAyarla", 2f);
        } 
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(sayi.ToString()) && birincil)
        {
            birlesmeEfekt.Play();
            collision.gameObject.SetActive(false);
            sayi += sayi;
            gameObject.tag = sayi.ToString();
            sayiText.text = sayi.ToString();
            switch (sayi)
            {
                case 4:
                    rend.sprite = gameManager.spriteObjeleri[1];
                    break;
                case 8:
                    rend.sprite = gameManager.spriteObjeleri[2];
                    break;
                case 16:
                    rend.sprite = gameManager.spriteObjeleri[3];
                    break;
                case 32:
                    rend.sprite = gameManager.spriteObjeleri[4];
                    break;
                case 64:
                    rend.sprite = gameManager.spriteObjeleri[5];
                    break;
                case 128:
                    rend.sprite = gameManager.spriteObjeleri[6];
                    break;
                case 256:
                    rend.sprite = gameManager.spriteObjeleri[7];
                    break;
                case 512:
                case 1024:
                case 2048:
                    rend.sprite = gameManager.spriteObjeleri[8];
                    break;
            }

            birincil = false;
            Invoke("DurumuAyarla", 2f);
        }
    }
}
