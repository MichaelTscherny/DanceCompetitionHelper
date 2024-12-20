using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DanceCompetitionHelper.Web.Models
{
    public class ErrorViewModel : ViewModelBase
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public List<ModelStateEntry> ModelErrors { get; set; } = new List<ModelStateEntry>();
    }
}