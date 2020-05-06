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
        StartLoadingScene("MapCreate");
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
        currentProgress = targetProgress = 0;
        //协程加载
        operation = SceneManager.LoadSceneAsync("Scenes/" + name);
        
        //让场景不自动跳转
        operation.allowSceneActivation = false;
        // 更新进度条
        while (currentProgress < 0.9f)
        {
            targetProgress = operation.progress;
            while (currentProgress < targetProgress)
            {
                currentProgress += 0.05f;
                SetProgress(currentProgress);
                yield return null;  //停一帧
            }
        }
        // 把剩下的也过度一下
        targetProgress = 0.99f;
        while (currentProgress < targetProgress)
        {
            currentProgress += 0.01f;
            SetProgress(currentProgress);
            yield return null;  //停一帧
        }
        // 加载完成给服务器发送完成资源加载的信号
        main.GetComponent<GameMain>().socket.sock_c2s.StartSync();
    }

    public void SwitchScene()
    {
        if (main.GetComponent<GameMain>().WorldSystem._model._RoomModule.IsLoadingCompleted())
        {
            Debug.Log("异步加载场景完成");
            SetProgress(1f);
            operation.allowSceneActivation = true;
        }
        
    }
}