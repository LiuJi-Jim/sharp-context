using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jim.OCR {
    /// <summary>
    /// 降噪算法接口
    /// </summary>
    public interface IDenoise {
        /// <summary>
        /// 输入二值图像，输出降噪后的图像
        /// </summary>
        byte[] Denoise(byte[] data, int width, int height);
    }
}
