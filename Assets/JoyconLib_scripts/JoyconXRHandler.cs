using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class JoyconXRHandler : MonoBehaviour
{
    private List<Joycon> joycons;
    private JoyconXR leftJoyconXR;
    private JoyconXR rightJoyconXR;
    public XRDirectInteractor leftInteractor;
    public XRDirectInteractor rightInteractor;

    void Start()
    {
        // 获取 Joycon 实例
        joycons = JoyconManager.Instance.j;

        // 确保至少有两个 Joycon 连接
        if (joycons.Count < 2)
        {
            Debug.LogError("需要至少两个 Joycon 才能使用 JoyconXR");
            return;
        }

        // 初始化 JoyconXR 实例
        leftJoyconXR = new JoyconXR(joycons[0], leftInteractor);
        rightJoyconXR = new JoyconXR(joycons[1], rightInteractor);

        // 调用 Start 方法
        leftJoyconXR.Start();
        rightJoyconXR.Start();
    }

    void Update()
    {
        // 调用 Update 方法
        leftJoyconXR.Update();
        rightJoyconXR.Update();
    }

    void OnDestroy()
    {
        // 调用 Stop 方法
        if (leftJoyconXR != null)
        {
            leftJoyconXR.Stop();
        }
        if (rightJoyconXR != null)
        {
            rightJoyconXR.Stop();
        }
    }
}