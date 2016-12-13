using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChristmasPartyMonitor.Extensions;
using ChristmasPartyMonitor.Models;
using ChristmasPartyMonitor.Utilities;
using Microsoft.ProjectOxford.Emotion.Contract;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

namespace ChristmasPartyMonitor.Services
{
    public class FaceService
    {
        private static readonly IList<FaceAttributeType> DefaultFaceAttributeTypes = new List<FaceAttributeType>
        {
            FaceAttributeType.Age,
            FaceAttributeType.Gender
        };

        private readonly FaceServiceClient _faceServiceClient;
        private readonly SlackService _slackService;

        public FaceService(FaceServiceClient faceServiceClient = null)
        {
            _faceServiceClient = faceServiceClient ?? new FaceServiceClient(Config.FaceServiceClientKey);
            _slackService = new SlackService();
        }

        public async Task<Face[]> DetectFacesAsync(string url = null, Stream imageStream = null)
        {
            if (!string.IsNullOrEmpty(url))
                return await this._faceServiceClient.DetectAsync(url, returnFaceAttributes: DefaultFaceAttributeTypes);

            if (imageStream != null)
                return await this._faceServiceClient.DetectAsync(imageStream, returnFaceAttributes: DefaultFaceAttributeTypes);

            return new Face[] {};
        }

        public async Task<Person[]> IdentifyFacesAsync(string personGroupId, Face[] faces, float confidenceThreshold,
            int maxNumOfCandidatesReturned)
        {
            var persons = new List<Person>(faces.Length);

            var identifyResults = await _faceServiceClient.IdentifyAsync(
                personGroupId,
                faces.Select(f => f.FaceId).ToArray(),
                confidenceThreshold < 0 ? Config.IdentifyConfidenceThreshold : confidenceThreshold,
                maxNumOfCandidatesReturned);

            foreach (var identifyResult in identifyResults)
            {
                persons.Add(await _faceServiceClient.GetPersonAsync(personGroupId, identifyResult.Candidates[0].PersonId));
            }

            if (persons.Any(p => p.Name.Equals("John Ptacek")))
            {
                this._slackService.SendSlackMessage("Ptacek's in the house.  _BAIL OUT!_");
            }

            return persons.ToArray();
        }

        public async Task<PersonEmotion[]> IdentifyFacesWithEmotionsAsync(string personGroupId, Face[] faces,
            Dictionary<Guid, Emotion> emotions, float confidenceThreshold, int maxNumOfCandidatesReturned)
        {
            var persons = await this.GetPeopleFromFaceIds(personGroupId, faces.Select(f => f.FaceId));
            
            if (persons.Values.Any(p => p.Name.Equals("John Ptacek")))
            {
                this._slackService.SendSlackMessage("Ptacek's in the house.  _BAIL OUT!_");
            }

            var peopleEmotions = this.GetEmotionsForPersons(persons, emotions);

            return peopleEmotions.ToArray();
        }

        public async Task<Dictionary<Guid, Person>> GetPeopleFromFaceIds(string peopleGroupId, IEnumerable<Guid> faceIds)
        {
            var faceIdArray = faceIds.ToArray();
            if (faceIdArray.Any())
            {
                var identities = await _faceServiceClient.IdentifyAsync(peopleGroupId, faceIdArray, 0.5f);

                if (identities.Any())
                {
                    var identifyResults = identities.Where(i => i.Candidates.Any());

                    var people = new Dictionary<Guid, Person>();

                    foreach (var id in identifyResults)
                    {
                        var person = await _faceServiceClient.GetPersonAsync(peopleGroupId, id.Candidates[0].PersonId);
                        people.Add(id.FaceId, person);
                    }

                    return people;
                }
            }

            return new Dictionary<Guid, Person>();
        }

        private IList<PersonEmotion> GetEmotionsForPersons(Dictionary<Guid, Person> persons,
            Dictionary<Guid, Emotion> emotions)
        {
            var personEmotions = new List<PersonEmotion>();

            foreach (var e in emotions)
            {
                var faceId = e.Key;
                var emotion = e.Value;
                var person = persons.ContainsKey(faceId) ? persons[faceId] : null;

                personEmotions.Add(new PersonEmotion
                {
                    Emotion = emotion.Scores.GetEmotion(),
                    PersonId = person?.PersonId ?? Guid.Empty,
                    Name = person?.Name ?? "Unknown Person",
                    UserData = person?.UserData ?? "N/A"
                });
            }

            return personEmotions;
        }
    }
}