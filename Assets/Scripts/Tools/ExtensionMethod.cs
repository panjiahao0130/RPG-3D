using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//拓展方法
public static class ExtensionMethod
{
    private const float DotThreshold = 0.5f;//DotThreshold 点阈值
    //由于在Unity中，将Transform类设为sealed是为了确保游戏对象的变换行为是可靠和稳定的，不能直接改transform类
    //this Transform 表示要扩展的是Transform这个类，逗号后面才是参数
    /// <summary>
    /// 判断是否在目标的前面
    /// </summary>
    /// <param name="transform">要拓展的类型</param>
    /// <param name="target">传入target参数</param>
    /// <returns>返回true表示在目标的前面</returns>
    public static bool IsFaceTarget(this Transform transform,Transform target)
    {
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();
        float dot= Vector3.Dot(transform.forward, vectorToTarget);
        return dot >= DotThreshold;
    }
}
