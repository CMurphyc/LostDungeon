using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject playerObject = null;                                
    private float cameraTrackingSpeed = 0.003f;                               //表示摄像机的追踪速度
    private Vector3 lastTargetPosition = Vector3.zero;             //上一目标位置
    private Vector3 currTargetPosition = Vector3.zero;             //下一目标位置
    private float currLerpDistance = 0.0f;

    public void SetTarget(GameObject tar)
    {
        playerObject = tar;
        Vector3 playerPos = playerObject.transform.position;
        Vector3 cameraPos = transform.position;                                               //记录摄像机的位置
        Vector3 startTargPos = playerPos;
        lastTargetPosition = startTargPos;
        currTargetPosition = startTargPos;
    }

    void LateUpdate()
    {
        //依据当前精灵的动画状态，实时更新
        trackPlayer();
        //将摄像头移动到目标位置
        currLerpDistance += cameraTrackingSpeed;
        transform.position = Vector3.Lerp(lastTargetPosition, currTargetPosition, currLerpDistance);
    }
    void trackPlayer()
    {
        Vector3 currCamPos = transform.position;       
        Vector3 currPlayerPos = playerObject.transform.position;
        lastTargetPosition = currCamPos;
        currTargetPosition = currPlayerPos;
        currTargetPosition.z = currCamPos.z;                          //保证摄像头z轴方向上的值不变
     }
}
