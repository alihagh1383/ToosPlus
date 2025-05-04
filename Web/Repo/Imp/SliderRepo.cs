using Data;
using Microsoft.EntityFrameworkCore;
using Web.Repo.Interface;

namespace Web.Repo.Imp
{
    public class SliderRepo(NewsDbContext dbContext) : ISliderRepo
    {
        public void AddSlide(Slide slide)
        {
            dbContext.Slides.Add(new Slide
            {
                Description = slide.Description,
                Image = slide.Image,
                IsActive = slide.IsActive,
                Title = slide.Title
            });
            dbContext.SaveChanges();
        }

        public IEnumerable<Slide> GetActiveSlides()
        {
            return dbContext.Slides.Where(p => p.IsActive);
        }

        public IEnumerable<Slide> GetAllSlides()
        {
            return dbContext.Slides;
        }

        public Slide? GetSlide(Guid id)
        {
            return dbContext.Slides.FirstOrDefault(p => p.Id == id);
        }

        public void RemoveSlide(Guid id)
        {
            dbContext.Slides.Where(p => p.Id == id).ExecuteDelete();
        }

        public void SetActive(Guid id)
        {
            dbContext.Slides.Where(p => p.Id == id)
                .ExecuteUpdate(p => p.SetProperty(p => p.IsActive, true));
        }

        public void SetDeActive(Guid id)
        {
            dbContext.Slides.Where(p => p.Id == id)
                           .ExecuteUpdate(p => p.SetProperty(p => p.IsActive, false));
        }

        public void UpdateSlide(Slide slide)
        {
            dbContext.Slides.Update(slide);
            dbContext.SaveChanges();
        }
    }
}
