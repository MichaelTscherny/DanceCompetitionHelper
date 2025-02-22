namespace DanceCompetitionHelper.Web.Models
{
    public class DeleteModalWrapper
    {
        public string AspController { get; set; } = default!;
        public string AspAction { get; set; } = default!;
        public string RouteId { get; set; } = default!;
        public string ModalId { get; set; } = Guid.NewGuid().ToString();

        public string? ModalTitle { get; set; }
        public string InfoText { get; set; } = default!;
    }
}
