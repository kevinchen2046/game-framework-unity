using System.Collections;
namespace vitamin
{
    public class UnitUtil
    {

        /**
		 * 直接转换 
		 * @param num 值
		 * @param unit 单位换算倍率
		 * @param unitString 单位名称
		 * @return 
		 */
        public static string convertUnits(float num, float unit, string unitString)
        {
            if (unit == 0) return num + unitString;
            return (num / unit) + unitString;
        }

        /**
		 * 货币格式化 (位数+单位)
		 * @param 数值，显示位数
		 * @return 
		 */
        private static string[] formatUnitArray = { "k", "m", "b" };//千   百万   十亿     以一千倍计数
        public static string currencyFormat(float value, int n = 3)
        {
            string str = MathUtil.Floor(value).ToString();
            int len = str.Length;
            if (len <= 3)
                return str;
            int index = MathUtil.Floor((len - 1) / 3);
            int temp = len - index * 3;
            if (temp == 3)
                return str.Substring(0, temp) + UnitUtil.formatUnitArray[index - 1];
            return str.Substring(0, temp) + "." + str.Substring(temp, n - temp) + UnitUtil.formatUnitArray[index - 1];
        }

        /**
		 * 货币格式化(加入分隔符)
		 * @param value
		 * @return 
		 * 
		 */
        public static string currencyFormat2(float value,string splitchar=",")
        {
            string valueString = value.ToString();
            int index = valueString.Length;
            ArrayList result = new ArrayList();
            while (index >= 3)
            {
                result.Insert(0,valueString.Substring(index - 3, 3));
                index = index - 3;
            };
            if (index > 0)
            {
                result.Insert(0,valueString.Substring(0, index));
            };
            return CollectionUtil.Join(result,splitchar);
        }
    }
}