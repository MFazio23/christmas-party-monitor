using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using ChristmasPartyMonitor.Models;
using Microsoft.ProjectOxford.Face.Contract;

namespace ChristmasPartyMonitor.Controllers
{
    [Route("1/person")]
    public class PersonController : BaseController
    {
        [HttpGet]
        public async Task<Person> GetPersonById(string personGroupId, Guid personId)
        {
            return await this.FaceServiceClient.GetPersonAsync(personGroupId, personId);
        }

        [HttpGet]
        public async Task<Person[]> ListPersonsInPersonGroup(string personGroupId)
        {
            return await this.FaceServiceClient.GetPersonsAsync(personGroupId);
        }

        [HttpPost]
        public async Task<CreatePersonResult> CreatePerson(string personGroupId, string name, string userData = null)
        {
            return await this.FaceServiceClient.CreatePersonAsync(personGroupId, name, userData);
        }

        [HttpPatch]
        public async Task<Person> UpdatePerson(string personGroupId, Guid personId, string name, string userData = null)
        {
            await this.FaceServiceClient.UpdatePersonAsync(personGroupId, personId, name, userData);
            return await this.GetPersonById(personGroupId, personId);
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeletePerson(string personGroupId, Guid personId)
        {
            await this.FaceServiceClient.DeletePersonAsync(personGroupId, personId);
            return Ok();
        }

        [HttpDelete]
        public async Task<IHttpActionResult> DeletePersons(string personGroupId, IList<Guid> personIds)
        {
            foreach (var personId in personIds)
            {
                await this.FaceServiceClient.DeletePersonAsync(personGroupId, personId);
            }
            return Ok();
        }

        [HttpPost]
        public async Task<IList<PersonLoadResult>> AddPersonsAndPictures(string personGroupId, IList<PersonToLoad> personsToLoad)
        {
            var results = new List<PersonLoadResult>();

            foreach (var personToLoad in personsToLoad)
            {
                var personLoadResult = new PersonLoadResult(personToLoad.Name);

                var createPersonResult = await this.FaceServiceClient.CreatePersonAsync(personGroupId, personToLoad.Name, personToLoad.UserData);

                foreach (var url in personToLoad.Urls)
                {
                    var addFaceResult = await this.FaceServiceClient.AddPersonFaceAsync(personGroupId, createPersonResult.PersonId, url);
                    personLoadResult.AddPersistedFaceId(addFaceResult.PersistedFaceId);
                }

                results.Add(personLoadResult);
            }

            return results;
        }
    }
}