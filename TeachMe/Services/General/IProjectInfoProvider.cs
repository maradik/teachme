using TeachMe.ProjectsSupport;

namespace TeachMe.Services.General
{
    public interface IProjectInfoProvider
    {
        string GetName(ProjectType projectType);
        string GetTitle(ProjectType projectType);
    }
}