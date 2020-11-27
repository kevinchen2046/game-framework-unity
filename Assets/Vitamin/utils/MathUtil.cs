
using System.Collections;
using UnityEngine;
namespace vitamin
{

    public class MathUtil
    {
        public static string toBin(int d)
        {
            return System.Convert.ToString(d, 2);
        }
        public static int fromBin(string b)
        {
            return System.Convert.ToInt32(b, 2);
        }
        public static int fromHex(string h)
        {
            return System.Convert.ToInt32(h, 16);
        }
        public static string fromHex(int h)
        {
            return System.Convert.ToString(h, 10);
        }
        public static string binToHex(string h)
        {
            return string.Format("{0:X}", System.Convert.ToInt32(h, 2));
        }
        public static string toHex(int d)
        {
            return string.Format("{0:x}", d);
        }

        private static Hashtable sinCache = new Hashtable();
        private static Hashtable cosCache = new Hashtable();

        /**
		 * 对sin函数的二次封装，降低sin函数的cpu消耗 
		 * @param angle
		 * @return 
		 * 
		 */
        public static float sin(float value)
        {
            int angle = MathUtil.Floor(value);
            angle = MathUtil.toLAngle(angle);
            if (!MathUtil.sinCache.ContainsValue(angle))
            {
                MathUtil.sinCache[angle] = Mathf.Sin(MathUtil.angleToRadian(angle));
            }
            return (float)MathUtil.sinCache[angle];
        }

        /**
		 * 对cos函数的二次封装，降低cos函数的cpu消耗 
		 * @param angle
		 * @return 
		 */
        public static float cos(float value)
        {
            int angle = MathUtil.Floor(value);
            angle = MathUtil.toLAngle(angle);
            if (!MathUtil.cosCache.Contains(angle))
            {
                MathUtil.cosCache[angle] = Mathf.Cos(MathUtil.angleToRadian(angle));
            }
            return (float)MathUtil.cosCache[angle];
        }

        /**
		 * 角度转弧度
		 * @param angle
		 * @return 
		 * 
		 */
        public static float angleToRadian(float angle)
        {
            return (angle * Mathf.PI) / 180;
        }

        /**
		 * 弧度转角度 
		 * @param radian
		 * @return 
		 */
        public static float radianToAngle(float radian)
        {
            return Mathf.Round((radian * 180) / Mathf.PI);
        }

        /**
		 * 范围随机取整
		 * @param min
		 * @param max
		 * @return 
		 */
        public static int RandRange(float min, float max)
        {
            return MathUtil.Floor(MathUtil.Random() * (max - min) + min);
        }
        public static int RandRange(int min, int max)
        {
            return MathUtil.Floor(MathUtil.Random() * (max - min) + min);
        }
        /**
		 * 范围随机 取浮点数
		 * @param min
		 * @param max
		 * @return 
		 */
        public static float RandRangeFloat(float min, float max)
        {
            return MathUtil.Random() * (max - min) + min;
        }

        public static float Random()
        {
            return UnityEngine.Random.Range(0.0f, 1.0f);
            //System.Random random = new System.Random(System.Guid.NewGuid().GetHashCode());
            //return (float)random.NextDouble();
        }
        /**
		 * 舍去浮点值的位数，最后一位四舍五入
		 * @param value
		 * @param dot
		 * @return 
		 */
        public static float roundFixed(float value, float dot)
        {
            dot = MathUtil.rangeLimit(dot, 0, 16);
            if (dot == 0) return value;
            float range = Mathf.Pow(10, dot);
            return Mathf.Round((value * range)) / range;
        }

        /**
		 * 舍去浮点值的位数，并保留浮点精度
		 * @param value
		 * @param dot
		 * @return 
		 */
        public static float floorFixed(float value, float dot)
        {
            dot = MathUtil.rangeLimit(dot, 0, 16);
            if (dot == 0) return value;
            float range = Mathf.Pow(10, dot);
            return (Mathf.Round((value * range) - (MathUtil.Floor(value) * range))) / range;
        }

        /**
		 * 取弧度值 
		 * @param x
		 * @param y
		 * @return 
		 * 
		 */
        public static float getRadian(float x, float y)
        {
            return Mathf.Atan2(y, x);
        }

        /**
		 * 取角度值 
		 * @param x
		 * @param y
		 * @return 
		 */
        public static float getAngle(float x, float y)
        {
            return MathUtil.radianToAngle(MathUtil.getRadian(x, y));
        }

        /**
		 * 取角度值 (精确)
		 * @param x
		 * @param y
		 * @return 
		 */
        public static float getAngleExact(float x, float y)
        {
            return (MathUtil.getRadian(x, y) * 180) / Mathf.PI;
        }

        /**
		 * 取角度值 ，并 区间化0-360 
		 * @param x
		 * @param y
		 * @return 
		 * 
		 */
        public static float getLAngle(float x, float y)
        {
            return MathUtil.toLAngle(MathUtil.radianToAngle(MathUtil.getRadian(x, y)));
        }

        /**
		 * 
		 * 获取以 上 为开始方向的方向 
		 * @param x
		 * @param y
		 * @return 
		 */
        public static int getUAngle(float x, float y)
        {
            float angle = MathUtil.radianToAngle(MathUtil.getRadian(x, y));
            if (angle < 0)
            {
                angle += 360;
            }
            angle += 90;
            angle %= 360;
            return MathUtil.Floor(angle);
        }

        /**
		 * 对角度区间化，返回的角度值 一定会是 0-360之间的数字 
		 * @param angle
		 * @return 
		 * 
		 */
        public static float toLAngle(float angle)
        {
            if ((angle > -1) && (angle < 360))
            {
                return angle;
            }
            angle = angle % 360;
            if (angle < 0)
            {
                angle = angle + 360;
            }
            return angle;
        }

