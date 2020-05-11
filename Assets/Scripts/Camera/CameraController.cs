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

    private Vector3 CurrentVelocity = Vector3.zero;
    private float SmoothTime = 0.01f;
    public void SetTarget(GameObject tar)
    {
        playerObject = tar;
        Vector3 playerPos = playerObject.transform.position;

        //Vector3 playerPos =PackConverter.FixVector2ToVector2( playerObject.GetComponent<PlayerModel_Component>().GetPlayerPosition());
        Vector3 cameraPos = transform.position;                                               //记录摄像机的位置
        Vector3 startTargPos = playerPos;
        lastTargetPosition = startTargPos;
        currTargetPosition = startTargPos;
    }
    public void ViewUpdate()
    {
        trackPlayer();
        //将摄像头移动到目标位置
        currLerpDistance += cameraTrackingSpeed;
        //transform.position = Vector3.Lerp(lastTargetPosition, currTargetPosition, currLerpDistance);
        transform.position = Vector3.SmoothDamp(lastTargetPosition, currTargetPosition, ref CurrentVelocity, Global.FrameRate/1000f);

    }

    //void LateUpdate()
    //{
    //    //依据当前精灵的动画状态，实时更新
    //    trackPlayer();
    //    //将摄像头移动到目标位置
    //    currLerpDistance += cameraTrackingSpeed;
    //    //transform.position = Vector3.Lerp(lastTargetPosition, currTargetPosition, currLerpDistance);
    //    transform.position = Vector3.SmoothDamp(lastTargetPosition, currTargetPosition, ref CurrentVelocity, 0.005f);


    //}
    void trackPlayer()
    {
        Vector3 currCamPos = transform.position;
        if (playerObject != null)
        {
            //Vector3 currPlayerPos = playerObject.transform.position;
            Vector3 currPlayerPos = PackConverter.FixVector2ToVector2(playerObject.GetComponent<PlayerModel_Component>().GetPlayerPosition());
            lastTargetPosition = currCamPos;
            currTargetPosition = currPlayerPos;
            currTargetPosition.z = currCamPos.z;
        }
        else
        {
            Debug.LogError("???");
        }
     }
}
