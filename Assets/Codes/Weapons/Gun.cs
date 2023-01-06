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
        public float Damage = 10f;
        public PlayerMovement PM;

        public MouseLookAt mouseLook;
        public AnimationCurve Spread_Curve;//�ӵ�ɢ������
        public float Spread;// ����ɢ��Ч���ļ���
        public GameObject GunIcon;
        public GameObject CrossUI;
        public CameraChange cameraController;

        public AudioClip reloadAmmoLeftClip; // ���ӵ���Ч1
        public AudioClip reloadOutOfAmmoLeftClip; // ���ӵ���Ч1
        public AudioClip fireClip;
        public AudioClip aimClip;
        public AudioClip startClip;

        protected Transform ShootPoint;
        protected AudioSource audioSource;
        protected float currentSpreadTime; // ��ǰ��������������ʱ��
        protected float SpreadAngel; //��ǰɢ���С
        protected float Last_Fire_Time;
        protected int Current_Bullet_In_Mag;
        protected int Current_Max_Bullet;
        protected bool Aiming;
        protected float OriginCameraField;
        protected Camera Gun_Camera;

        protected Animator GunAnimator;
        protected AnimatorStateInfo info;

        protected virtual void Start()
        {
            Gun_Camera = cameraController.getMainCamera();
            ShootPoint = Gun_Camera.transform;
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

        public void updateWeaponState()
        {
            if (this.GetModel<IPauseModel>().IsPause.Value == true)
            {
                return;
            }
            Gun_Camera = cameraController.getMainCamera();
            ShootPoint = Gun_Camera.transform;
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
            if (isAiming() && Gun_Camera == cameraController.first_person_camera)
            {
                CrossUI.SetActive(false);
            }
            else
            {
                CrossUI.SetActive(true);
            }

        }

        public void playStartSound()
        {
            audioSource = GetComponent<AudioSource>();
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
            GameObject hitParticleEffect = Instantiate(Hit_Particial, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)); //��������Ч
            GameObject bulletHoleEffect = Instantiate(Bullet_Hole, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));

            Destroy(bulletHoleEffect, 3f);
            Destroy(hitParticleEffect, 1f);
        }

        protected void hitOnEnemy(RaycastHit hit)
        {
            hit.collider.GetComponent<bool_control>().hit(Damage);
        }

        protected void doShoot()
        {
            Vector3 spread = Random.insideUnitCircle * SpreadAngel * Gun_Camera.fieldOfView * 0.005f;
            if (isAiming()) spread = spread * 0.1f;
            RaycastHit hit;
            Vector3 shootDirection = ShootPoint.forward + spread;
            //UnityEngine.Debug.Log(spread.x + " " + spread.y);
            Vector3 shootPosition = ShootPoint.position + 1.2f * ShootPoint.forward.normalized;
            //shootPosition.y -= 0.3f;
            if (Physics.Raycast(shootPosition, shootDirection, out hit, Fire_Range))
            {
                if (hit.collider.GetComponent<bool_control>() != null)
                {
                    hitOnEnemy(hit);
                }
                else
                {
                    hitOnObjects(hit);
                }
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
            Gun_Camera.fieldOfView = Aiming ? 30 : OriginCameraField;
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
            //���ֻ��ӵ�����
            if (info.IsName("Reload Out Of Ammo") || info.IsName("Reload Ammo Left"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void setBulletInMag(int bullet_num)
        {
            Current_Bullet_In_Mag = bullet_num;
        }

        public int getMagCapacity()
        {
            return Mag_Capacity;
        }
        
        public Animator getAnimator()
        {
            return GunAnimator;
        }
        public abstract void attack();
        protected abstract void UpdateUI();
    }
}

