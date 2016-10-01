using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeachMe.Helpers.Settings;
using TeachMe.ProjectsSupport;

namespace TeachMe.Services.General
{
    public class ProjectInfoProvider : IProjectInfoProvider
    {
        public string GetTitle(ProjectType projectType)
        {
            switch (projectType)
            {
                case ProjectType.Student:
                    return ApplicationSettings.StudentProjectTitle;
                case ProjectType.Teacher:
                    return ApplicationSettings.TeacherProjectTitle;
                default:
                    throw new NotImplementedException(projectType.ToString());
            }
        }

        public string GetName(ProjectType projectType)
        {
            switch (projectType)
            {
                case ProjectType.Student:
                    return ApplicationSettings.StudentProjectName;
                case ProjectType.Teacher:
                    return ApplicationSettings.TeacherProjectName;
                default:
                    throw new NotImplementedException(projectType.ToString());
            }
        }
    }
}