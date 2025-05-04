using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Web.Services
{
    public class FileService
    {
        public string folder { get; private set; }
        public string ToPath(string Name) => Path.Combine(folder, Name);
        public FileService(string Folder)
        {
            if (!Directory.Exists(Path.Combine(Folder))) Directory.CreateDirectory(Path.Combine(Folder)).Create();
            folder = Folder;
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
        public void DeleteFile(string Name)
        {
            File.Delete(ToPath(Name));
        }
        public FileStream? GetStream(string Name)
        {
            return File.Exists(ToPath(Name)) ? File.OpenRead(ToPath(Name)) : null;
        }
        public string? GetString(string Name)
        {
            var fs = GetStream(Name);
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
