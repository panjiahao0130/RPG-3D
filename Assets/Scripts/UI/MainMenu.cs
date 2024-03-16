using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    private Button newGameBtn;
    private Button continueBtn;
    private Button quitBtn;
    private PlayableDirector director;

    private void Awake()
    {
        newGameBtn = transform.GetChild(0).GetComponent<Button>();
        continueBtn = transform.GetChild(1).GetComponent<Button>();
        quitBtn = transform.GetChild(2).GetComponent<Button>();
        newGameBtn.onClick.AddListener(PlayTimeLine);
        continueBtn.onClick.AddListener(ContinueGame);
        quitBtn.onClick.AddListener(QuitGame);
        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;
    }

    private void PlayTimeLine()
    {
        director.Play();
    }

    private void NewGame(PlayableDirector playableDirector)
    {
        //将角色数据先删除
        PlayerPrefs.DeleteAll();
        //场景切换
        SceneController.Instance.TransitionToFirstLevel();
    }

    private void ContinueGame()
    {
        //场景切换 读取数据
        SceneController.Instance.TransitionToLoadGame();
    }

    private void QuitGame()
    {
        // 区分平台执行退出操作
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 在编辑器中停止播放
#elif UNITY_STANDALONE
            Application.Quit(); // 在桌面平台上退出游戏
#elif UNITY_WEBGL
            Application.OpenURL("about:blank"); // 在 WebGL 上打开一个空白页面来关闭游戏
#endif
    }
    
}
