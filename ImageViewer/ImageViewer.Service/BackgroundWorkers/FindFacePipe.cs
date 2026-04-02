using ImageViewer.Service.IoCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Service.BackgroundWorkers
{
    public class FindFacePipe : PipeCommunication
    {
        public async Task Initilize()
        {
            await base.Initilize("image_faces", System.IO.Pipes.PipeDirection.InOut);
        }

        public async Task GetFaces(string imagePath)
        {
            Dictionary<string, string> payLoad = new Dictionary<string, string>();
            payLoad.Add("image_path", imagePath);
            await base.SendMessage(payLoad);
        }

        public async Task<Dictionary<string, string>> GetResultImage()
        {
            return await base.ReadMessage<Dictionary<string, string>>();
        }
    }
}
