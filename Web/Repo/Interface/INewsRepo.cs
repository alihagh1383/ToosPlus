using Web.DTO.Read;
using Web.DTO.Write;

namespace Web.Repo.Interface
{
    public interface INewsRepo
    {
        public (IEnumerable<RNewsDto> news, int countall) GetNewsWithCreatorAndCategory(int count, int page, string? cat = null);
        public (IEnumerable<RNewsDto> news, int countall) SearchNewsWithCreatorAndCategory(int count, int page, string q, string? cat = null);
        public (RNewsDto? news, bool Sucsses) AddNews(WNewsDto dto);
        public (RNewsDto? news, bool Sucsses) GetNews(Guid id);
        public (RNewsDto? news, bool Sucsses) UpdateNews(RNewsDto dto); 
    }
}
