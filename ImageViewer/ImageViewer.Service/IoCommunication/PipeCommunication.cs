using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Service.IoCommunication
{
    public abstract class PipeCommunication
    {
        private string _pipeName;
        PipeDirection _direction;
        private NamedPipeServerStream namedPipeClientStream;
        private StreamReader streamReader;
        private StreamWriter streamWriter;
        int _bufferSize = 4096;
        bool _isConnected = false;

        protected async Task Initilize(string pipeName, PipeDirection direction)
        {
            try
            {
                _pipeName = pipeName;
                _direction = direction;
                namedPipeClientStream = new NamedPipeServerStream(_pipeName, _direction);
                
                namedPipeClientStream.WaitForConnection();
                streamReader = new StreamReader(namedPipeClientStream);
                streamWriter = new StreamWriter(namedPipeClientStream) { AutoFlush = true };
                await streamWriter.WriteAsync("ping");
                string ping = await ReadMessage();
                if (string.IsNullOrEmpty(ping))
                {
                _isConnected = false;
                } else if(ping.ToLower() == "pong")
                {

                    _isConnected = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error initializing pipe communication: " + ex.Message);
                _isConnected = false;

            }
        }
        public async Task<string> ReadMessage()
        {
            try
            {
                var buffer = new byte[1024];
                var data = new List<byte>();
                while (true)
                {
                    try
                    {

                        var countOfData = await namedPipeClientStream.ReadAsync(buffer, data.Count, buffer.Length);
                        if (countOfData <= 0)
                        {
                            break;
                        }
                        data.AddRange(buffer.Take(countOfData));
                        if (countOfData < buffer.Length)
                        {
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Exception");
                        break;
                    }
                }

                //string messageObject = await streamReader.ReadToEndAsync();
                //if (messageObject == null)
                //{
                //    return default(T);
                //}
                //var jsonMessage = System.Text.Json.JsonSerializer.Deserialize<T>(messageObject);
                //return jsonMessage;

                return Encoding.UTF8.GetString(data.ToArray());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending message: " + ex.Message);
                return default(string);
            }
        }

        public async Task SendMessage(object message)
        {
            if (!_isConnected)
            {
                Console.WriteLine("Pipe is not connected or not in the correct direction for sending messages.");
                return;
            }
            try
            {
                string jsonMessage = System.Text.Json.JsonSerializer.Serialize(message);
                await streamWriter.WriteLineAsync(jsonMessage);
                await streamWriter.FlushAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending message: " + ex.Message);
            }
        }

        public async Task<T> ReadMessage<T>() 
        {
            if (!_isConnected || _direction != PipeDirection.In)
            {
                Console.WriteLine("Pipe is not connected or not in the correct direction for sending messages.");
                return default(T);
            }
            try
            {
                string messageObject = await streamReader.ReadToEndAsync();
                if (messageObject == null)
                {
                    return default(T);
                }
                var jsonMessage = System.Text.Json.JsonSerializer.Deserialize<T>(messageObject);
                return jsonMessage;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending message: " + ex.Message);
                return default(T);
            }
        }

    }
}
