using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class Hedefler
{
    public Sprite hedefGorsel;
    public int topDegeri;
    public GameObject gorevTamam;
    public string hedefTuru;
}

[Serializable]
public class HedeflerUI
{ 
    public GameObject hedef;
    public Image hedefGorsel;
    public TextMeshProUGUI hedefDegerText;
    public GameObject gorevTamam;
}

public class GameManager : MonoBehaviour
{
    [Header("LEVEL AYARLARI")]
    public Sprite[] spriteObjeleri;
    [SerializeField] private GameObject[] toplar;
    [SerializeField] private TextMeshProUGUI kalanTopSayisiText;
    int kalanTopSayisi;
    int havuzIndex;

    [Header("TOP ATIS SISTEMI")]
    [SerializeField] private GameObject topAtici;
    [SerializeField] private GameObject topSoketi;
    [SerializeField] private GameObject gelecekTop;
    GameObject seciliTop;

    [Header("DIGER OBJELER")]
    [SerializeField] private ParticleSystem patlamaEfekt;
    [SerializeField] private ParticleSystem[] kutuKirilmaEfetkleri;
    int kutuKirilmaEfektIndex;

    [Header("GOREV ISLEMLERI")]
    [SerializeField] private List<HedeflerUI> hedeflerUI;
    [SerializeField] private List<Hedefler> hedefler;
    int topDegeri, kutuDegeri, toplamGorevSayisi;
    public bool topHedefiVarMi;
    bool kutuHedefiVarMi;

    [Header("SESLER VE UI")]
    [SerializeField] private AudioSource[] sesler;
    [SerializeField] private GameObject[] paneller;

    // 2 - KIRMIZI
    // 4 - SARI
    // 8 - YESIL
    // 16 - MAVI
    // 32 - KOYU MAVI
    // 64 - KOYU YESIL
    // 128 - MOR
    // 256 - TURUNCU
    // 512 - KARISIK SARI
    // 1024
    // 2048

    void Start()
    {
        kalanTopSayisi = toplar.Length;
        TopGetir(true);
        toplamGorevSayisi = hedefler.Count;
        for (int i = 0; i < hedefler.Count; i++)
        {
            hedeflerUI[i].hedef.SetActive(true);
            hedeflerUI[i].hedefGorsel.sprite = hedefler[i].hedefGorsel;
            hedeflerUI[i].hedefDegerText.text = hedefler[i].topDegeri.ToString();
            if (hedefler[i].hedefTuru == "top")
            {
                topHedefiVarMi = true;
                topDegeri = hedefler[i].topDegeri;
            }
            else if (hedefler[i].hedefTuru == "kutu")
            {
                kutuHedefiVarMi = true;
                kutuDegeri = hedefler[i].topDegeri;
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("oyunZemini"))
                {
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    topAtici.transform.position = Vector2.MoveTowards(topAtici.transform.position,
                        new Vector2(mousePosition.x, topAtici.transform.position.y), 30 * Time.deltaTime);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            seciliTop.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            seciliTop.transform.parent = null;
            seciliTop.GetComponent<Top>().BirincilDurumuDegistir();
            TopGetir(false);
        }
    }

    void TopGetir(bool ilkKurulum)
    {
        if (ilkKurulum)
        {
            toplar[havuzIndex].transform.SetParent(topAtici.transform);
            toplar[havuzIndex].transform.position = topSoketi.transform.position;
            toplar[havuzIndex].SetActive(true);
            seciliTop = toplar[havuzIndex];

            havuzIndex++;

            toplar[havuzIndex].transform.position = gelecekTop.transform.position;
            toplar[havuzIndex].SetActive(true);
            kalanTopSayisiText.text = kalanTopSayisi.ToString();
        }
        else
        {
            if (toplar.Length != 0)
            {
                toplar[havuzIndex].transform.SetParent(topAtici.transform);
                toplar[havuzIndex].transform.position = topSoketi.transform.position;
                toplar[havuzIndex].SetActive(true);
                seciliTop = toplar[havuzIndex];

                kalanTopSayisi--;
                kalanTopSayisiText.text = kalanTopSayisi.ToString();

                if (havuzIndex != toplar.Length - 1)
                {

                    havuzIndex++;

                    toplar[havuzIndex].transform.position = gelecekTop.transform.position;
                    toplar[havuzIndex].SetActive(true);
                    
                }
            }

            if(kalanTopSayisi == 0)
            {
                Invoke("GorevleriKontrolEt", 3f);
            }
        }
    }

    public void PatlamaEfekti(Vector2 pozisyon)
    {
        patlamaEfekt.gameObject.transform.position = pozisyon;
        patlamaEfekt.gameObject.SetActive(true);
        patlamaEfekt.Play();
        sesler[2].Play();
    }

    public void KutuParcalamaEfekt(Vector2 pozisyon)
    {
        kutuKirilmaEfetkleri[kutuKirilmaEfektIndex].gameObject.transform.position = pozisyon;
        kutuKirilmaEfetkleri[kutuKirilmaEfektIndex].gameObject.SetActive(true);
        patlamaEfekt.Play();
        sesler[1].Play();

        if (kutuHedefiVarMi)
        {
            kutuDegeri--;

            if (kutuDegeri == 0)
            {
                hedeflerUI[1].gorevTamam.SetActive(true);
                sesler[3].Play();
                toplamGorevSayisi--;
                if (toplamGorevSayisi == 0)
                {
                    Kazandin();
                }
            }
        }
       
        if(kutuKirilmaEfektIndex == kutuKirilmaEfetkleri.Length - 1)
        {
            kutuKirilmaEfektIndex = 0;
        }
        else
        {
            kutuKirilmaEfektIndex++;
        }
    }

    public void GorevSayiKontrol(int sayi)
    {
        if(sayi == topDegeri)
        {
            hedeflerUI[0].gorevTamam.SetActive(true);

            toplamGorevSayisi--;
            sesler[3].Play();
            if (toplamGorevSayisi == 0)
            {
                Kazandin();
            }
        }
    }

    void GorevleriKontrolEt()
    {
        if (toplamGorevSayisi == 0)
        {
            Kazandin();
        }
        else
        {
            Kaybettin();
        }
    }

    void Kazandin()
    {
        sesler[4].Play();
        paneller[0].SetActive(true);
    }

    void Kaybettin()
    {
        sesler[5].Play();
        paneller[1].SetActive(true);
    }

    public void TopBirlestirmSes()
    {
        sesler[0].Play();
    }

    public void PanelButonlari(int index)
    {
        if(index == 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
