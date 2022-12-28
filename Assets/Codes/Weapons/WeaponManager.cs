using Codes.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Gun Main_Weapon;
    public Gun Secaondary_Weapon;
    

    private Gun carriedWeapon;

    private void Start()
    {
        carriedWeapon = Main_Weapon;
    }

    private void Update()
    {
        carriedWeapon.updateWeaponState();
        SwapWeapon();
    }

    public Gun getCarriedWeapon()
    {
        return carriedWeapon;
    }
    private void SwapWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            carriedWeapon.gameObject.SetActive(false);
            carriedWeapon.GunIcon.SetActive(false);
            carriedWeapon = Main_Weapon;
            carriedWeapon.gameObject.SetActive(true);
            carriedWeapon.GunIcon.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DeActivateCarriedWeapon();
            carriedWeapon = Secaondary_Weapon;
            ActivateCarriedWeapon();
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            DeActivateCarriedWeapon();
            changeWeapon();
            ActivateCarriedWeapon();
        }
    }

    private void changeWeapon()
    {
        if (carriedWeapon == Main_Weapon) carriedWeapon = Secaondary_Weapon;
        else carriedWeapon = Main_Weapon;
    }

    private void DeActivateCarriedWeapon()
    {
        carriedWeapon.gameObject.SetActive(false);
        carriedWeapon.GunIcon.SetActive(false);
    }
    private void ActivateCarriedWeapon()
    {
        carriedWeapon.gameObject.SetActive(true);
        carriedWeapon.GunIcon.SetActive(true);
    }
}
