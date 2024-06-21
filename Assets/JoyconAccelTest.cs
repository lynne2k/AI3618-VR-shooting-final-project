using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyconAccelTest : MonoBehaviour
{
    private List<Joycon> joycons;
    public int jc_ind = 0; // Joycon index

    void Start()
    {
        // 获取Joycon数组
        joycons = JoyconManager.Instance.j;
        if (joycons.Count < jc_ind + 1)
        {
            Debug.LogError("Joycon not found!");
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (joycons.Count > 0)
        {
            Joycon j = joycons[jc_ind];
            Vector3 accel = j.GetAccel(); // 获取加速度数据

            // 计算加速度大小
            float accelMagnitude = accel.magnitude;

            // 输出加速度数据和大小
            Debug.Log(string.Format("Accel x: {0:N2}, y: {1:N2}, z: {2:N2}, Magnitude: {3:N2}", accel.x, accel.y, accel.z, accelMagnitude));
        }
    }
}
