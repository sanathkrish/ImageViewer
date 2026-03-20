using ImageViewer.Service.IoCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Service.BackgroundWorkers
{
    public class ImageComparisionPipe: PipeCommunication
    {
        public async Task Initilize()
        {
            await base.Initilize("image_compare",System.IO.Pipes.PipeDirection.InOut);
        }

        public async Task CompareImage(string leftImagePath, string rightImagePath) 
        {
            Dictionary<string,string> payLoad = new Dictionary<string,string>();
            payLoad.Add("left_image",leftImagePath);
            payLoad.Add("right_image", rightImagePath);
            await base.SendMessage(payLoad);
        }

        public async Task<Dictionary<string,string>> GetResultImage()
        {
            return await base.ReadMessage<Dictionary<string, string>>();
        }
    }
}
