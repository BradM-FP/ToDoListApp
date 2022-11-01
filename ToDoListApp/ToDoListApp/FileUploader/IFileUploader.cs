namespace ToDoListApp.FileUploader
{
    public interface IFileUploader
    {

        Task<string> UploadFile(IFormFile file);
        

    }
}
