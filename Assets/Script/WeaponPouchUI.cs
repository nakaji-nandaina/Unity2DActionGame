using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPouchUI : MonoBehaviour
{
    public GameObject WeaponPouchPanel;
    public GameObject contentHolder;
    public GameObject buttonprefab;
    public GameObject setButton;
    public GameObject destButton;
    //右側に表示する選択中のアイテムの説明
    public Image selectedImage;
    public Text selectedExplane;
    public Text selectedName;
    //左側に表示する装備中の武器と変更後の武器
    public Image currentWeaponImage;
    public Image nextWeaponImage;
    public Text currentWeaponName;
    public Text nextWeaponName;
    public Text currentWeaponAttack;
    public Text nextWeaponAttack;

    public int selectedWeapon;
    bool CanFunc;
    public void Start()
    {
        selectedWeapon = 0;
        CanFunc = false;
        setButton.GetComponent<Button>().onClick.AddListener(setMainWeapon);
        destButton.GetComponent<Button>().onClick.AddListener(destWeapon);
    }

    public void InitiateWeaponPouch(WeaponPouch weaponPouch)
    {
        selectedWeapon = GameManager.instance.Player.mainWeapon;
        selectedName.text = weaponPouch.Pouch[selectedWeapon].Name;
        selectedImage.sprite = weaponPouch.Pouch[selectedWeapon].Icon;
        currentWeaponName.text = weaponPouch.Pouch[selectedWeapon].Name;
        currentWeaponAttack.text=weaponPouch.Pouch[selectedWeapon].At.ToString();
        currentWeaponImage.sprite = weaponPouch.Pouch[selectedWeapon].Icon;
        nextWeaponName.text = weaponPouch.Pouch[selectedWeapon].Name;
        nextWeaponAttack.text = weaponPouch.Pouch[selectedWeapon].At.ToString();
        nextWeaponImage.sprite = weaponPouch.Pouch[selectedWeapon].Icon;
        SetExplane(selectedWeapon);
    }

    public void SetExplane(int num)
    {
        selectedExplane.text = "こうげきりょく　　" + GameManager.instance.Player.weaponPouch.Pouch[num].At.ToString() + "\n";
        selectedExplane.text += "りろーどじかん　　" + GameManager.instance.Player.weaponPouch.Pouch[num].DisTime.ToString("F1") + "\n";
        if (GameManager.instance.Player.weaponPouch.Pouch[selectedWeapon].penetrate) selectedExplane.text += "貫通攻撃\n";
        if (GameManager.instance.Player.weaponPouch.Pouch[selectedWeapon].nagaoshi) selectedExplane.text += "連続攻撃\n";
        if (GameManager.instance.Player.weaponPouch.Pouch[selectedWeapon].cursor) selectedExplane.text += "操作可能\n";
    }
    
    public void UpdateWeaponPouchUI(WeaponPouch weaponPouch)
    {
        int currentButtonCount = contentHolder.transform.childCount;
        int currentWeaponCount = weaponPouch.Pouch.Count;

        if (currentButtonCount < currentWeaponCount)
        {
            int num = currentWeaponCount - currentButtonCount;
            for(int i = 0; i < num; i++)
            {
                GameObject newButton = Instantiate(buttonprefab);
                newButton.transform.SetParent(contentHolder.transform, false);
            }
        }
        if (currentWeaponCount < currentButtonCount)
        {
            for(int i = currentButtonCount-1; i >= currentWeaponCount; i--)
            {
                Destroy(contentHolder.transform.GetChild(i).gameObject);
            }
        }

        for(int i=0; i < currentWeaponCount; i++)
        {
            GameObject Currentbutton =  contentHolder.transform.GetChild(i).gameObject;
            Currentbutton.transform.GetChild(0).GetComponent<Image>().sprite = weaponPouch.Pouch[i].Icon;
            int num = i;
            Currentbutton.GetComponent<Button>().onClick.AddListener(()=>buttonFunc(num));

        }
    }

    public void buttonFunc(int num)
    {
        selectedWeapon = num;
        selectedName.text= GameManager.instance.Player.weaponPouch.Pouch[selectedWeapon].Name;
        selectedImage.sprite= GameManager.instance.Player.weaponPouch.Pouch[selectedWeapon].Icon;
        nextWeaponImage.sprite = GameManager.instance.Player.weaponPouch.Pouch[selectedWeapon].Icon;
        nextWeaponName.text = GameManager.instance.Player.weaponPouch.Pouch[selectedWeapon].Name;
        nextWeaponAttack.text = GameManager.instance.Player.weaponPouch.Pouch[selectedWeapon].At.ToString();
        SetExplane(selectedWeapon);
        CanFunc = true;
    }

    public void setMainWeapon()
    {
        if (!CanFunc) return;
        GameManager.instance.Player.mainWeapon = selectedWeapon;
        currentWeaponImage.sprite = GameManager.instance.Player.weaponPouch.Pouch[selectedWeapon].Icon;
        currentWeaponName.text = GameManager.instance.Player.weaponPouch.Pouch[selectedWeapon].Name;
        currentWeaponAttack.text = GameManager.instance.Player.weaponPouch.Pouch[selectedWeapon].At.ToString();
        UpdateWeaponPouchUI(GameManager.instance.Player.weaponPouch);
    }

    public void destWeapon()
    {
        if (!CanFunc) return;
        if (selectedWeapon == GameManager.instance.Player.mainWeapon) return;
        if (selectedWeapon < GameManager.instance.Player.mainWeapon)GameManager.instance.Player.mainWeapon--;
        GameManager.instance.Player.weaponPouch.Pouch.RemoveAt(selectedWeapon);
        UpdateWeaponPouchUI(GameManager.instance.Player.weaponPouch);
        nextWeaponImage.sprite = GameManager.instance.Player.weaponPouch.Pouch[GameManager.instance.Player.mainWeapon].Icon;
        nextWeaponName.text = GameManager.instance.Player.weaponPouch.Pouch[GameManager.instance.Player.mainWeapon].Name;
        nextWeaponAttack.text = GameManager.instance.Player.weaponPouch.Pouch[GameManager.instance.Player.mainWeapon].At.ToString();
        CanFunc = false;
    }
}
