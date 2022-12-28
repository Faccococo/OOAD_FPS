using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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
    private AudioSource audioSource;

    public float crouch_height;
    public float Speed;
    public float Jump_height;
    public float Gravity;
    public Animator character_animator;

    public AudioClip walkingSound;
    public AudioClip runingSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        characterController= GetComponent<CharacterController>();
        characterTransform = transform;
        originHeight = characterController.height;
    }

    private void Update()
    {
        isJumping = !characterController.isGrounded;
        isRuning = false;
        isMoving = false;
        isCrouching = false;
        isWalking= false;

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
            }
            if (isCrouching) StartCoroutine(change_height(crouch_height));
            else StartCoroutine(change_height(originHeight));
        }
        time_floating += Time.deltaTime;
        move_direction.y -= 0.5f * Gravity * (Time.deltaTime + 2 * time_floating) * Time.deltaTime;
        
        characterController.Move(Speed * Time.deltaTime * move_direction);

        character_animator.SetBool("Walk", isMoving && !isRuning);
        character_animator.SetBool("Run", isRuning);

        if (isMoving)
        {
            audioSource.clip = isWalking? walkingSound:runingSound; 
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
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
}
