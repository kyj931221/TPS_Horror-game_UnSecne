using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** TargetDamage 는 공격 받는 모든 대상들에게 적용되는 인터페이스 입니다.
 *  상속받는 클래스는 OnDamage 메서드를 구현해야 합니다. **/

/** TargetDamage is an interface that applies to all targets under attack.
  * Inherited classes must implement OnDamage methods. **/

public interface TargetDamage
{
    /** damage : 대미지 크기 
     *  hitPoint : 공격당한 위치
     *  hitNormal : 공격당한 표면의 방향 **/

    void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal);
}
