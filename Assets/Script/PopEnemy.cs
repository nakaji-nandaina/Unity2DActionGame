using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopEnemy : MonoBehaviour
{
    [SerializeField,Tooltip("出現モンスター")]
    private GameObject[] Enemys;
    [SerializeField,Tooltip("発生数")]
    private int OccNum;
    [SerializeField, Tooltip("再発生の有無")]
    private bool IsRepop=false;
    [SerializeField, Tooltip("再発生時間")]
    private float RepopTime;
    private float RepopCounter;
    // Start is called before the first frame update
    void Start()
    {
        RepopCounter = RepopTime;
        for (int i = 0; i < OccNum; i++)
        {
            int enemy = Random.Range(0, Enemys.Length);
            GameObject popenemy = Instantiate(Enemys[enemy], this.transform.position,this.transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.Player.ps == PlayerController.PS.normal) return;
        if (!IsRepop) return;
        RepopCounter -= Time.deltaTime;
        if (RepopCounter <= 0&&15f<Mathf.Sqrt(Mathf.Pow(this.transform.position.x-GameManager.instance.Player.transform.position.x,2)+ Mathf.Pow(this.transform.position.y - GameManager.instance.Player.transform.position.y, 2)))
        {
            int enemy = Random.Range(0, Enemys.Length);
            GameObject popenemy = Instantiate(Enemys[enemy], this.transform.position, this.transform.rotation);
            RepopCounter = RepopTime;
        }
    }
}
