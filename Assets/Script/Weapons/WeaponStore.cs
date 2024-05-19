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

    //���퐶�Y��UI���X�V����i���������═�퐶�Y���ɌĂ΂��j
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
        //�O�������destroy���ɃC���f�b�N�X�������̂ŁA�t���ɂ��Ă���B
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
            //�����_����for����i������ƃo�O��̂Œ�`���Ȃ���
            int idx = i;
            getButton.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = ViewRecipeList[i].CraftedWeapon.Icon;
            getButton.transform.GetChild(1).GetComponent<Text>().text = ViewRecipeList[i].CraftedWeapon.Name;

            getButton.transform.GetChild(2).GetComponent<Text>().text = ViewRecipeList[i].cost.ToString() + " G";
            if (IsHaveMaterials(ViewRecipeList[i], false) && GameManager.currentMoney >= ViewRecipeList[i].cost) getButton.transform.GetChild(3).GetComponent<Text>().text = "�����";
            else getButton.transform.GetChild(3).GetComponent<Text>().text = "����Ȃ�";
            getButton.GetComponent<Button>().onClick.AddListener(()=> { WeaponViewButtonOnclick(idx); });
        }
    }

    //����V���b�vUI�����������鏈��
    public void WeaponStoreUIInit()
    {
        currentRecipeNum = -1;
        weaponIcon.sprite = null;
        weaponNameText.text = "�Ԃ������񂽂�����";
        weaponDetailText.text = "����������傭�@0\n���[�ǂ�����@0\n";
        weaponMaterialsText.text = "";
        weaponMakeButton.SetActive(false);
        weaponMakeButton.GetComponent<Button>().onClick.RemoveAllListeners();
        weaponMakeButton.GetComponent<Button>().onClick.AddListener(()=> { WeaponCraft(); });
    }

    //�������郌�V�s�̓�,����̑f�ނ������Ă�����̂̃��X�g (�f�ނ�����K�v���ɒB���Ă��Ȃ����̂��܂�)
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

    //���Y��ʂŐ��Y���镐�킪�I�����ꂽ�Ƃ��ɔ��������ʕ\�����̏���
    public void WeaponViewButtonOnclick(int idx)
    {
        currentRecipeNum = idx;
        WeaponRecipe recipe = ViewRecipeList[currentRecipeNum];

        weaponIcon.sprite = recipe.CraftedWeapon.Icon;
        weaponNameText.text = recipe.CraftedWeapon.Name;
        weaponDetailText.text = "����������傭�@" + recipe.CraftedWeapon.At+"\n";
        weaponDetailText.text += "���[�ǂ�����@" + recipe.CraftedWeapon.DisTime + "\n";
        if(recipe.CraftedWeapon.cursor) weaponDetailText.text += "����\\n";
        if (recipe.CraftedWeapon.bakuhatu) weaponDetailText.text += "���j�U��\n";
        if (recipe.CraftedWeapon.penetrate) weaponDetailText.text += "�ђʍU��\n";
        if (recipe.CraftedWeapon.nagaoshi) weaponDetailText.text += "�A���U��\n";
        weaponMakeButton.SetActive(IsHaveMaterials(recipe,false)&& GameManager.currentMoney >= recipe.cost);
        weaponMaterialsText.text = "";
        for (int materialIdx = 0; materialIdx < recipe.materials.Count; materialIdx++)
        {
            weaponMaterialsText.text += recipe.materials[materialIdx].item.itemname+" �~"+ recipe.materials[materialIdx].num+"\n";
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
