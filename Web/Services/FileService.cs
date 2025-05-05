using Microsoft.AspNetCore.Components.Forms;

namespace Web.Services
{
    public class FileService
    {
        public string Folder { get; private set; }
        public string ToPath(string name) => Path.Combine(Folder, name);
        public FileService(string folder)
        {
            if (!Directory.Exists(Path.Combine(folder))) Directory.CreateDirectory(Path.Combine(folder)).Create();
            this.Folder = folder;
        }
        public async Task<string?> UploadFile(IBrowserFile stream)
        {
            var ex = stream.Name.ToLower().Split('.').Last();
            if (new List<string>() { "jpg", "png" }.Contains(ex))
            {
                var name = $"{Guid.CreateVersion7()}.{ex}";
                var f = File.OpenWrite(ToPath(name));
                await stream.OpenReadStream(int.MaxValue).CopyToAsync(f);
                await f.FlushAsync();
                f.Close();
                return name;
            }
            return null;
        }
        public void DeleteFile(string name)
        {
            File.Delete(ToPath(name));
        }
        public FileStream? GetStream(string name)
        {
            return File.Exists(ToPath(name)) ? File.OpenRead(ToPath(name)) : null;
        }
        public string? GetString(string name)
        {
            var fs = GetStream(name);
            var ms = new MemoryStream();
            fs?.CopyTo(ms);
            return fs is { } ? Convert.ToBase64String(ms.ToArray()) : null;
        }
        public static IResult DownloadFile(string name, FileService fs)
        {
            if (fs.GetStream(name) is { } f)
                return Results.File(f);
            else return Results.NotFound();
        }
    }
}
