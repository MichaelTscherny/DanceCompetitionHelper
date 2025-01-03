using DanceCompetitionHelper.Data;
using DanceCompetitionHelper.Web.Enums;

using Microsoft.AspNetCore.Mvc.Rendering;

using MigraDoc.DocumentObjectModel;

using PdfSharp.Pdf;

using System.ComponentModel.DataAnnotations;

namespace DanceCompetitionHelper.Web.Models.Pdfs
{
    public class PdfViewModel : ViewModelBase
    {
        public Guid? CompetitionId { get; set; }
        public Guid? CompetitionClassId { get; set; }
        public List<SelectListItem> CompetitionClasses { get; set; } = new List<SelectListItem>();
        public Guid? ParticipantId { get; set; }

        public PdfPageLayout PageLayout { get; set; } = PdfPageLayout.SinglePage;
        public PageFormat PageFormat { get; set; } = PageFormat.A4;
        public Orientation PageOrientation { get; set; } = Orientation.Portrait;
        public bool Shading { get; set; } = true;

        public GroupForViewEnum GroupForView { get; set; } = GroupForViewEnum.None;

        public string? SearchString { get; set; }

        [Range(0, int.MaxValue)]
        public int? From { get; set; }

        [Range(0, int.MaxValue)]
        public int? To { get; set; }


        public string FileName { get; set; } = default!;
        public Stream PdtStream { get; set; } = default!;

        public ParticipantFilter ToParticipantFilter()
        {
            return new ParticipantFilter()
            {
                Name = SearchString,
                StartNumberFrom = From,
                StartNumberTo = To,
            };
        }
    }
}
