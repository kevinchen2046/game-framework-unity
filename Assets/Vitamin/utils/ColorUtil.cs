namespace vitamin { 
    public class ColorUtil
    {
        public static string toHex(int value)
        {
            return "#" + MathUtil.toHex(value);
        }

        /**
		 * ��ɫ���� 
		 * @return 
		 * 
		 */
        public static int toBright(int color, int brightOff = 0x66)
        {
            int[] list = ColorUtil.Extract(color);
            return ColorUtil.Merge(list[0] + brightOff, list[1] + brightOff, list[2] + brightOff);
        }

        /**
         * ��ɫ�䰵
         * @return 
         * 
         */
        public static int toDark(int color, int darkOff = 0x66)
        {
            int[] list = ColorUtil.Extract(color);
            return ColorUtil.Merge(list[0] - darkOff, list[1] - darkOff, list[2] - darkOff);
        }

        /**
         * ����ɫֵ����ȡ��ԭɫ 
         * @param color
         * @return 
         * 
         */
        public static int[] Extract(int color)
        {
            //			var R:number=color>>16;
            //			var G:number=(color>>8)-(R<<8);
            //			var B:number=color-(R<<16)-(G<<8);
            int R = color >> 16;
            int G = (color >> 8) & 0x00FF;
            int B = color << 24 >> 24;
            return new int[] { R, G, B };
        }

        /**
         * ����ԭɫ�ϲ� 
         * @param r
         * @param g
         * @param b
         * @return 
         */
        public static int Merge(int r, int g, int b)
        {
            //return (r<<16)+(g<<8)+b;
            if (r > 0xFF) r = 0xFF;
            if (g > 0xFF) g = 0xFF;
            if (b > 0xFF) b = 0xFF;
            if (r < 0) r = 0;
            if (g < 0) g = 0;
            if (b < 0) b = 0;
            return r << 16 | (g << 8) | b;
        }

        /**
		 * ��32λ��ɫֵ����ȡ��ԭɫ 
		 * @param color
		 * @return 
		 * 
		 */
        public static int[] extract32(int color)
        {
            int A = color >> 24 & 0xFF;
            int R = color >> 16 & 0xFF << 8 >> 8;
            int G = (color >> 8) & 0x00FF;
            int B = color << 24 >> 24;
            return new int[] { A, R, G, B };
        }

        /**
		 * ������ͨ����Ϣ����ԭɫ�ϲ� 
		 * @param r
		 * @param g
		 * @param b
		 * @return 
		 */
        public static int merge32(int a, int r, int g, int b)
        {
            if (a > 0xFF) a = 0xFF;
            if (r > 0xFF) r = 0xFF;
            if (g > 0xFF) g = 0xFF;
            if (b > 0xFF) b = 0xFF;
            if (a < 0) a = 0;
            if (r < 0) r = 0;
            if (g < 0) g = 0;
            if (b < 0) b = 0;
            return (a << 24) | (r << 16) | (g << 8) | b;
        }

        /**
		 * ��ɫ���
		 * @param color
		 * @param arg ������ɫ
		 * @return 
		 */
        public static int add(int color, params int[] colors)
        {

            int[] list = ColorUtil.Extract(color);

            int R = list[0];
            int G = list[1];
            int B = list[2];
            foreach (var temColor in colors)
            {
                list = ColorUtil.Extract(temColor);
                R += list[0];
                G += list[1];
                B += list[2];
            }
            return ColorUtil.Merge(R, G, B);
        }

        /**
         * ��ɫ���
         * @param color
         * @param other
         * @return 
         */
        public static int sub(int color, params int[] colors)
        {
            int[] list = ColorUtil.Extract(color); ;

            int R = list[0];
            int G = list[1];
            int B = list[2];
            foreach (var temColor in colors)
            {
                list = ColorUtil.Extract(temColor);
                R -= list[0];
                G -= list[1];
                B -= list[2];
            }
            return ColorUtil.Merge(R, G, B);
        }

        /**
         * ��32λ��ɫ����ȡ͸��ͨ����ֵ 
         * @param color
         * @return 
         */
        public static int extractAlphaFrom32(int color)
        {
            return color >> 24;
        }
    }
}