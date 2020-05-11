using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class AsyncLoadingScene : MonoBehaviour {

    // 加载的进度条
    private Slider loadingProgressSlider;
    // 加载的百分比显示
    private Text loadingprogressText;

    private float targetProgress;

    private float currentProgress;
    private GameObject main;
    private AsyncOperation operation;

    void Start()
    {

        loadingProgressSlider = GameObject.Find("Canvas/LoadingProgressSlider").GetComponent<Slider>();
        loadingprogressText = GameObject.Find("Canvas/LoadingprogressText").GetComponent<Text>();
        main = GameObject.FindWithTag("GameEntry");
        main.GetComponent<GameMain>().WorldSystem.ResetBattle();
        main.GetComponent<GameMain>().WorldSystem.ResetBattle();
        switch (GameObject.Find("GameEntry").GetComponent<GameMain>().WorldSystem._model._RoomListModule.roomType)
        {
            case RoomType.Pve:
                StartLoadingScene("MapCreate");
                break;
            case RoomType.Pvp:
                StartLoadingScene("Melee");
                break;
        }
    }

    
    void Update()
    {
        SwitchScene();
    }

    // 设置界面的进度显示
    public void SetProgress(float progress)
    {
        loadingProgressSlider.value = Mathf.Clamp(progress, 0, 1);
        loadingprogressText.text = ((int)(progress * 100)).ToString() + '%';
    }
    
    // 启动加载场景的协程
    public void StartLoadingScene(string name)
    {
        Debug.Log("开始异步加载场景");
        StartCoroutine(LoadingSceneByNameAsync(name));
    }
    
    // 加载场景的协程
    public IEnumerator LoadingSceneByNameAsync(string name)
    {
        Debug.Log("is in Coroutine: " + 1);
        currentProgress = targetProgress = 0;
        //协程加载
        operation = SceneManager.LoadSceneAsync("Scenes/" + name);
        Debug.Log("is in Coroutine: " + 2 + " progress: " + operation.progress + ", allowSceneActivation: " + operation.allowSceneActivation);
        //让场景不自动跳转
        operation.allowSceneActivation = false;
        Debug.Log("is in Coroutine: " + 3 + " progress: " + operation.progress + ", allowSceneActivation: " + operation.allowSceneActivation);
        // 更新进度条
        while (currentProgress < 0.9f)
        {
            Debug.Log("is in Coroutine: " + 4 + " progress: " + operation.progress + ", allowSceneActivation: " + operation.allowSceneActivation);
            targetProgress = operation.progress;
            while (currentProgress < targetProgress)
            {
                currentProgress += 0.05f;
                SetProgress(currentProgress);
                Debug.Log("is in Coroutine: " + 5 + " progress: " + operation.progress + ", allowSceneActivation: " + operation.allowSceneActivation);
                yield return null;  //停一帧
                Debug.Log("is in Coroutine: " + 6 + " progress: " + operation.progress + ", allowSceneActivation: " + operation.allowSceneActivation);
            }
        }
        Debug.Log("is in Coroutine: " + 7 + " progress: " + operation.progress + ", allowSceneActivation: " + operation.allowSceneActivation);
        // 把剩下的也过度一下
        targetProgress = 0.99f;
        while (currentProgress < targetProgress)
        {
            currentProgress += 0.01f;
            SetProgress(currentProgress);
            Debug.Log("is in Coroutine: " + 8 + " progress: " + operation.progress + ", allowSceneActivation: " + operation.allowSceneActivation);
            yield return null;  //停一帧
            Debug.Log("is in Coroutine: " + 9 + " progress: " + operation.progress + ", allowSceneActivation: " + operation.allowSceneActivation);
        }
        // 加载完成给服务器发送完成资源加载的信号
        main.GetComponent<GameMain>().socket.sock_c2s.StartSync();
        Debug.Log("is in Coroutine: " + 10 + " progress: " + operation.progress + ", allowSceneActivation: " + operation.allowSceneActivation);
    }

    public void SwitchScene()
    {
        if (main.GetComponent<GameMain>().WorldSystem._model._RoomModule.IsLoadingCompleted())
        {
            Debug.Log("异步加载场景完成");
            SetProgress(1f);
            main.GetComponent<GameMain>().WorldSystem._map.PlayAudioByScene("MapCreate");
            operation.allowSceneActivation = true;
        }
        
    }
}