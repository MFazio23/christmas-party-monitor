using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ProjectOxford.Face.Contract;

namespace ChristmasPartyMonitor.Controllers
{
    [Route("1/persongroup")]
    public class PersonGroupController : BaseController
    {
        [HttpGet]
        public async Task<PersonGroup[]> GetListOfPersonGroups()
        {
            return await FaceServiceClient.ListPersonGroupsAsync();
        }

        [HttpGet]
        public async Task<PersonGroup> GetPersonGroup(string id)
        {
            return await FaceServiceClient.GetPersonGroupAsync(id);
        }

        [HttpPut] //The Cognitive Services API is also a PUT, so I'm duplicating this here.
        public async Task CreatePersonGroup(string id, string name, string userData)
        {
            await FaceServiceClient.CreatePersonGroupAsync(id, name, userData);
        }

        [HttpPatch]
        public async Task UpdatePersonGroup(string id, string name, string userData)
        {
            await FaceServiceClient.UpdatePersonGroupAsync(id, name, userData);
        }

        [HttpDelete]
        public async Task DeletePersonGroup(string id)
        {
            await FaceServiceClient.DeletePersonGroupAsync(id);
        }

        [HttpGet]
        [Route("1/persongroup/training")]
        public async Task<TrainingStatus> GetPersonGroupTrainingStatus(string id)
        {
            return await FaceServiceClient.GetPersonGroupTrainingStatusAsync(id);
        }

        [HttpPost]
        [Route("1/persongroup/training")]
        public async Task TrainPersonGroup(string id)
        {
            await FaceServiceClient.TrainPersonGroupAsync(id);
        }
    }
}