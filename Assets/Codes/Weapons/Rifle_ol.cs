using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace Codes.Weapon
{
    public class Rifle_ol : Gun_ol
    {

        public Text Current_Bullet_In_Mag_UI;
        public Text Current_Max_Bullet_UI;
        public Text Weapon_Name;

        public override void attack()
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Fire();
            }
        }

        protected override void UpdateUI()
        {
            Current_Bullet_In_Mag_UI.text = Current_Bullet_In_Mag.ToString();
            Current_Max_Bullet_UI.text = Current_Max_Bullet.ToString();
            Weapon_Name.text = "Rifle";
        }
    }
}