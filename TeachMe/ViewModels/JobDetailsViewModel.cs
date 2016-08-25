using TeachMe.Models;

namespace TeachMe.ViewModels
{
    public class JobDetailsViewModel
    {
        private JobMessage[] messages;

        public Job Job { get; set; }
        public JobMessage[] Messages { get { return messages ?? (messages = new JobMessage[0]); } set { messages = value; } }
    }
}