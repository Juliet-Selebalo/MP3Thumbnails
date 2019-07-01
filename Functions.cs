using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage.Blob;
using NAudio.Wave;

namespace MP3Thumbnails_WebJob
{
    public class Functions
    {

      public static void GenerateThumbnail(
        [QueueTrigger("mp3SampleMaker")] String blobInfo,
        [Blob("audio/mp3Files/{queueTrigger}")] CloudBlockBlob inputBlob,
        [Blob("audio/samples/{queueTrigger}")] CloudBlockBlob outputBlob, TextWriter logger)
        {

            //set duration of sample to 20 seconds
            int duration = 20;

            //use log.WriteLine() rather than Console.WriteLine() for trace output
            logger.WriteLine("CreateMP3Sample() started...");
            logger.WriteLine("Input blob is: " + blobInfo);

            // Open streams to blobs for reading and writing as appropriate.
            // Pass references to application specific methods
            using (Stream input = inputBlob.OpenRead())
            using (Stream output = outputBlob.OpenWrite())
            {
                CreateMP3Sample(input, output, duration);
                outputBlob.Properties.ContentType = "audio/mpeg3.";
                outputBlob.Metadata["Title"] = inputBlob.Metadata["Title"];
            }
            logger.WriteLine("CreateMP3Sample() completed...");
        }

        // Create thumbnail - the detail is unimportant but notice formal parameter types.

        private static void CreateMP3Sample(Stream input, Stream output, int duration)
          {
            using (var reader = new Mp3FileReader(input, wave => new NLayer.NAudioSupport.Mp3FrameDecompressor(wave)))
            {
                Mp3Frame frame;
                frame = reader.ReadNextFrame();
                int frameTimeLength = (int)(frame.SampleCount / (double)frame.SampleRate * 1000.0);
                int framesRequired = (int)(duration / (double)frameTimeLength * 1000.0);

                int frameNumber = 0;
                while ((frame = reader.ReadNextFrame()) != null)
                {
                    frameNumber++;

                    if (frameNumber <= framesRequired)
                    {
                        output.Write(frame.RawData, 0, frame.RawData.Length);
                    }
                    else break;
                }
            }
        }
    }
}
