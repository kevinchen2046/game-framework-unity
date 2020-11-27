using System.Collections;
namespace vitamin
{
    public class StringUtil
    {
        /**
		 * 字符串解析为数组(按"&","|",",",":"顺序来分隔)
		 * @param value
		 * @return 
		 */
        public static string[][] parseString(string value)
        {
            string[][] ary = { };
            if (value != null)
            {
                string[] temp1 = value.Split('&');
                int len = temp1.Length;
                for (var i = 0; i < len; i++)
                {
                    string str = temp1[i];
                    if (str.IndexOf("|") >= 0)
                    {
                        string[] temp2 = str.Split('|');
                        int n = MathUtil.RandRange(0, temp2.Length - 1);
                        str = temp2[n];
                    }
                    string[] temp3 = str.Split(',');
                    foreach (var item in temp3)
                    {
                        ary[ary.Length] = item.Split(':');
                    }
                }
            }
            foreach (object[] item2 in ary)
            {
                int len = item2.Length;
                for (var i = 0; i < len; i++)
                {
                    item2[i] = System.Convert.ToInt32(item2[i]);
                }
            }
            return ary;
        }
    }
}