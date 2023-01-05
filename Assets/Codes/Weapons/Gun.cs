using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using QFramework;
using static OOADFPS;

namespace Codes.Weapon
{
    public abstract class Gun : OOADFPSController, Weapon
    {
        public Transform MuzzlePoint;
        public Transform CasingPoint;

        public ParticleSystem MuzzleParticle;
        public GameObject Hit_Particial;
        public GameObject Bullet_Hole;

        public int Mag_Capacity;
        public int Total_Bullet;
        public int Fire_Range;
        public float Fire_Rate;
        public PlayerMovement PM;
        public Camera Gun_Camera;
        public MouseLookAt mouseLook;
        public AnimationCurve Spread_Curve;//子弹散射曲线
        public float Spread;// 用于散射效果的减弱
        public GameObject GunIcon;
        public GameObject CrossUI;

        public AudioClip reloadAmmoLeftClip; // 换子弹音效1
        public AudioClip reloadOutOfAmmoLeftClip; // 换子弹音效1
        public AudioClip fireClip;
        public AudioClip aimClip;
        public AudioClip startClip;

        protected AudioSource audioSource;
        protected float currentSpreadTime; // 当前后坐力曲线曲线时间
        protected float SpreadAngel; //当前散射大小
        protected float Last_Fire_Time;
        protected int Current_Bullet_In_Mag;
        protected int Current_Max_Bullet;
        protected bool Aiming;
        protected float OriginCameraField;
        
        

        protected Animator GunAnimator;
        protected AnimatorStateInfo info;


        protected virtual void Start()
        {
            SpreadAngel = 0f;
            Current_Bullet_In_Mag = Mag_Capacity;
            Current_Max_Bullet = Total_Bullet;
            GunAnimator = GetComponent<Animator>();
            info = GunAnimator.GetCurrentAnimatorStateInfo(0);
            audioSource = GetComponent<AudioSource>();
            OriginCameraField = Gun_Camera.fieldOfView;
            UpdateUI();
            playStartSound();
        }

        public void playStartSound()
        {
            audioSource.clip = startClip;
            audioSource.Play();
        }

        protected bool isAllowedShooting()
        {
            return Time.time - Last_Fire_Time > 1 / Fire_Rate && !PM.IsRuning && !isReloading();
        }

        protected bool isAllowedAiming()
        {
            return !PM.IsRuning && !isReloading() && !isChecking();
        }

        protected void hitOnObjects(RaycastHit hit)
        {
            GameObject hitParticleEffect = Instantiate(Hit_Particial, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)); //创造火光特效
            GameObject bulletHoleEffect = Instantiate(Bullet_Hole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

            Destroy(bulletHoleEffect, 3f);
            Destroy(hitParticleEffect, 1f);
        }

        protected void doShoot()
        {
            Vector3 spread = Random.insideUnitCircle * SpreadAngel * Gun_Camera.fieldOfView * 0.005f;
            if (isAiming()) spread = spread * 0.1f;
            RaycastHit hit;
            Vector3 shootDirection = MuzzlePoint.forward + spread;
            //UnityEngine.Debug.Log(spread.x + " " + spread.y);
            if (Physics.Raycast(MuzzlePoint.position, shootDirection, out hit, Fire_Range))
            {
                hitOnObjects(hit);
            }
        }

        protected void Fire()
        {
            if (!isAllowedShooting() || Current_Bullet_In_Mag <= 0)
            {
                MuzzleParticle.Stop();
                currentSpreadTime = 0;
                return;
            }
            Current_Bullet_In_Mag -= 1;
            PlayFireAnimation();
            Last_Fire_Time = Time.time;
            MuzzleParticle.Play();
            doShoot();
            mouseLook.FiringForTest();
            updateRecoilVal();
            PlayFireSound();
        }
        protected void PlayFireSound()
        {
            audioSource.clip = fireClip; 
            audioSource.Play();
        }
        protected void PlayFireAnimation()
        {
            if (Aiming)
            {
                GunAnimator.Play("Aim Fire", 0, 0);
            }
            else
            {
                GunAnimator.Play("Fire", 0, 0);
            }
        }

        protected void Reload()
        {
            if (Current_Max_Bullet <= 0) return;
            DoReloadAnimation();
            Current_Bullet_In_Mag = Mag_Capacity;
            Current_Max_Bullet -= Mag_Capacity;
        }

        protected void DoReloadAnimation()
        {
            if (Current_Bullet_In_Mag > 0)
            {
                GunAnimator.Play("Reload Ammo Left", 0, 0);
                PlayReloadSound(false);
            }
            if (Current_Bullet_In_Mag == 0)
            {
                GunAnimator.Play("Reload Out Of Ammo", 0, 0);
                PlayReloadSound(true);
            }
        }

        protected void PlayReloadSound(bool outOfAmmo)
        {
            if (outOfAmmo)
            {
                audioSource.clip = reloadOutOfAmmoLeftClip;
                audioSource.Play();
            }
            else
            {
                audioSource.clip = reloadAmmoLeftClip;
                audioSource.Play();
            }
        }

        protected void UpdateAimState()
        {
            Gun_Camera.fieldOfView = Aiming ? 20 : OriginCameraField;
            GunAnimator.SetBool("Aim", Aiming);
        }

        protected void playAimSound()
        {
            audioSource.clip = aimClip;
            audioSource.Play();
        }

        protected void updateRecoilVal()
        {
            currentSpreadTime += Time.deltaTime;
            float recoilFraction = currentSpreadTime * Spread * 0.1f;
            SpreadAngel = Spread_Curve.Evaluate(recoilFraction);
        }
        protected void CheckWeapon()
        {
            GunAnimator.Play("Inspect", 0, 0);
        }

        public bool isChecking()
        {
            if (info.IsName("Inspect")) return true;
            else return false;
        }

        public bool isAiming()
        {
            return Aiming;
        }

        public bool isReloading()
        {
            //两种换子弹动画
            if (info.IsName("Reload Out Of Ammo") || info.IsName("Reload Ammo Left"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void updateWeaponState()
        {
            if (this.GetModel<IPauseModel>().IsPause.Value == true)
            {
                return;
            }
            Aiming = false;
            info = GunAnimator.GetCurrentAnimatorStateInfo(0);
            if (Input.GetKeyDown(KeyCode.Mouse1) && isAllowedAiming())
            {
                playAimSound();
            }
            if (Input.GetKey(KeyCode.Mouse1) && isAllowedAiming())
            {
                Aiming = true;
            }
            UpdateAimState();
            attack();
            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                CheckWeapon();
            }
            UpdateUI();
            if (isAiming())
            {
                CrossUI.SetActive(false);
            }
            else
            {
                CrossUI.SetActive(true);    
            }
        }
        public Animator getAnimator()
        {
            return GunAnimator;
        }
        public abstract void attack();
        protected abstract void UpdateUI();
    }
}

