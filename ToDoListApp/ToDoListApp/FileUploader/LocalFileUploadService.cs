namespace ToDoListApp.FileUploader
{
    public class LocalFileUploadService : IFileUploader
    {
        private readonly IWebHostEnvironment env;

        public LocalFileUploadService(IWebHostEnvironment environment)
        {
            env = environment;
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            var filePath = Path.Combine(env.ContentRootPath, @"wwwroot\TextFiles", file.FileName);
            using var filestream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(filestream);

            return filePath;
        }
    }
}
