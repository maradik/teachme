namespace TeachMe.ViewModels.Jobs
{
    public class JobActionResult
    {
        public bool HasErrors { get; set; }
        public string ErrorMessage { get; set; }
        public string RedirectUrl { get; set; }
    }
}