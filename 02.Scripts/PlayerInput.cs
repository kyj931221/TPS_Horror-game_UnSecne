using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** Player Input 스크립트 에서는 플레이어의 입력을 감지하고 다른 컴포넌트 (PlayerMovement)에 전달합니다.
 *  각 이동, 회전, 발사, 장전, 점프 등 기능 부분은 [프로퍼티]를 사용하여 변수값을 읽거나 쓰는 과정을 
 *  유연하게 처리하도록 작성하였습니다. **/

/** The Player Input script detects the player's input and passes it to other components.
 * For functional parts such as movement, rotation, firing, loading, jumping, etc., use [Property] to read or write variable values
 * It's written to be flexible. **/

public class PlayerInput : MonoBehaviour
{
    /** 각 기능에 대한 할당된 입력 값의 이름 입니다.
     *  [Edit - Project Setting - Input Manager] 에서 확인할 수 있습니다. **/

    /** Name of the assigned input value for each function.
     * [Edit - Project Setting - Input Manager] **/

    public string moveAxisName = "Vertical";
    public string rotateAxisName = "Horizontal";
    public string fireButtonName = "Fire1";
    public string reloadButtonName = "Reload";
    public string jumpButtonName = "Jump";

    public float move { get; private set; }
    public float rotate { get; private set; }
    public bool fire { get; private set; }
    public bool reload { get; private set; }
    public bool jump { get; private set; }
    public bool movechk {  get; private set; }
    public bool reychk { get; private set; }

    //private bool isFlashlightOn = false; // 손전등 상태

    //void Start()
    //{
       // fire = false;
    //}

    // Update is called once per frame
    private void Update()
    {
        if(GameManager.Instance != null && GameManager.Instance.isGameover)
        {
            move = 0;
            rotate = 0;
            fire = false;
            reload = false;
            return;
        }

        move = Input.GetAxis(moveAxisName);
        rotate = Input.GetAxis(rotateAxisName);
        fire = Input.GetButtonDown(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
        jump = Input.GetButtonDown(jumpButtonName);

        //if(Input.GetButtonDown(fireButtonName))
        //{
        //    isFlashlightOn = !isFlashlightOn;
        //    fire = isFlashlightOn;
        //}
    }
}
