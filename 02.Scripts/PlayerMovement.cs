using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Player Movement 에서는 플레이어의 입력 값에 맞춰서 플레이어 캐릭터를 이동하고
 *  적절한 애니메이션을 동작시킵니다. 
 *  이동, 회전, 점프에 대한 설정 값을 변경할 수 있습니다.
 *  Player Input 스크립트와 함께 입력과 입력에 의한 액터를 분리하였습니다. **/

/** Player Movement moves the player character according to the player's input value
* Operate the appropriate animation.
* You can change the settings for movement, rotation, and jumping.
* You separated the input from the input with the Player Input script. **/

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; //앞 뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도
    public float jumpForce = 20f; //점프 높이
    public float jumpRay = 0.5f;
    private int jumpCount = 0;
    private int maxCount = 1;

    public LayerMask Ground;

    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트

    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디
    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터

    private bool isGrounded;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        Jump();
        MoveCheck();
        playerAnimator.SetBool("ReyCheck",ReyCheck());
        playerAnimator.SetBool("Jump", isGrounded);
    }

    private void FixedUpdate()
    {
        Rotate(); // 회전 실행
        Move(); // 움직임 실행

        playerAnimator.SetFloat("Move", playerInput.move);
    }

    private void Move()
    {
        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
    }

    private void Rotate()
    {
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        playerRigidbody.rotation = playerRigidbody.rotation * Quaternion.Euler(0, turn, 0f);
    }

    private void Jump()
    {
        CheckGroundStatus();

        //Debug.Log(isGrounded + " " + playerInput.jump);

        if (playerInput.jump)
        {
            if(jumpCount < maxCount)
            {
                playerRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                jumpCount++;
            }
        }
    }

    private bool CheckGroundStatus()
    {
        // 플레이어의 위치에서 아래로 Ray를 쏴서 땅에 닿았는지 체크 합니다.
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.1f, Ground);
        if (isGrounded)
        {
            jumpCount = 0;
            return (isGrounded);
        }
        return false;
    }

    /** 애니메이션에서 원활한 표현을 위하여 플레이어의 이동을 체크 합니다.
     *  플레이어의 점프 이후 착지를 하면서 생기는 딜레이를 줄이기 위하여
     *  기본 Idle 자세를 추가하여 어떠한 경우에도 기본 자세(Movement)로 넘어가기 위하여
     *  항상 True 가 되도록 합니다. **/

    /** Check the player's movement for smooth expression in the animation.
     * To reduce the delay caused by landing after the player's jump
     * In order to move to the basic movement in any case by adding the basic Idle posture
     * Always be True. **/

    private bool MoveCheck()
    {
        if(playerInput.move == 0)
        {
            return true;
        }
        return true;
    }

    /** 애니메이션에서 지면에 가까이 있을 때(계단이나 굴곡진 지형) 점프 애니메이션이
     *  자주 플레이 되는 문제를 방지하기 위해 레이 캐스트를 측정하여 짧은 거리에서는
     *  점프 애니메이션이 나타나지 않도록 합니다. **/

    /** When you're close to the ground (step or curved terrain) in an animation, you can see the jump animation
     * To avoid frequent play problems, measure the ray cast for a short distance
     * Do not allow jump animation to appear. **/

    private bool ReyCheck()
    {
        //Debug.Log("Rey Check Stert");
        if (Physics.Raycast(transform.position, Vector3.down, jumpRay, Ground))
        {
            // 플레이어에서 지면까지의 레이 측정값이 True 이면 Movement 애니메이션 그대로 진행
            //Debug.Log("Hit");
            return true ;
        }
        // 플레이어에서 지면까지의 레이 측정값이 False 이면 Jump 애니메이션 으로 넘어간다.
        //Debug.Log("Not Hit");
        return false; // 점프
    }
}
