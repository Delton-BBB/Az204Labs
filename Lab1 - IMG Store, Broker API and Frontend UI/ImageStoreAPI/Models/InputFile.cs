namespace ImageStoreAPI.Models
{
    public class InputFile
    {
        public IFormFile fileContent { get; set; }

        public string? description { get; set; }

        public InputFile() { }


    }
}
