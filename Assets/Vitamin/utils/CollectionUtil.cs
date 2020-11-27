
using System;
using System.Collections;
namespace vitamin { 

    public class ShuffleComparer : IComparer
    {
        int IComparer.Compare(object a, object b)
        {
            return MathUtil.Random() > 0.5 ? 1 : -1;
        }
    }

    public class CollectionUtil
    {
        /**
		 * 从一个集合取随机的元素
		 * @param min 区间最小值
		 * @param max 区间最大值
		 * @param total 需要取出的值集合长度
		 */
        public static object[] GetRandom(ArrayList list, int total)
        {
            ArrayList keys = new ArrayList();
            for (int i = 0; i < list.Count; i++)
            {
                keys.Add(i);
            }
            object[] results = { };
            while (total > 0)
            {
                int index = MathUtil.Floor((MathUtil.Random() * keys.Count));
                int key = (int)keys[index];
                keys.RemoveAt(index);
                results[results.Length] = list[key];
                total--;
            }
            return results;
        }

        public static object GetRandom(ArrayList list)
        {
            int index = MathUtil.RandRange(0, (list.Count - 1));
            return list[index];
        }
        /**
		 * 通过连接符将集合做字符串连接
		 * @param list 集合
		 * @param joinchar 连接符
		 */
        public static string Join(ArrayList list, string joinchar)
        {
            var result = "";
            for (var i = 0; i < list.Count; i++)
            {
                result += (i < list.Count - 1) ? (list[i].ToString() + joinchar) : (list[i].ToString());
            }
            return result;
        }
        public static string Join(object[] list, string joinchar)
        {
            var result = "";
            for (var i = 0; i < list.Length; i++)
            {
                result += (i < list.Length - 1) ? (list[i].ToString() + joinchar) : (list[i].ToString());
            }
            return result;
        }
        public static string Join(int[] list, string joinchar)
        {
            var result = "";
            for (var i = 0; i < list.Length; i++)
            {
                result += (i < list.Length - 1) ? (list[i].ToString() + joinchar) : (list[i].ToString());
            }
            return result;
        }
        public static string Join(string[] list, string joinchar)
        {
            var result = "";
            for (var i = 0; i < list.Length; i++)
            {
                result += (i < list.Length - 1) ? (list[i].ToString() + joinchar) : (list[i].ToString());
            }
            return result;
        }
        public static string Join(float[] list, string joinchar)
        {
            var result = "";
            for (var i = 0; i < list.Length; i++)
            {
                result += (i < list.Length - 1) ? (list[i].ToString() + joinchar) : (list[i].ToString());
            }
            return result;
        }
        public static string Join(byte[] list, string joinchar)
        {
            var result = "";
            for (var i = 0; i < list.Length; i++)
            {
                result += (i < list.Length - 1) ? (list[i].ToString() + joinchar) : (list[i].ToString());
            }
            return result;
        }
        /**
		 * 释放集合 
		 * @param source
		 */
        public static void Clear(ArrayList list)
        {
            list.Clear();
        }

        /**
		 * 添加元素到集合 
		 * @param source
		 * @param _args
		 */
        public static void Add(ArrayList list, params object[] data)
        {
            for (var i = 0; i < data.Length; i++)
            {
                list.Add(data[i]);
            }
        }

        /**
		 * 从集合移除元素 
		 * @param source
		 * @param _args
		 * 
		 */
        public static void Remove(ArrayList list, params object[] data)
        {
            for (var i = 0; i < data.Length; i++)
            {
                list.Remove(data[i]);
            }
        }

        /**
		 * 打乱集合 
		 * @param source
		 * 
		 */
        public static ArrayList Shuffle(ArrayList list)
        {
            list.Sort(new ShuffleComparer());
            return list;
        }
        public static object[] Shuffle(object[] list)
        {
            Array.Sort(list, new ShuffleComparer());
            return list;
        }
        public static int[] Shuffle(int[] list)
        {
            Array.Sort(list, new ShuffleComparer());
            return list;
        }
        public static float[] Shuffle(float[] list)
        {
            Array.Sort(list, new ShuffleComparer());
            return list;
        }
        public static string[] Shuffle(string[] list)
        {
            Array.Sort(list, new ShuffleComparer());
            return list;
        }
    }
}