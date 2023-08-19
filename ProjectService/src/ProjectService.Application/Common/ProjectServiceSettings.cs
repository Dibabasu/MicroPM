namespace ProjectService.Application.Common
{
    public class ProjectServiceSettings
    {
        public string ConnectionString { get; set; } = string.Empty;
        public string EtagSalt { get; set; } = string.Empty;
    }

}