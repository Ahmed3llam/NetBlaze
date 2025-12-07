using NetBlaze.Domain.Common;
using NetBlaze.SharedKernel.Enums;
using NetBlaze.SharedKernel.HelperUtilities.General;

namespace NetBlaze.Domain.Entities
{
    public class Vacation : BaseEntity<long>
    {
        // Properties

        public DayOfWeek? Day { get; set; }

        public VacationType VacationType { get; set; }

        public DateTime? VacationDate { get; set; }

        public DateTime? AlternativeDate { get; set; }

        public string? Description { get; set; }

        // Domain Methods

        public static Vacation Create<TDto>(TDto vacationEntityDto) where TDto : class
        {
            return ReflectionMapper.MapToNew<TDto, Vacation>(vacationEntityDto);
        }

        public void Update<TDto>(TDto vacationEntityDto) where TDto : class
        {
            this.MapToExisting(vacationEntityDto);
        }
    }
}
