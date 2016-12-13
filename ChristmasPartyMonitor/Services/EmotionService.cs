using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChristmasPartyMonitor.Extensions;
using ChristmasPartyMonitor.Utilities;
using Microsoft.ProjectOxford.Emotion;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Face.Contract;

namespace ChristmasPartyMonitor.Services
{
    public class EmotionService
    {
        private readonly EmotionServiceClient _emotionServiceClient;

        public EmotionService(EmotionServiceClient emotionServiceClient = null)
        {
            _emotionServiceClient = emotionServiceClient ?? new EmotionServiceClient(Config.EmotionServiceClientKey);
        }

        public async Task<Emotion[]> GetEmotions(Stream imageStream = null, string url = null)
        {
            var emotions = imageStream != null
                ? await this._emotionServiceClient.RecognizeAsync(imageStream)
                : await this._emotionServiceClient.RecognizeAsync(url);

            return emotions;
        }

        public async Task<Dictionary<Guid, Emotion>> GetEmotionsForFaces(Face[] faces, Stream imageStream = null, string url = null)
        {
            var emotionRecon = imageStream != null
                ? await this._emotionServiceClient.RecognizeAsync(imageStream)
                : await this._emotionServiceClient.RecognizeAsync(url);

            var emotions = new Dictionary<Guid, Emotion>();

            foreach (var emotion in emotionRecon)
            {
                var faceId =
                    faces.FirstOrDefault(f => f.FaceRectangle.FaceRectanglesAreEqual(emotion.FaceRectangle))?.FaceId;

                if (faceId.HasValue)
                    emotions.Add(faceId.Value, emotion);
            }

            return emotions;
        }
    }
}