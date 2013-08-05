using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jim.OCR.Mathematics {
    public static class MathUtils {
        public static void Swap<T>(ref T a, ref T b) {
            T tmp = a;
            a = b;
            b = tmp;
        }
        public static void Swap<T>(T[] arr, int a, int b) {
            T tmp = arr[a];
            arr[a] = arr[b];
            arr[b] = tmp;

        }
        /// <summary>
        /// 找出比n大的最小2的整数次幂
        /// </summary>
        public static int ExpandTo2yn(int n) {
            int m = 1;
            while (m < n) m <<= 1;
            return m;
        }
        /// <summary>
        /// 
        /// 把数组扩展到2的整数次幂
        /// </summary>
        /// <param name="offset">下标起始值</param>
        public static T[] Expand<T>(T[] arr, int offset) {
            int len = ExpandTo2yn(arr.Length - offset);
            T[] buf = new T[len + offset];
            Array.Copy(arr, offset, buf, offset, arr.Length - offset);
            return buf;
        }
        /// <summary>
        /// 把数组扩展到2的整数次幂
        /// </summary>
        public static T[] Expand<T>(T[] arr) {
            return Expand(arr, 0);
        }
    }
}
