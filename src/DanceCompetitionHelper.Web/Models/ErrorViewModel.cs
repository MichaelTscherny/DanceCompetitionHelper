using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DanceCompetitionHelper.Web.Models
{
    public class ErrorViewModel : ViewModelBase
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => string.IsNullOrEmpty(RequestId) == false;

        public List<ModelStateEntry> ModelErrors { get; set; } = new List<ModelStateEntry>();
    }
}