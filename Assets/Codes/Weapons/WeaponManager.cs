using Codes.Weapon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using static OOADFPS;

public class WeaponManager : OOADFPSController
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
        //Debug.Log(this.GetModel<IPauseModel>().IsPause.Value);
        //if (this.GetModel<IPauseModel>().IsPause.Value == true)
        //{
        //    return;
        //}
        carriedWeapon.updateWeaponState();
        SwapWeapon();
    }

    public Gun getCarriedWeapon()
    {
        return carriedWeapon;
    }
    private void SwapWeapon()
    {
        if (carriedWeapon.isReloading())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DeActivateCarriedWeapon();
            carriedWeapon = Main_Weapon;
            ActivateCarriedWeapon();
            carriedWeapon.playStartSound();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DeActivateCarriedWeapon();
            carriedWeapon = Secaondary_Weapon;
            ActivateCarriedWeapon();
            carriedWeapon.playStartSound();
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            DeActivateCarriedWeapon();
            changeWeapon();
            ActivateCarriedWeapon();
            carriedWeapon.playStartSound();
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
