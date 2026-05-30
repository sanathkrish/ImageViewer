using System;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Microsoft.UI.Xaml.Media.Imaging;
using System.Numerics;

namespace ImageViewer.Service
{
    public class ImageAnalysisService
    {
        // Supported image extensions
        private static readonly string[] _imageExtensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif", ".tiff", ".webp" };

        public bool IsImageFile(string path)
        {
            var ext = System.IO.Path.GetExtension(path)?.ToLowerInvariant();
            return !string.IsNullOrEmpty(ext) && _imageExtensions.Contains(ext);
        }

        public async Task<bool> IsCorruptedAsync(string filePath)
        {
            try
            {
                var file = await StorageFile.GetFileFromPathAsync(filePath);
                using var stream = await file.OpenAsync(FileAccessMode.Read);
                var decoder = await BitmapDecoder.CreateAsync(stream);
                // attempt to get pixel data small region
                var pixels = await decoder.GetPixelDataAsync();
                return false;
            }
            catch
            {
                return true;
            }
        }

        // Average hash (aHash) 8x8
        public async Task<ulong?> ComputePerceptualHashAsync(string filePath)
        {
            try
            {
                var file = await StorageFile.GetFileFromPathAsync(filePath);
                using var stream = await file.OpenAsync(FileAccessMode.Read);
                var decoder = await BitmapDecoder.CreateAsync(stream);
                // downscale to 8x8
                var transform = new BitmapTransform { ScaledWidth = 8, ScaledHeight = 8 };
                var pixelData = await decoder.GetPixelDataAsync(BitmapPixelFormat.Gray8, BitmapAlphaMode.Straight, transform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.DoNotColorManage);
                var bytes = pixelData.DetachPixelData();
                if (bytes == null || bytes.Length == 0) return null;
                // compute average
                double avg = bytes.Average(b => (double)b);
                ulong hash = 0;
                for (int i = 0; i < bytes.Length && i < 64; i++)
                {
                    if (bytes[i] > avg)
                    {
                        hash |= (1UL << i);
                    }
                }
                return hash;
            }
            catch
            {
                return null;
            }
        }

        // Simple blur detection using variance of Laplacian approximation by downscaling and gradient energy
        public async Task<bool> IsBlurredAsync(string filePath, double threshold = 100.0)
        {
            try
            {
                var file = await StorageFile.GetFileFromPathAsync(filePath);
                using var stream = await file.OpenAsync(FileAccessMode.Read);
                var decoder = await BitmapDecoder.CreateAsync(stream);
                var transform = new BitmapTransform { ScaledWidth = 64, ScaledHeight = 64 };
                var pixelData = await decoder.GetPixelDataAsync(BitmapPixelFormat.Gray8, BitmapAlphaMode.Straight, transform, ExifOrientationMode.RespectExifOrientation, ColorManagementMode.DoNotColorManage);
                var bytes = pixelData.DetachPixelData();
                if (bytes == null || bytes.Length == 0) return false;

                int w = (int)decoder.PixelWidth > 64 ? 64 : (int)decoder.PixelWidth;
                int h = (int)decoder.PixelHeight > 64 ? 64 : (int)decoder.PixelHeight;
                if (w == 0 || h == 0) return false;

                double sum = 0;
                int count = 0;
                for (int y = 1; y < h - 1; y++)
                {
                    for (int x = 1; x < w - 1; x++)
                    {
                        int idx = y * w + x;
                        int center = bytes[idx];
                        int lap = -center
                            + bytes[idx - 1] + bytes[idx + 1] + bytes[idx - w] + bytes[idx + w]
                            + bytes[idx - w - 1] + bytes[idx - w + 1] + bytes[idx + w - 1] + bytes[idx + w + 1];
                        sum += lap * lap;
                        count++;
                    }
                }
                if (count == 0) return false;
                double var = sum / count;
                return var < threshold;
            }
            catch
            {
                return false;
            }
        }

        public static int HammingDistance(ulong a, ulong b)
        {
            return BitOperations.PopCount(a ^ b);
        }
    }
}
