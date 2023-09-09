using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShotManager : MonoBehaviour
{
    private Vector2 targetPos, direction, SpawnPos;
    private Quaternion shotRotate;

    public void ShotAttack(Vector2 PlayerPos,  Vector2 attackDir,GameObject shotWeapon,int at,float kbforce,WeaponData wd)
    {
        //SpawnPos = PlayerPos + (MousePos - PlayerPos).normalized*0.7f;
        direction = attackDir.normalized;
        SpawnPos = PlayerPos +direction*0.7f;
        shotRotate = Quaternion.Euler(0, 0,
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg-90);
        GameObject ShotObj = Instantiate(shotWeapon,
            SpawnPos, shotRotate);
        ShotObj.GetComponent<Weapon>().WD = wd;
        ShotObj.GetComponent<Weapon>().weaponDirection=direction;
        ShotObj.GetComponent<Weapon>().attackDamage = at+wd.At;
        ShotObj.GetComponent<Weapon>().kbforce = kbforce+wd.KbForce;
    }
}
