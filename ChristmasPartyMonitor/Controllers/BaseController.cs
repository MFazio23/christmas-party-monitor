using System.Web.Http;
using ChristmasPartyMonitor.Services;
using ChristmasPartyMonitor.Utilities;
using Microsoft.ProjectOxford.Face;

namespace ChristmasPartyMonitor.Controllers
{
    public abstract class BaseController : ApiController
    {
        protected FaceServiceClient FaceServiceClient;

        protected EmotionService EmotionService;
        protected FaceService FaceService;
        protected SlackService SlackService;

        protected BaseController()
        {
            this.FaceServiceClient = new FaceServiceClient(Config.FaceServiceClientKey, Config.FaceServiceApiRoot);

            this.EmotionService = new EmotionService();
            this.FaceService = new FaceService(this.FaceServiceClient);
            this.SlackService = new SlackService();
        }
    }
}