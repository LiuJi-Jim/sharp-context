using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jim.OCR {
    /// <summary>
    /// 二值化算法接口
    /// </summary>
    public interface IBinarization {
        /// <summary>
        /// 输入256级灰度图，输出二值图像
        /// </summary>
        byte[] Binarization(byte[] grey, int width, int height);
    }
}
