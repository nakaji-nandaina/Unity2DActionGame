using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotManager : MonoBehaviour
{
    private Vector2 targetPos, direction, SpawnPos;
    private Quaternion shotRotate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EmemyShot(Vector2 PlayerPos, Vector2 EnemyPos, Vector2 attackDir, GameObject shotWeapon)
    {
        targetPos = PlayerPos;
        SpawnPos = EnemyPos + (PlayerPos - EnemyPos).normalized * 0.7f;
        direction = attackDir.normalized;
        shotRotate = Quaternion.Euler(0, 0,
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90);
        GameObject ShotObj = Instantiate(shotWeapon,
            SpawnPos, shotRotate);
        ShotObj.GetComponent<EnemyWeapon>().moveDirection(direction);
        //Debug.Log("shot");
    }
}
