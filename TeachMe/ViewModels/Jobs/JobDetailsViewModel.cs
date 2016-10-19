using TeachMe.Models.Jobs;

namespace TeachMe.ViewModels.Jobs
{
    public class JobDetailsViewModel
    {
        public Job Job { get; set; }
        public JobActionType[] JobAvailableActions { get; set; }
        public bool ChatIsVisible { get; set; }
    }
}