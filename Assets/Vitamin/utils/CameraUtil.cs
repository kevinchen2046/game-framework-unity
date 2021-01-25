using System;

using UnityEngine;

namespace vitamin
{
    public class CameraUtil
    {
        /// <summary>
        /// 获取摄像机截面宽高
        /// </summary>
        /// <param name="camera"></param>
        /// <returns></returns>
        public static Rect GetRect(Camera camera)
        {
            float distance = 18;
            float halfFOV = (camera.fieldOfView * 0.5f) * Mathf.Deg2Rad;
            float aspect = camera.aspect;

            float height = distance * Mathf.Tan(halfFOV);
            float width = height * aspect;
            return new Rect(0, 0, width, height);
        }
    }
}
