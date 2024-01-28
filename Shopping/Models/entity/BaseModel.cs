using Shopping.Interfaces;
namespace Shopping.Models.entity
{
    public abstract class BaseModel : IBaseModel
    {
        public int Id { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
