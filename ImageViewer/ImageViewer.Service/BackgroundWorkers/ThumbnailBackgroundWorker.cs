using ImageViewer.Model;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;

namespace ImageViewer.Service.BackgroundWorkers
{

    public class ThumbnailBackgroundWorker:BackgroundWorkers.CustomBackgroundWorker<ThumbnailInfo>
    {
        public Queue<ThumbnailInfo> _data = new Queue<ThumbnailInfo>();
        public Action<ThumbnailInfo> ThumbnailReady;
        public ThumbnailBackgroundWorker() : base()
        {
            
        }

        public override void DoWorkAction(ThumbnailInfo input)
        {
            this.IsRunning = true;
            while (true && IsRunning)
            {
                try
                {
                    var dataToProcess = IAction.Invoke();
                    if (dataToProcess != null)
                    {
                        this.CreateThumbnailBytesAsync(dataToProcess.Path).ContinueWith(t =>
                        {
                            try
                            {
                                if (t.Result != null)
                                {
                                    dataToProcess.Data = t.Result;
                                    NAction?.Invoke(dataToProcess);
                                }
                                else
                                {
                                    // Handle case where thumbnail couldn't be generated
                                    Console.WriteLine($"Failed to generate thumbnail for: {dataToProcess.Path}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error processing thumbnail for {dataToProcess.Path}: {ex.Message}");
                            }
                        });
                    }
                    else
                    {
                        Task.Delay(1000).Wait(); // Wait before checking the queue again
                    }
                }
                catch (InvalidOperationException ex)
                {
                    // Queue is empty, wait before trying again
                    Task.Delay(1000).Wait();
                    this.IsRunning = false;
                }
            }
            this.IsRunning = false;
        }

        public void AddToQueue(ThumbnailInfo info)
        {
                _data.Enqueue(info);
        }

        public override void NotifyAction(ThumbnailInfo output)
        {
            Console.WriteLine($"Thumbnail generated for: {output.Path}");
            output.ImageCallback?.Invoke(output.Data);
        
        }
        public override ThumbnailInfo InterceptorAction()
        {
            if(_data.Count > 0)
            {
                return _data.Dequeue();
            }
            return null;
        }

        public async Task<BitmapImage> GetEmbeddedThumbnailAsync(string filePath, uint size = 200)
        {
            BitmapImage bitmap = new BitmapImage();
            StorageFile file =
         await StorageFile.GetFileFromPathAsync(filePath);

            using var thumbnail =
                await file.GetThumbnailAsync(
                    ThumbnailMode.PicturesView,
                    size,
                    ThumbnailOptions.UseCurrentScale);

            if (thumbnail == null)
                return null;

            await bitmap.SetSourceAsync(thumbnail);

            return bitmap;
        }

        public async Task<byte[]> CreateThumbnailBytesAsync( string filePath,uint width = 200,uint height=200)
        {
            // Open file via System.IO
            using FileStream fs =
                new FileStream(filePath, FileMode.Open, FileAccess.Read);

            using IRandomAccessStream input =
                fs.AsRandomAccessStream();

            // Decode
            BitmapDecoder decoder =
                await BitmapDecoder.CreateAsync(input);

            // Scale at decode time (fastest)
            var transform = new BitmapTransform
            {
                ScaledWidth = width,
                ScaledHeight = height,
                InterpolationMode = BitmapInterpolationMode.Fant
            };

            var pixelData = await decoder.GetPixelDataAsync(
                BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Ignore,
                transform,
                ExifOrientationMode.RespectExifOrientation,
                ColorManagementMode.DoNotColorManage);

            // Encode thumbnail → JPEG
            using var output =
                new InMemoryRandomAccessStream();

            BitmapEncoder encoder =
                await BitmapEncoder.CreateAsync(
                    BitmapEncoder.JpegEncoderId,
                    output);

            encoder.SetPixelData(
                BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Ignore,
                width,
                height,
                decoder.DpiX,
                decoder.DpiY,
                pixelData.DetachPixelData());

            await encoder.FlushAsync();

            // Convert stream → byte[]
            byte[] bytes = new byte[output.Size];

            await output.ReadAsync(
                bytes.AsBuffer(),
                (uint)output.Size,
                InputStreamOptions.None);

            return bytes;
        }
    }
}
