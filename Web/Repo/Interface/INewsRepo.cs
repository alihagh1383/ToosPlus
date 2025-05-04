using Web.DTO.Read;
using Web.DTO.Write;

namespace Web.Repo.Interface
{
    public interface INewsRepo
    {
        public (IEnumerable<RNewsDto> news, int countall) GetNewsWithCreatorAndCategory(int Count, int Page, string? Cat = null);
        public (IEnumerable<RNewsDto> news, int countall) SearchNewsWithCreatorAndCategory(int Count, int Page, string q, string? Cat = null);
        public (RNewsDto? news, bool Sucsses) AddNews(WNewsDto dto);
        public (RNewsDto? news, bool Sucsses) GetNews(Guid id);
        public (RNewsDto? news, bool Sucsses) UpdateNews(RNewsDto dto); 
    }
}
