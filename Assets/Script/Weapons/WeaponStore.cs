using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStore : MonoBehaviour
{
    public GameObject weaponStorePanel;
    public GameObject weaponStoreScrollView;
    public GameObject weaponViewButton;
    public GameObject weaponMakeButton;
    public Image weaponIcon;
    public Text weaponNameText;
    public Text weaponMaterialsText;
    public Text weaponDetailText;

    List<WeaponRecipe> ViewRecipeList;
    public int currentRecipeNum;

    //武器生産のUIを更新する（初期化時や武器生産時に呼ばれる）
    public void MakeWeaponUIUpdate()
    {
        WeaponStoreUIInit();
        ViewRecipeList = CanChooseRecipeList();
        int currentButtonCount = weaponStoreScrollView.transform.childCount;
        int currentRecipeCount = ViewRecipeList.Count;
        if (currentRecipeCount > currentButtonCount)
        {
            for (int i = 0; i < currentRecipeCount - currentButtonCount; i++)
            {
                GameObject newButton = Instantiate(weaponViewButton);
                newButton.transform.SetParent(weaponStoreScrollView.transform, false);
            }
        }
        //前からやるとdestroy時にインデックスがずれるので、逆順にしている。
        if (currentRecipeCount < currentButtonCount)
        {
            for (int i = currentButtonCount - 1; i >= currentRecipeCount; i--)
            {
                Destroy(weaponStoreScrollView.transform.GetChild(i).gameObject);
            }
        }
        for (int i = 0; i < weaponStoreScrollView.transform.childCount; i++)
        {
            GameObject getButton = weaponStoreScrollView.transform.GetChild(i).gameObject;
            //ラムダ式にfor文のiを入れるとバグるので定義しなおす
            int idx = i;
            getButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = ViewRecipeList[i].CraftedWeapon.Icon;
            getButton.transform.GetChild(1).GetComponent<Text>().text = ViewRecipeList[i].CraftedWeapon.Name;

            getButton.transform.GetChild(2).GetComponent<Text>().text = ViewRecipeList[i].cost.ToString() + " G";
            if (IsHaveMaterials(ViewRecipeList[i], false) && GameManager.currentMoney >= ViewRecipeList[i].cost) getButton.transform.GetChild(3).GetComponent<Text>().text = "つくれる";
            else getButton.transform.GetChild(3).GetComponent<Text>().text = "つくれない";
            getButton.GetComponent<Button>().onClick.AddListener(()=> { WeaponViewButtonOnclick(idx); });
        }
    }

    //武器ショップUIを初期化する処理
    public void WeaponStoreUIInit()
    {
        currentRecipeNum = -1;
        weaponIcon.sprite = null;
        weaponNameText.text = "ぶきをせんたくして";
        weaponDetailText.text = "こうげきりょく　0\nりろーどじかん　0\n";
        weaponMaterialsText.text = "";
        weaponMakeButton.SetActive(false);
        weaponMakeButton.GetComponent<Button>().onClick.RemoveAllListeners();
        weaponMakeButton.GetComponent<Button>().onClick.AddListener(()=> { WeaponCraft(); });
    }

    //武器を作るレシピの内,製作の素材を持っているもののリスト (素材が製作必要数に達していないものも含む)
    public List<WeaponRecipe> CanChooseRecipeList()
    {
        List<WeaponRecipe> recipeList=new List<WeaponRecipe>();
        List<Recipe> recipeDB = GameManager.instance.Player.database.recipeDatabase;
        for (int idx = 0; idx < recipeDB.Count; idx++)
        {
            if (recipeDB[idx].GetType().ToString() != nameof(WeaponRecipe)) continue;
            
            if (IsHaveMaterials((WeaponRecipe)recipeDB[idx],true)) recipeList.Add((WeaponRecipe)recipeDB[idx]);
        }
        return recipeList;
    }

    public bool IsHaveMaterials(WeaponRecipe recipe,bool min)
    {
        for (int materialIdx = 0; materialIdx < recipe.materials.Count; materialIdx++)
        {
            if (GameManager.instance.Player.inventory.existItem(recipe.materials[materialIdx].item)&&min) continue;
            if (GameManager.instance.Player.inventory.numItem(recipe.materials[materialIdx].item) >= recipe.materials[materialIdx].num) continue;
            return false;
        }
        return true;
    }

    //生産画面で生産する武器が選択されたときに発生する画面表示等の処理
    public void WeaponViewButtonOnclick(int idx)
    {
        currentRecipeNum = idx;
        WeaponRecipe recipe = ViewRecipeList[currentRecipeNum];

        weaponIcon.sprite = recipe.CraftedWeapon.Icon;
        weaponNameText.text = recipe.CraftedWeapon.Name;
        weaponDetailText.text = "こうげきりょく　" + recipe.CraftedWeapon.At+"\n";
        weaponDetailText.text += "りろーどじかん　" + recipe.CraftedWeapon.DisTime + "\n";
        if(recipe.CraftedWeapon.cursor) weaponDetailText.text += "操作可能\n";
        if (recipe.CraftedWeapon.bakuhatu) weaponDetailText.text += "爆破攻撃\n";
        if (recipe.CraftedWeapon.penetrate) weaponDetailText.text += "貫通攻撃\n";
        if (recipe.CraftedWeapon.nagaoshi) weaponDetailText.text += "連続攻撃\n";
        weaponMakeButton.SetActive(IsHaveMaterials(recipe,false)&& GameManager.currentMoney >= recipe.cost);
        weaponMaterialsText.text = "";
        for (int materialIdx = 0; materialIdx < recipe.materials.Count; materialIdx++)
        {
            weaponMaterialsText.text += recipe.materials[materialIdx].item.itemname+" ×"+ recipe.materials[materialIdx].num+"\n";
        }
        weaponMaterialsText.text += recipe.cost + " G";
    }

    public void WeaponCraft()
    {
        if (GameManager.instance.Player.weaponPouch.max <= GameManager.instance.Player.weaponPouch.Pouch.Count) return;
        GameManager.instance.Player.weaponPouch.AddWeapon(ViewRecipeList[currentRecipeNum].CraftedWeapon);
        MakeWeaponUIUpdate();
    }

}
