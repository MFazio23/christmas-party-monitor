using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ProjectOxford.Face.Contract;

namespace ChristmasPartyMonitor.Controllers
{
    [Route("1/personface")]
    public class PersonFaceController : BaseController
    {
        [HttpGet]
        public async Task<PersonFace> GetPersonFaceById(string personGroupId, Guid personId, Guid persistedFaceId)
        {
            return await this.FaceServiceClient.GetPersonFaceAsync(personGroupId, personId, persistedFaceId);
        }

        [HttpPost]
        public async Task<AddPersistedFaceResult> AddPersonFace(string personGroupId, Guid personId, string url = null, string userData = null)
        {
            if (string.IsNullOrEmpty(url))
            {
                var imageStream = await Request.Content.ReadAsStreamAsync();
                return await this.FaceServiceClient.AddPersonFaceAsync(personGroupId, personId, imageStream, userData);
            }

            return await this.FaceServiceClient.AddPersonFaceAsync(personGroupId, personId, url, userData);
        }

        [HttpPatch]
        public async Task<PersonFace> UpdatePersonFace(string personGroupId, Guid personId, Guid persistedFaceId, string userData = null)
        {
            await this.FaceServiceClient.UpdatePersonFaceAsync(personGroupId, personId, persistedFaceId, userData);
            return await this.GetPersonFaceById(personGroupId, personId, persistedFaceId);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeletePersonFace(string personGroupId, Guid personId, Guid persistedFaceId)
        {
            await this.FaceServiceClient.DeletePersonFaceAsync(personGroupId, personId, persistedFaceId);
            return Ok();
        }
        
        [HttpPost]
        public async Task<IList<AddPersistedFaceResult>> AddPersonFaces(string personGroupId, Guid personId, string[] urls)
        {
            IList<AddPersistedFaceResult> results = new List<AddPersistedFaceResult>(urls.Length);

            foreach (var url in urls)
            {
                results.Add(await this.FaceServiceClient.AddPersonFaceAsync(personGroupId, personId, url));
            }

            return results;
        }
    }
}
