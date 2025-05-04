using Data;

namespace Web.Repo.Interface
{
    public interface ISliderRepo
    {
        public Slide? GetSlide(Guid id);
        public void AddSlide(Slide slide);
        public void RemoveSlide(Guid id);
        public void UpdateSlide(Slide slide);
        public void SetActive(Guid id);
        public void SetDeActive(Guid id);
        public IEnumerable<Slide> GetAllSlides();
        public IEnumerable<Slide> GetActiveSlides();
    }
}
