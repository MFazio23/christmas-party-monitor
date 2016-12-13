using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using ChristmasPartyMonitor.Extensions;
using ChristmasPartyMonitor.Models;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Face.Contract;

namespace ChristmasPartyMonitor.Controllers
{
    [RoutePrefix("1/face")]
    public class FaceController : BaseController
    {
        
        [Route("")]
        [HttpPost]
        public async Task<Face[]> DetectFaces(string url = null)
        {
            if (!string.IsNullOrEmpty(url))
            {
                return await this.FaceService.DetectFacesAsync(url);
            }

            var imageStream = await Request.Content.ReadAsStreamAsync();
            if (imageStream != null)
            {
                return await this.FaceService.DetectFacesAsync(imageStream: imageStream);
            }

            return null;
        }
        
        [Route("identity")]
        [HttpPost]
        public async Task<Person[]> IdentifyFaces(string personGroupId, string url = null, float confidenceThreshold = -1, int maxNumOfCandidatesReturned = 1)
        {
            Face[] faces = {};

            if (!string.IsNullOrEmpty(url))
            {
                faces = await this.FaceService.DetectFacesAsync(url);
            }
            else
            {
                var imageStream = await Request.Content.ReadAsStreamAsync();
                if (imageStream != null)
                {
                    faces = await this.FaceService.DetectFacesAsync(imageStream: imageStream);
                }
            }

            return await this.FaceService.IdentifyFacesAsync(personGroupId, faces, confidenceThreshold, maxNumOfCandidatesReturned);
        }
        
        [Route("identity/emotion")]
        [HttpPost]
        public async Task<PersonEmotion[]> IdentifyFacesWithEmotions(string personGroupId, string url = null, float confidenceThreshold = -1, int maxNumOfCandidatesReturned = 1)
        {
            Face[] faces = {};
            var emotions = new Dictionary<Guid, Emotion>();

            if (!string.IsNullOrEmpty(url))
            {
                faces = await this.FaceService.DetectFacesAsync(url);
                emotions = await this.EmotionService.GetEmotionsForFaces(faces, url: url);
            }
            else
            {
                var imageStream = await Request.Content.ReadAsStreamAsync();
                if (imageStream != null)
                {
                    faces = await this.FaceService.DetectFacesAsync(imageStream: imageStream.CreateStreamCopy());
                    emotions = await this.EmotionService.GetEmotionsForFaces(faces, imageStream.CreateStreamCopy());
                }
            }

            return await this.FaceService.IdentifyFacesWithEmotionsAsync(personGroupId, faces, emotions, confidenceThreshold, maxNumOfCandidatesReturned);
        }
        
        [Route("emotion")]
        [HttpPost]
        public async Task<Emotion[]> IdentifyEmotions(string url = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                var imageStream = await Request.Content.ReadAsStreamAsync();
                if (imageStream != null)
                    return await this.EmotionService.GetEmotions(imageStream);
            }

            return await this.EmotionService.GetEmotions(url: url);
        }

    }
}