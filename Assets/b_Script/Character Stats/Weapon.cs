using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject weaponModel;
    public void ShowWeapon()
    {
        if (weaponModel != null)
        {
            weaponModel.SetActive(true);
        }
    }
}
