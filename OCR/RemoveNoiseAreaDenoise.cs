using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jim.OCR {
    public class RemoveNoiseAreaDenoise : IDenoise {
        public int Threshold = 10;

        public byte[] Denoise(byte[] data, int width, int height) {
            byte[,] arr = Utils.ByteArr1DTo2D(data, width, height);
            bool[,] vi = (bool[,])Array.CreateInstance(typeof(bool), height, width);

            for (int i = 0; i < height; ++i) {
                for (int j = 0; j < width; ++j) {
                    vi[i, j] = false;
                }
            }
            for (int i = 0; i < height; ++i) {
                for (int j = 0; j < width; ++j) {
                    if (vi[i, j]) continue;
                    if (arr[i, j] == 255) continue;
                    int sum = 0;
                    dfsSearch(arr, vi, i, j, width, height, ref sum);
                    if (sum < Threshold) {
                        dfsDel(arr, vi, i, j, width, height);
                    }
                }
            }

            byte[] result = Utils.ByteArr2DTo1D(arr);
            return result;
        }

        private static readonly int[] dy = { -1, -1, -1, 0, 1, 1, 1, 0 };
        private static readonly int[] dx = { -1, 0, 1, 1, 1, 0, -1, -1 };

        private void dfsDel(byte[,] arr, bool[,] vi, int y, int x, int width, int height) {
            arr[y, x] = 255;
            for (int i = 0; i < 8; ++i) {
                int ny = y + dy[i], nx = x + dx[i];
                if (ny < 0 || ny >= height) continue;
                if (nx < 0 || nx >= width) continue;

                if (arr[ny, nx] == 0) dfsDel(arr, vi, ny, nx, width, height);
            }
        }

        private void dfsSearch(byte[,] arr, bool[,] vi, int y, int x, int width, int height, ref int sum) {
            if (vi[y, x]) return;
            vi[y, x] = true;
            ++sum;
            for (int i = 0; i < 8; ++i) {
                int ny = y + dy[i], nx = x + dx[i];
                if (ny < 0 || ny >= height) continue;
                if (nx < 0 || nx >= width) continue;

                if (arr[ny, nx] == 0) dfsSearch(arr, vi, ny, nx, width, height, ref sum);
            }
        }
    }
}
