using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Mirror;

public class PlayerMovement_ol : NetworkBehaviour
{
    private CharacterController characterController;
    private Transform characterTransform;
    private Vector3 move_direction;
    private float time_floating;
    private float originHeight;

    private bool isRuning;
    private bool isMoving;
    private bool isCrouching;
    private bool isJumping;
    private bool isWalking;
    private Animator character_animator;
    private AudioSource audioSource;
    private Vector3 Born_Position;

    public float crouch_height;
    public float Speed;
    public float Jump_height;
    public float Gravity;
    public WeaponManager_ol weapon;
    public float resetTime;

    public AudioClip walkingSound;
    public AudioClip runingSound;
    public Text text;
    
    public override void OnStartLocalPlayer()
    {
        Born_Position = transform.position;
        audioSource = GetComponent<AudioSource>();
        characterController = GetComponent<CharacterController>();
        characterTransform = transform;
        originHeight = characterController.height;
        time_floating = 0;
        //character_animator = weapon.getCarriedWeapon().getAnimator();
    }

    private void Update()
    {
        Physics.autoSyncTransforms = false;
        isJumping = !characterController.isGrounded;
        isRuning = false;
        isMoving = false;
        isCrouching = false;
        isWalking= false;
        character_animator = weapon.getCarriedWeapon().getAnimator();

        if (!isJumping)
        {
            isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0);
            isRuning = Input.GetKey(KeyCode.LeftShift) && isMoving;
            isCrouching = Input.GetKey(KeyCode.LeftControl);
            isWalking = !isRuning&& isMoving;

            time_floating = 0;
            float walk_speed = isRuning ? 2f : 1f;
            walk_speed = isCrouching ? 0.5f * walk_speed : walk_speed;
            var movement_horizontal = Input.GetAxis("Horizontal") * walk_speed;
            var movement_vertical = Input.GetAxis("Vertical") * walk_speed;

            move_direction = characterTransform.TransformDirection(new Vector3(movement_horizontal, 0, movement_vertical));
            if (Input.GetButton("Jump") && !isCrouching)
            {
                move_direction.y = Jump_height;
                if (text != null && text.IsActive())
                {
                    SceneManager.LoadScene(0);
                }
            }
            if (isCrouching) StartCoroutine(change_height(crouch_height));
            else StartCoroutine(change_height(originHeight));
        }
        if (time_floating >= resetTime)
        {
            Physics.autoSyncTransforms = true;
            characterTransform.position = Born_Position;
        }
        //Debug.Log(characterTransform.position);
        //Debug.Log(time_floating);
        time_floating += Time.deltaTime;
        move_direction.y -= 0.5f * Gravity * (Time.deltaTime + 2 * time_floating) * Time.deltaTime;
        
        characterController.Move(Speed * Time.deltaTime * move_direction);

        character_animator.SetBool("Walk", isMoving && !isRuning);
        character_animator.SetBool("Run", isRuning);

        playSound();
    }

    public void playSound()
    {
        if (isMoving && !isJumping)
        {
            audioSource.clip = isWalking ? walkingSound : runingSound;
            if (!audioSource.isPlaying)     audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
        }
    }

    public IEnumerator change_height(float target_height) {
        float current_height = 0f;
        characterController.height = Mathf.SmoothDamp(characterController.height, target_height, ref current_height, Time.deltaTime);
        yield return null;
    }
        
    public bool IsJumping { get { return isJumping; } }
    public bool IsRuning { get { return isRuning; } }
    public bool IsMoving { get { return isMoving; } }
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("1");
        if (collision.gameObject.tag == "bounce")
        {
            move_direction.y = Jump_height * 2;
            characterController.Move(Speed * Time.deltaTime * move_direction);
            return;
        }


    }
}
