using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : Singleton<InventoryController>
{
    [SerializeField]
    private List<Item> currentAmmoList;
    [SerializeField]
    private int itemCountInInventory = 5;
    [SerializeField]
    private bool hasEmptySlot;
    [SerializeField]
    public Item nullItem;

    public int activeSlotIndex = 0;

    public bool HasEmptySlot
    {
        get { return hasEmptySlot; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchItem(int step)
    {
        currentAmmoList[activeSlotIndex].isActive = false;
        WeaponSystemUI.Instance.displayItems[activeSlotIndex].selectedIcon.SetActive(false);
        activeSlotIndex += step;
        if (activeSlotIndex < 0) activeSlotIndex = currentAmmoList.Count - 1;
        if (activeSlotIndex == currentAmmoList.Count) activeSlotIndex = 0;
        currentAmmoList[activeSlotIndex].isActive = true;
        WeaponSystemUI.Instance.displayItems[activeSlotIndex].selectedIcon.SetActive(true);
        //WeaponSystemUI.Instance.artwork.sprite = GetCurrentItem().ammoStats.artwork;
    }

    public Item AddNewAmmoToInventory(AmmoStats ammoStats, int count, Suckable ammoObject)
    {
        int firstSlot = GetSlotByName(ammoStats.name);
        int emptySlot = GetSlotByName("Null");
        Item item = GetCurrentItem();

        MyDebug.Instance.Log(firstSlot + " " + emptySlot);

        if (firstSlot >= 0)
        {
            //Check stack item here with existed item
            int leftOverAmmo = currentAmmoList[firstSlot].AddAmmo(count, ammoObject);
            Debug.Log("Add MORE" + count + ammoStats.name + " and left over" + leftOverAmmo);
            item = currentAmmoList[firstSlot];
        }
        else if (emptySlot != -1)
        {
            //Add new iteam which not existed in inventory
            //currentAmmoList[emptySlot] = new Item(ammoStats, count, ofActiveAmmo && !IsExistedAmmoForWeaponSlot(ammoStats.weaponSlot));
            if (emptySlot == activeSlotIndex)
            {
                currentAmmoList[emptySlot] = new Item(ammoStats, count, true, ammoObject, emptySlot);
                //WeaponSystemUI.Instance.artwork.sprite = GetCurrentItem().ammoStats.artwork;
            }
            else currentAmmoList[emptySlot] = new Item(ammoStats, count, false, ammoObject, emptySlot);
            item = currentAmmoList[emptySlot];
            Debug.Log(item);
            //if (ofActiveAmmo) activeSlotIndex = emptySlot;
            Debug.Log("Add NEW" + count + ammoStats.name);
        }
        else
        {
            //Debug.Log("No more slot");
            hasEmptySlot = true;
            //New item but don't enough slot
        }
        return item;
    }

    public void ResetCurrentSlot()
    {
        GetCurrentItem().ResetItem(nullItem);
    }

    public int GetSlotByName(string name)
    {
        for (int i = 0; i < itemCountInInventory; i++)
        {
            if (currentAmmoList[i].ammoStats.name == name) return i;
        }
        return -1;
    }

    public Item GetItemByIndex(int index)
    {
        return currentAmmoList[index];
    }

    public Item GetItem(AmmoStats ammoStats)
    {
        return currentAmmoList[GetSlotByName(ammoStats.name)];
    }

    public Item GetCurrentItem()
    {
        return currentAmmoList[activeSlotIndex];
    }

    public bool IsExistedAmmoForWeaponSlot(ActiveWeapon.WeaponSlot weaponSlot)
    {
        for (int i = 0; i < itemCountInInventory; i++)
        {
            if (currentAmmoList[i].ammoStats.weaponSlot == weaponSlot) return true;
        }
        return false;
    }
}
