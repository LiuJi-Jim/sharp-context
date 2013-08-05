using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jim.OCR {
    /// <summary>
    /// 灰度化算法接口
    /// </summary>
    public interface IGreyscale {
        /// <summary>
        /// 输入24位RGB字节数组，输出256级灰度字节数组
        /// </summary>
        byte[] Greyscale(byte[] rgb);
    }
}