        public static int toLAngle(int angle)
        {
            return MathUtil.Floor(MathUtil.toLAngle((float)angle));
        }
        /**
		 * 限定值在区间内
		 * @param value
		 * @param minValue
		 * @param maxValue
		 * @return 
		 * 
		 */
        public static float rangeLimit(float value, float minValue, float maxValue)
        {
            if (value < minValue) value = minValue;
            if (value > maxValue) value = maxValue;
            return value;
        }

        /**
		 * 取得两点间的弧度 
		 * @param x1
		 * @param y1
		 * @param x2
		 * @param y2
		 * @return 
		 * 
		 */
        public static float getTwoPointRadian(float x1, float y1, float x2, float y2)
        {
            float offx = (x2 - x1);
            float offy = (y2 - y1);
            return MathUtil.getRadian(offx, offy);
        }

        /**
		 * 取两点间的角度 
		 * @param x1
		 * @param y1
		 * @param x2
		 * @param y2
		 * @return 
		 * 
		 */
        public static float getTwoPointAngle(float x1, float y1, float x2, float y2)
        {
            return MathUtil.radianToAngle(MathUtil.getTwoPointRadian(x1, y1, x2, y2));
        }

        /**
		 * 获取两点的距离 
		 * @param x1
		 * @param y1
		 * @param x2
		 * @param y2
		 * @return 
		 * 
		 */
        public static float getDistance(float x1, float y1, float x2, float y2)
        {
            float offx = (x2 - x1);
            float offy = (y2 - y1);
            return Mathf.Sqrt((offx * offx) + (offy * offy));
        }

        /**
		 * 从一个区间内取出随机且不重复的指定数量的值集合
		 * @param min 区间最小值
		 * @param max 区间最大值
		 * @param total 需要取出的值集合长度
		 */
        public static int[] Randoms(int min, int max, int total)
        {
            if (max == -1) max = 0;

            ArrayList list = new ArrayList();
            int index = 0;
            for (int i = min; i < max; i++)
            {
                list[index++] = i;
            }
            int[] results = { };
            while (total > 0)
            {
                int p = MathUtil.Floor(MathUtil.Random() * list.Count);
                int value = (int)list[p];
                list.RemoveAt(p);
                results[results.Length] = value;
                total--;
            }
            return results;
        }
        /**
		 * 根据两个点，长度，确定 横穿两点射线上任意点的位置 （算角度方式）
		 * @param x1
		 * @param y1
		 * @param x2
		 * @param y2
		 * @param length
		 * @return 
		 */
        public static Vector2 getRightAngleSide(float x1, float y1, float x2, float y2, float length)
        {
            float angle = MathUtil.getTwoPointAngle(x1, y1, x2, y2);
            float vx = length * MathUtil.cos(angle);
            float vy = length * MathUtil.sin(angle);
            return new Vector2(vx, vy);
        }

        /**
		 * 根据两个点，长度，确定 横穿两点射线上任意点的位置 （直接算距离）
		 * @param x1
		 * @param y1
		 * @param x2
		 * @param y2
		 * @param length
		 * @return 
		 * 
		 */
        public static Vector2 getLinePoint(float x1, float y1, float x2, float y2, float length)
        {
            float distance = MathUtil.getDistance(x1, y1, x2, y2);
            if (distance == 0 || distance == length) return new Vector2(x2, y2);
            float off = length / (distance - length);
            Vector2 result = new Vector2();
            result.x = (x1 + (x2 * off)) / (1 + off);
            result.y = (y1 + (y2 * off)) / (1 + off);
            return result;
        }

        /**
		 * 根据角度，长度，获取目标点  //oldName:getLinePoint2
		 * @param x
		 * @param y
		 * @param length
		 * @param angle
		 * @return 
		 */
        public static Vector2 getLinePointByAngle(float x, float y, float length, float angle, Vector2 cache )
        {
            float vx = x + (length * MathUtil.cos(angle));
            float vy = y + (length * MathUtil.sin(angle));
            cache.x = vx;
            cache.y = vy;
            return cache;
        }
        public static Vector2 getLinePointByAngle(float x, float y, float length, float angle)
        {
            return MathUtil.getLinePointByAngle(x, y, length, angle, new Vector2());
        }
        /**
		 * 根据角度，长度，获取目标点  (精确)
		 * @param x
		 * @param y
		 * @param length
		 * @param angle
		 * @return
		 */
        public static Vector2 getLinePointByAngleExact(float x, float y, float length, float angle)
        {
            float vx = x + (length * Mathf.Cos(MathUtil.angleToRadian(angle)));
            float vy = y + (length * Mathf.Sin(MathUtil.angleToRadian(angle)));
            return new Vector2(vx, vy);
        }

        /**
		 * 取整 相当于 Math.round(v);
		 * @param v
		 * @return 
		 */
        public static int Floor(float v)
        {
            return (int)v;
            // return (int)((int)v >> 0);
        }

        /**
		 * 取绝对值 相当于Math.abs(v);
		 * @param v
		 * @return 
		 * 
		 */
        public static int abs(int v)
        {
            return (v ^ (v >> 31)) - (v >> 31);
        }

        /**
		 * 检查number是否为偶数 相当于 i%2==0
		 * @param v
		 * @return 
		 * 
		 */
        public static bool isEven(int v)
        {
            return (v & 1) == 0;
        }

        /**
		 * 取反 相当于 v=-v;
		 * @param v
		 * @return 
		 * 
		 */
        public static float flip(int v)
        {
            return ~v + 1;
        }


        /**
		 * 获取符号返回1，0，－1
		 */
        public static int sign(float val)
        {
            int result = 1;
            if (val < 0) result = -1;
            return result;
        }
    }
}