using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Jim.OCR {
    public class ImageProcessor {
        /// <summary>
        /// 将位图转为24位RGB字节数组（会去掉Sride）
        /// </summary>
        public static byte[] Bmp2Bytes(Bitmap bmp) {
            int width = bmp.Width, height = bmp.Height;
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            int stride = data.Stride;
            IntPtr ptr = data.Scan0;
            byte[] raw = new byte[stride * height];
            Marshal.Copy(ptr, raw, 0, raw.Length);

            byte[] bytes = new byte[width * height * 3];
            int src = 0, dst = 0;
            for (int i = 0; i < height; ++i) {
                Array.Copy(raw, src, bytes, dst, width * 3);
                src += stride;
                dst += width * 3;
            }
            bmp.UnlockBits(data);
            return bytes;
        }

        /// <summary>
        /// 将24位RGB字节数组转为彩色位图
        /// </summary>
        public static Bitmap RgbBytes2Bmp(byte[] rgb, int width, int height) {
            return Bytes2BmpWithFormat(rgb, width, height, PixelFormat.Format24bppRgb, 3);
        }

        /// <summary>
        /// 256级灰度字节数组转为灰度位图（同样适用于二值图像）
        /// </summary>
        public static Bitmap Greyscale2Bmp(byte[] bytes, int width, int height) {
            Bitmap bmp = Bytes2BmpWithFormat(bytes, width, height, PixelFormat.Format8bppIndexed, 1);
            ColorPalette cp = bmp.Palette;
            for (int i = 0; i < 256; ++i) {
                cp.Entries[i] = Color.FromArgb(255, i, i, i);
            }
            cp.Entries[127] = Color.FromArgb(255, 0, 0);
            bmp.Palette = cp;
            return bmp;
        }

        private static Bitmap Bytes2BmpWithFormat(byte[] bytes, int width, int height, PixelFormat format, int pixelLen) {
            Bitmap bmp = new Bitmap(width, height, format);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, format);
            int stride = data.Stride;
            byte[] raw = new byte[stride * height];
            //for (int i = 0; i < raw.Length; ++i) raw[i] = 0;
            int dst = 0, src = 0;
            for (int i = 0; i < height; ++i) {
                Array.Copy(bytes, src, raw, dst, width * pixelLen);
                src += width * pixelLen;
                dst += stride;
            }
            IntPtr ptr = data.Scan0;
            Marshal.Copy(raw, 0, ptr, bytes.Length);
            bmp.UnlockBits(data);
            return bmp;
        }
    }
}