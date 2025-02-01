using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Helper;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Models.Pdfs;

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

using PdfSharp.Fonts;

namespace DanceCompetitionHelper.Web.Helper.Documents
{
    public class PdfGenerator
    {
        public Unit DefaultFontSize { get; set; } = Unit.FromPoint(10);
        public Unit DefaultHeaderFooterFontSize { get; set; } = Unit.FromPoint(8);

        public Color DefaultTableHeaderColor { get; set; } = Colors.LightGray;
        public Color DefaultTableShadingColor { get; set; } = new Color(0xe0, 0xe0, 0xe0);

        #region General Stuff

        static PdfGenerator()
        {
            GlobalFontSettings.FontResolver = new UserFontResolver();
        }

        #endregion General Stuff

        #region Helper

        public Document CreateDefaultDocument(
            string title,
            string subject,
            Competition? competition)
        {
            var retDoc = new Document
            {
                Info =
                {
                    Title = title,
                    Subject = string.Format(
                        "{0} - {1}",
                        subject,
                        competition.GetCompetitionName()),
                    Comment = competition.GetCompetitionName(),
                    Author = DanceCompetitionHelper.DanceCompetitionHelperInfoString,
                },
            };

            return retDoc;
        }

        public Document SetDefaultStyle(
            Document document)
        {
            // ---- FONTS ----
            var style = document.Styles[StyleNames.Normal]!;

            style.Font.Size = DefaultFontSize;

            // ---- FOOTER ----
            // Style for footer.
            style = document.Styles[StyleNames.Footer]!;
            style.ParagraphFormat.ClearAll();
            style.ParagraphFormat.TabStops.AddTabStop(Unit.FromCentimeter(8), TabAlignment.Center);
            style.ParagraphFormat.TabStops.AddTabStop(Unit.FromCentimeter(16), TabAlignment.Right);

            return document;
        }

        public Document SetDefaultHeaderFooter(
            Document document,
            Competition? competition,
            CompetitionClass? competitionClass,
            string? headerTitle)
        {
            var headers = document.LastSection.Headers.Primary;

            // ----
            var compName = string.Empty;
            if (competition != null)
            {
                compName = string.Format(
                    "{0} - {1}",
                    competition.CompetitionDate.ToShortDateString(),
                    competition.GetCompetitionName());
            }

            foreach (var curText in new[]
                {
                    compName,
                    competitionClass?.GetCompetitionClassName(),
                    headerTitle,
                }
                .Where(
                    x => string.IsNullOrWhiteSpace(
                        x) == false))
            {
                headers.AddParagraph(
                    curText ?? string.Empty);
            }

            headers.Format.Font.Size = DefaultHeaderFooterFontSize;
            headers.Format.Alignment = ParagraphAlignment.Center;

            // ----
            var footers = document.LastSection.Footers.Primary;

            var footerPara = new Paragraph();
            // left
            footerPara.AddDateField();
            footerPara.AddTab();

            // center
            footerPara.AddText(
                DanceCompetitionHelper.DanceCompetitionHelperInfoString);
            footerPara.AddTab();

            // right
            footerPara.AddPageField();
            footerPara.AddText("/");
            footerPara.AddNumPagesField();

            footers.Add(
                footerPara);

            footers.Format.Font.Size = DefaultHeaderFooterFontSize;
            footers.Format.Alignment = ParagraphAlignment.Center;

            return document;
        }

        public Document SetDefaultPageSetup(
            Document document,
            PdfViewModel model)
        {
            // CAUTION: Do not use "document.DefaultPageSetup.Clone()";
            // the PageFormat will not change any sizes
            document.LastSection.PageSetup.PageFormat = model.PageFormat;
            document.LastSection.PageSetup.Orientation = model.PageOrientation;

            return document;
        }

        public Stream ToPdfStream(
            Document document,
            PdfViewModel model)
        {
            var pdfRenderer = new PdfDocumentRenderer
            {
                // Associate the MigraDoc document with a renderer.
                Document = document,
                PdfDocument =
                {
                    // Change some settings before rendering the MigraDoc document.
                    PageLayout = model.PageLayout,
                    ViewerPreferences =
                    {
                        // FitWindow = true,
                        HideToolbar = true,
                        HideWindowUI = true,
                        HideMenubar = true,
                    }
                }
            };

            // Layout and render document to PDF.
            pdfRenderer.RenderDocument();

            var memStream = new MemoryStream();
            pdfRenderer.PdfDocument.Save(
                memStream);

            return memStream;
        }

        public (Unit ContentWidth, Unit ContentHeight) GetEffectiveContentSizes(
            PageSetup pageSetup)
        {
            if (pageSetup == null)
            {
                return (Unit.Empty, Unit.Empty);
            }

            PageSetup.GetPageSize(
                pageSetup.PageFormat,
                out var pageWidth,
                out var pageHeight);

            if (pageSetup.Orientation == Orientation.Landscape)
            {
                // swap!..
                (pageWidth, pageHeight) = (pageHeight, pageWidth);
            }

            var defaultPageSetup = pageSetup.Document?.DefaultPageSetup ?? new PageSetup();
            var useLeftMargin = pageSetup.LeftMargin.IsEmpty
                ? defaultPageSetup.LeftMargin
                : pageSetup.LeftMargin;
            var useRightMargin = pageSetup.RightMargin.IsEmpty
                ? defaultPageSetup.RightMargin
                : pageSetup.RightMargin;
            var useTopMargin = pageSetup.TopMargin.IsEmpty
                ? defaultPageSetup.TopMargin
                : pageSetup.TopMargin;
            var useBottomMargin = pageSetup.BottomMargin.IsEmpty
                ? defaultPageSetup.BottomMargin
                : pageSetup.BottomMargin;

            return (
                pageWidth - useLeftMargin - useRightMargin,
                pageHeight - useTopMargin - useBottomMargin);
        }

        public int GetMaxColumnsCount(
            PageSetup pageSetup)
        {
            var retMaxColumns = 4;
            var useOrientation = pageSetup?.Orientation ?? Orientation.Portrait;

            switch (pageSetup?.PageFormat)
            {
                // ----
                case PageFormat.A6 when useOrientation == Orientation.Portrait:
                    retMaxColumns = 1;
                    break;
                case PageFormat.A6 when useOrientation == Orientation.Landscape:
                    retMaxColumns = 2;
                    break;

                // ----
                case PageFormat.A5 when useOrientation == Orientation.Portrait:
                    retMaxColumns = 2;
                    break;
                case PageFormat.A5 when useOrientation == Orientation.Landscape:
                    retMaxColumns = 4;
                    break;

                // ----
                case PageFormat.A4 when useOrientation == Orientation.Portrait:
                case PageFormat.Letter when useOrientation == Orientation.Portrait:
                    retMaxColumns = 4;
                    break;

                case PageFormat.A4 when useOrientation == Orientation.Landscape:
                case PageFormat.Letter when useOrientation == Orientation.Landscape:
                    retMaxColumns = 8;
                    break;

                // ----
                case PageFormat.A3 when useOrientation == Orientation.Portrait:
                    retMaxColumns = 8;
                    break;
                case PageFormat.A3 when useOrientation == Orientation.Landscape:
                    retMaxColumns = 12;
                    break;

                // ----
                case PageFormat.A2 when useOrientation == Orientation.Portrait:
                    retMaxColumns = 13;
                    break;
                case PageFormat.A2 when useOrientation == Orientation.Landscape:
                    retMaxColumns = 19;
                    break;

                // ----
                case PageFormat.A1 when useOrientation == Orientation.Portrait:
                    retMaxColumns = 19;
                    break;
                case PageFormat.A1 when useOrientation == Orientation.Landscape:
                    retMaxColumns = 29;
                    break;

                // ----
                case PageFormat.A0 when useOrientation == Orientation.Portrait:
                    retMaxColumns = 29;
                    break;
                case PageFormat.A0 when useOrientation == Orientation.Landscape:
                    retMaxColumns = 40;
                    break;
            }

            return retMaxColumns;
        }

        #endregion Helper

        #region Get PDFs stuff

        public Stream GetMultipleStarters(
            Competition? competition,
            List<MultipleStarter> multipleStarters,
            PdfViewModel model)
        {
            var document = CreateDefaultDocument(
                "Multiple Starters",
                string.Empty,
                competition);
            SetDefaultStyle(
                document);
            SetDefaultHeaderFooter(
                document,
                competition,
                null,
                string.Format(
                    "{0} Multiple Starters",
                    multipleStarters.Count));
            SetDefaultPageSetup(
                document,
                model);

            // ----
            var (useWidht, _) = GetEffectiveContentSizes(
                document.LastSection.PageSetup);
            var dispCunksSize = GetMaxColumnsCount(
                document.LastSection.PageSetup);
            var allDisplayInfoBlocks = ViewModelExtensions
                .ExtractDisplayInfo(
                    multipleStarters,
                    model.GroupForView);

            // for (var blockIdx = 0; blockIdx < allDisplayInfoBlocks.Count; blockIdx++)
            // foreach (var allDisplayInfos in allDisplayInfoBlocks)
            foreach (var allDisplayInfo in allDisplayInfoBlocks)
            {
                // var allDisplayInfo = allDisplayInfoBlocks[blockIdx];
                var allDisplayInfoCount = allDisplayInfo.Count;

                foreach (var curDisplayInfos in allDisplayInfo
                    .Chunk(
                        dispCunksSize))
                {
                    var curSection = document.LastSection;

                    var table = new Table
                    {
                        Borders =
                        {
                            Width = 0.75,
                            Visible = true,
                        },
                    };

                    // -- prepare the table...
                    var helpLayouter = new SimplePdfTableLayouter(
                        useWidht);
                    helpLayouter.AddColumn(
                        Unit.FromCentimeter(
                            6));
                    helpLayouter.AddColumns(
                        curDisplayInfos.Length);
                    helpLayouter.ApplyTo(
                        table);

                    // -- fill header
                    var curRow = table.AddRow();
                    curRow.HeadingFormat = true;
                    curRow.Format.Font.Bold = true;

                    if (model.Shading)
                    {
                        curRow.Shading.Color = DefaultTableHeaderColor;
                    }

                    var curCellId = 0;
                    curRow.Cells[curCellId].AddParagraph(
                        string.Format(
                            "Name ({0} x {1}) (P A/B; S A/B - Starts)",
                            multipleStarters?.Count ?? 0,
                            allDisplayInfoCount));

                    foreach (var curDisplayInfo in curDisplayInfos)
                    {
                        curCellId++;
                        curRow.Cells[curCellId].AddParagraph(
                            string.Format(
                                "{0}{1}",
                                curDisplayInfo.ClassName,
                                curDisplayInfo.Ignore
                                    ? " [IGNORE!]"
                                    : string.Empty));
                    }

                    // -- fill data
                    foreach (var item in multipleStarters ?? Enumerable.Empty<MultipleStarter>())
                    {
                        curRow = table.AddRow();
                        curCellId = 0;
                        Color? curShading = (curRow.Index % 2 == 0)
                            ? DefaultTableShadingColor
                            : null;

                        curRow.Cells[curCellId].AddParagraph(
                            string.Format(
                                "{0} ({1} / {2}; {3} / {4} - {5})",
                                item.Name,
                                // 1
                                item.PointsA,
                                item.PointsB,
                                // 3
                                item.StartsA,
                                item.StartsB,
                                // 5
                                item.CompetitionClasses.Count));

                        foreach (var curDisplayInfo in curDisplayInfos)
                        {
                            curCellId++;
                            var curCellText = string.Empty;

                            if (item.CompetitionClassNamesByClassId.ContainsKey(curDisplayInfo.ClassId))
                            {
                                var usePart = item.Participants
                                    .FirstOrDefault(
                                        x => x.CompetitionClassId == curDisplayInfo.ClassId);

                                curCellText = string.Format(
                                    "# {0}",
                                    item.StartnumberByClassId[curDisplayInfo.ClassId]);
                            }

                            curRow[curCellId].VerticalAlignment = VerticalAlignment.Center;
                            var newPara = curRow[curCellId].AddParagraph(
                                curCellText);
                            newPara.Format.Alignment = ParagraphAlignment.Center;
                        }

                        if (model.Shading
                            && curShading != null)
                        {
                            curRow.Shading.Color = curShading ?? Colors.White;
                        }
                    }

                    // -- add table
                    curSection.Add(
                        table);

                    document.AddSection();
                }
            }

            // remove last section...
            document.Sections.RemoveObjectAt(
                document.Sections.Count - 1);

            // ----
            return ToPdfStream(
                document,
                model);
        }

        public Stream GetMultipleStartersDependentClassesView(
            Competition? competition,
            List<MultipleStarter> multipleStarters,
            PdfViewModel model)
        {
            var document = CreateDefaultDocument(
                "Multiple Starters - Dependent Classes",
                string.Empty,
                competition);
            SetDefaultStyle(
                document);
            SetDefaultHeaderFooter(
                document,
                competition,
                null,
                string.Format(
                    "Dependent Classes for Multiple Starters",
                    multipleStarters.Count));
            SetDefaultPageSetup(
                document,
                model);

            // ----
            var (useWidht, _) = GetEffectiveContentSizes(
                document.LastSection.PageSetup);
            var dispCunksSize = GetMaxColumnsCount(
                document.LastSection.PageSetup);
            var (dependentClassIdsAndParticipants, allDisplayInfoBlocks) = ViewModelExtensions
                .ExtractMultipleStarterDependentClasses(
                    multipleStarters,
                    model.GroupForView);

            foreach (var allDisplayInfo in allDisplayInfoBlocks)
            {
                var allDisplayInfoCount = allDisplayInfo.Count;

                foreach (var (curClassIds, curMultipleStarters) in dependentClassIdsAndParticipants)
                {
                    foreach (var curClassesByChunk in curClassIds.Chunk(
                        dispCunksSize))
                    {
                        var curSection = document.LastSection;

                        var table = new Table
                        {
                            Borders =
                            {
                                Width = 0.75,
                                Visible = true,
                            },
                        };

                        // -- prepare the table...
                        var helpLayouter = new SimplePdfTableLayouter(
                            useWidht);
                        helpLayouter.AddColumn(
                            Unit.FromCentimeter(
                                6));
                        // TODO: min/max width??
                        helpLayouter.AddColumns(
                            curClassesByChunk.Length);
                        helpLayouter.ApplyTo(
                            table);

                        // -- fill header
                        var curRow = table.AddRow();
                        curRow.HeadingFormat = true;
                        curRow.Format.Font.Bold = true;

                        if (model.Shading)
                        {
                            curRow.Shading.Color = DefaultTableHeaderColor;
                        }

                        var curCellId = 0;
                        curRow.Cells[curCellId].AddParagraph(
                            string.Format(
                                "Name ({0} x {1}) (P A/B; S A/B - Starts)",
                                curMultipleStarters?.Count ?? 0,
                                allDisplayInfoCount));

                        foreach (var curDisplayInfo in allDisplayInfo)
                        {
                            if (curClassesByChunk.Contains(
                                curDisplayInfo.ClassId) == false)
                            {
                                continue;
                            }

                            curCellId++;
                            curRow.Cells[curCellId].AddParagraph(
                                string.Format(
                                    "{0}{1}",
                                    curDisplayInfo.ClassName,
                                    curDisplayInfo.Ignore
                                        ? " [IGNORE!]"
                                        : string.Empty));
                        }

                        // -- fill data
                        foreach (var item in curMultipleStarters ?? Enumerable.Empty<MultipleStarter>())
                        {
                            curRow = table.AddRow();
                            curCellId = 0;
                            Color? curShading = (curRow.Index % 2 == 0)
                                ? DefaultTableShadingColor
                                : null;

                            curRow.Cells[curCellId].AddParagraph(
                                string.Format(
                                    "{0} ({1} / {2}; {3} / {4} - {5})",
                                    item.Name,
                                    // 1
                                    item.PointsA,
                                    item.PointsB,
                                    // 3
                                    item.StartsA,
                                    item.StartsB,
                                    // 5
                                    item.CompetitionClasses.Count));

                            foreach (var curDisplayInfo in allDisplayInfo)
                            {
                                if (curClassesByChunk.Contains(
                                    curDisplayInfo.ClassId) == false)
                                {
                                    continue;
                                }

                                curCellId++;
                                var curCellText = string.Empty;

                                if (item.CompetitionClassNamesByClassId.ContainsKey(curDisplayInfo.ClassId))
                                {
                                    var usePart = item.Participants
                                        .FirstOrDefault(
                                            x => x.CompetitionClassId == curDisplayInfo.ClassId);

                                    curCellText = string.Format(
                                        "# {0}",
                                        item.StartnumberByClassId[curDisplayInfo.ClassId]);
                                }

                                curRow[curCellId].VerticalAlignment = VerticalAlignment.Center;
                                var newPara = curRow[curCellId].AddParagraph(
                                    curCellText);
                                newPara.Format.Alignment = ParagraphAlignment.Center;
                            }

                            if (model.Shading
                                && curShading != null)
                            {
                                curRow.Shading.Color = curShading ?? Colors.White;
                            }
                        }

                        // -- add table
                        curSection.Add(
                            table);

                        document.AddSection();
                    }
                }
            }

            // remove last section...
            document.Sections.RemoveObjectAt(
                document.Sections.Count - 1);

            // ----
            return ToPdfStream(
                document,
                model);
        }

        public Stream GetPossiblePromotions(
            Competition? competition,
            List<Participant> possiblePromotions,
            List<MultipleStarter> multipleStarters,
            PdfViewModel model)
        {
            var document = CreateDefaultDocument(
                "Possible Promotions",
                string.Empty,
                competition);
            SetDefaultStyle(
                document);
            SetDefaultHeaderFooter(
                document,
                competition,
                null,
                string.Format(
                    "{0} Possible Promotions",
                    possiblePromotions.Count));
            SetDefaultPageSetup(
                document,
                model);

            // ----
            var (useWidht, _) = GetEffectiveContentSizes(
                document.LastSection.PageSetup);
            var (helpIncludedClasses, maxDisplayClasses) = ViewModelExtensions
                .ExtractPossiblePromotionCompetitionClasses(
                    possiblePromotions);
            var displayInfoCompClasses = ViewModelExtensions.ExtractDisplayInfo(
                helpIncludedClasses,
                Enums.GroupForViewEnum.None)
                .FirstOrDefault()
                ?? new List<(string ClassName, Guid ClassId, bool Ignore)>();
            var displayInfoCompClassesIds = new List<Guid>(
                displayInfoCompClasses.Select(
                    x => x.ClassId));

            var partInfoByCompetitionClass = new Dictionary<Guid, Dictionary<Guid, Participant>>();

            // helper to get the correct start number for a competition class
            foreach (var curMiltiStarter in multipleStarters ?? Enumerable.Empty<MultipleStarter>())
            {
                foreach (var curPart in curMiltiStarter.Participants)
                {
                    var usePartId = curPart.ParticipantId;
                    if (partInfoByCompetitionClass.TryGetValue(
                        usePartId,
                        out var byClassId) == false)
                    {
                        byClassId = new Dictionary<Guid, Participant>();
                        partInfoByCompetitionClass[usePartId] = byClassId;
                    }

                    foreach (var curPartClassInfos in curMiltiStarter.Participants)
                    {
                        byClassId[curPartClassInfos.CompetitionClassId] = curPartClassInfos;
                    }
                }
            }

            var curSection = document.LastSection;

            // ----
            foreach (var curPossPromotion in possiblePromotions ?? Enumerable.Empty<Participant>())
            {
                var curClassesByClassId = curPossPromotion.DisplayInfo?.PromotionInfo?.IncludedCompetitionClasses
                    .ToDictionary(
                        x => x.CompetitionClassId)
                        ?? new Dictionary<Guid, CompetitionClass>();

                var table = new Table
                {
                    Borders =
                    {
                        Width = 0.75,
                        Visible = true,
                    },
                };

                // -- prepare the table...
                var helpLayouter = new SimplePdfTableLayouter(
                    useWidht);
                helpLayouter.AddColumn(
                    Unit.FromCentimeter(
                        6));
                helpLayouter.AddColumns(
                    curClassesByClassId.Count,
                    Unit.FromCentimeter(
                        3));
                helpLayouter.ApplyTo(
                    table);

                // -- fill header
                var curCellId = 0;
                var curRow = table.AddRow();
                curRow.HeadingFormat = true;
                curRow.Format.Font.Bold = true;
                curRow[curCellId].AddParagraph(
                    "Name");

                foreach (var curClassId in displayInfoCompClassesIds)
                {
                    var classInfo = string.Empty;

                    if (curClassesByClassId.TryGetValue(
                        curClassId,
                        out var curClass))
                    {
                        classInfo = curClass.GetCompetitionClassName();

                        if (curClass?.Ignore ?? false)
                        {
                            classInfo += " [IGNORE!]";
                        }
                    }

                    if (string.IsNullOrEmpty(
                        classInfo) == false)
                    {
                        curCellId++;
                        curRow[curCellId].AddParagraph(
                            classInfo);
                    }
                }

                var useStartNumber = -1;
                var isAlreadyPromoted = curPossPromotion.DisplayInfo?.PromotionInfo?.AlreadyPromoted ?? false;

                // -- DATA -- ROW 0
                curCellId = 0;
                curRow = table.AddRow();
                // curRow[curCellId].MergeDown = 1;
                curRow[curCellId].AddParagraph(
                    curPossPromotion.GetNames());

                if (isAlreadyPromoted)
                {
                    if (model.Shading)
                    {
                        curRow[curCellId].Shading.Color = Colors.PaleVioletRed;
                    }
                    else
                    {
                        curRow[curCellId].AddParagraph(
                            "!!!");
                    }
                }

                // -- Points/Starts
                foreach (var curClassId in displayInfoCompClassesIds)
                {
                    if (curClassesByClassId.TryGetValue(
                        curClassId,
                        out var curClass))
                    {
                        if (partInfoByCompetitionClass.TryGetValue(
                            curPossPromotion.ParticipantId,
                            out var compClassesByPartId))
                        {
                            if (compClassesByPartId.TryGetValue(
                                curClassId,
                                out var foundPart))
                            {
                                useStartNumber = foundPart.StartNumber;
                            }
                        }
                        else
                        {
                            if (curClassId == curPossPromotion.CompetitionClassId)
                            {
                                useStartNumber = curPossPromotion.StartNumber;
                            }
                        }

                        var curClassPointsInfo = "-";
                        if (curClass.CompetitionClassId == curPossPromotion.CompetitionClassId)
                        {
                            curClassPointsInfo = string.Format(
                                "{0} / {1}",
                                curClass.MinPointsForPromotion,
                                curClass.MinStartsForPromotion);
                        }

                        curCellId++;
                        curRow[curCellId].VerticalAlignment = VerticalAlignment.Center;
                        var par = curRow[curCellId].AddParagraph(
                            curClassPointsInfo);
                        par.Format.Alignment = ParagraphAlignment.Center;

                        par = curRow[curCellId].AddParagraph(
                            string.Format(
                                "#{0} +{1} / +1",
                                useStartNumber,
                                curClass.PointsForFirst));
                        par.Format.Alignment = ParagraphAlignment.Center;
                        par.Format.LineSpacingRule = LineSpacingRule.Exactly;
                    }
                }


                // -- DATA -- ROW 1
                curCellId = 0;
                curRow = table.AddRow();
                curRow[curCellId].AddParagraph(
                    string.Format(
                        "A: {0} / {1}",
                        curPossPromotion.OrgPointsPartA,
                        curPossPromotion.OrgStartsPartA));

                for (curCellId = 1; curCellId < table.Columns.Count; curCellId++)
                {
                    curRow[curCellId].AddParagraph(
                        "/");
                    curRow[curCellId].Format.Alignment = ParagraphAlignment.Center;
                }

                // -- DATA -- ROW 2
                if (curPossPromotion.OrgPointsPartB.HasValue)
                {
                    curCellId = 0;
                    curRow = table.AddRow();
                    curRow[curCellId].AddParagraph(
                        string.Format(
                            "B: {0} / {1}",
                            curPossPromotion.OrgPointsPartB,
                            curPossPromotion.OrgStartsPartB));

                    for (curCellId = 1; curCellId < table.Columns.Count; curCellId++)
                    {
                        curRow[curCellId].AddParagraph(
                            "/");
                        curRow[curCellId].Format.Alignment = ParagraphAlignment.Center;
                    }
                }

                /*
                // -- DATA -- ROW 3 (dummy)
                curCellId = 0;
                curRow = table.AddRow();
                */

                curSection.AddParagraph();
                curSection.Add(
                    table);
            }

            /* ?? remove last section...
            document.Sections.RemoveObjectAt(
                document.Sections.Count - 1);
            */

            // ----
            return ToPdfStream(
                document,
                model);
        }

        public Stream GetNumberCards(
            Competition? competition,
            List<Participant> pariticpants,
            PdfViewModel model)
        {
            var document = CreateDefaultDocument(
                "Number Cards",
                string.Empty,
                competition);
            SetDefaultStyle(
                document);
            SetDefaultPageSetup(
                document,
                model);

            // our special stuff...
            var curSection = document.LastSection;
            var useMargin = Unit.FromMillimeter(5);
            curSection.PageSetup.LeftMargin = useMargin; // Unit.Zero;
            curSection.PageSetup.RightMargin = useMargin; // Unit.Zero;
            curSection.PageSetup.TopMargin = useMargin; // Unit.Zero;
            curSection.PageSetup.BottomMargin = useMargin; // Unit.Zero;

            var (useWidht, useHeight) = GetEffectiveContentSizes(
                curSection.PageSetup);

            curSection.PageSetup.BottomMargin = Unit.Zero;

            foreach (var curParts in pariticpants.Chunk(2))
            {
                Participant firstPart = curParts[0];
                Participant? secondPart = curParts.Length >= 2
                    ? curParts[1]
                    : null;

                // ----
                var table = new Table
                {
                    Borders =
                {
                    Width = 0.75,
                    // Visible = true,
                    Visible =  false,
                },
                    // LeftPadding = useMargin,
                };

                // -- prepare the table...
                var tableColumn = table.AddColumn(
                    useWidht);
                tableColumn.Format.Alignment = ParagraphAlignment.Center;
                tableColumn.LeftPadding = Unit.Zero;
                tableColumn.RightPadding = Unit.Zero;

                var numberInfoHeight = Unit.FromCentimeter(1);
                var numberHeight = (useHeight / 2.0) - numberInfoHeight - useMargin;
                // var numberTopHeight = Unit.FromMillimeter(5);

                // "Arial Narrow"
                var useNumberFontSize3Digits = Unit.FromPoint(420);
                var useNumberFontSize4Digits = Unit.FromPoint(320);
                var useNumberInfoFontSize = Unit.FromPoint(8);

                // ------
                // first row...
                var firstNumber = table.AddRow();
                firstNumber[0].VerticalAlignment = VerticalAlignment.Center;
                firstNumber.Height = numberHeight;
                firstNumber.HeightRule = RowHeightRule.Exactly;
                firstNumber.TopPadding = Unit.Zero;
                firstNumber.BottomPadding = Unit.Zero;
                firstNumber.KeepWith = 1;

                var firstNumberString = firstPart.StartNumber.ToString("D0");

                var firstNumberPara = firstNumber.Cells[0].AddParagraph(
                    firstNumberString);
                // firstNumberPara.Format.Borders.Visible = true;
                firstNumberPara.Format.Alignment = ParagraphAlignment.Center;
                firstNumberPara.Format.Font.Name = "Arial Narrow";
                firstNumberPara.Format.Font.Size = firstNumberString.Length <= 3
                    ? useNumberFontSize3Digits
                    : useNumberFontSize4Digits;
                firstNumberPara.Format.Font.Bold = true;

                var firstNumberInfo = table.AddRow();
                firstNumberInfo[0].VerticalAlignment = VerticalAlignment.Center;
                firstNumberInfo.Height = numberInfoHeight;
                firstNumberInfo.HeightRule = RowHeightRule.Exactly;
                firstNumberInfo.TopPadding = Unit.Zero;
                firstNumberInfo.BottomPadding = Unit.Zero;

                var firstNumberInfoPara = firstNumberInfo.Cells[0].AddParagraph(
                    string.Format(
                        "{0} - {1}",
                        firstPart.Competition.CompetitionDate.ToShortDateString(),
                        firstPart.CompetitionClass.GetCompetitionClassName()));
                firstNumberInfoPara.Format.Font.Size = useNumberInfoFontSize;

                // ------
                // filler row...
                var filler = table.AddRow();
                filler.Height = useMargin + Unit.FromMillimeter(4); //  Unit.FromMillimeter(2);
                filler.HeightRule = RowHeightRule.Exactly;
                filler.TopPadding = Unit.Zero;
                filler.BottomPadding = Unit.Zero;

                // ------
                // second row...
                var secondNumber = table.AddRow();
                secondNumber[0].VerticalAlignment = VerticalAlignment.Center;
                secondNumber.Height = numberHeight; // - useMargin;
                secondNumber.HeightRule = RowHeightRule.Exactly;
                secondNumber.TopPadding = Unit.Zero;
                secondNumber.BottomPadding = Unit.Zero;
                secondNumber.KeepWith = 1;

                if (secondPart != null)
                {
                    var secondNumberString = secondPart.StartNumber.ToString("D0");
                    var secondNumberPara = secondNumber.Cells[0].AddParagraph(
                        secondNumberString);
                    secondNumberPara.Format.Alignment = ParagraphAlignment.Center;
                    secondNumberPara.Format.Font.Name = "Arial Narrow";
                    secondNumberPara.Format.Font.Size = secondNumberString.Length <= 3
                        ? useNumberFontSize3Digits
                        : useNumberFontSize4Digits;
                    secondNumberPara.Format.Font.Bold = true;
                }

                var secondNumberInfo = table.AddRow();
                secondNumberInfo[0].VerticalAlignment = VerticalAlignment.Center;
                secondNumberInfo.Height = numberInfoHeight;
                secondNumberInfo.HeightRule = RowHeightRule.Exactly;
                secondNumberInfo.TopPadding = Unit.Zero;
                secondNumberInfo.BottomPadding = Unit.Zero;

                if (secondPart != null)
                {
                    var secondNumberInfoPara = secondNumberInfo.Cells[0].AddParagraph(
                        string.Format(
                            "{0} - {1}",
                            secondPart.Competition.CompetitionDate.ToShortDateString(),
                            secondPart.CompetitionClass.GetCompetitionClassName()));
                    secondNumberInfoPara.Format.Font.Size = useNumberInfoFontSize;
                }

                // ----
                curSection.Add(
                    table);

                // document.AddSection();
            }

            /* remove last section...
            document.Sections.RemoveObjectAt(
                document.Sections.Count - 1);
            */

            // ----
            return ToPdfStream(
                document,
                model);
        }

        public Stream GetParticipants(
            Competition? competition,
            List<Participant> participants,
            PdfViewModel model)
        {
            var document = CreateDefaultDocument(
                "Participants",
                string.Empty,
                competition);
            SetDefaultStyle(
                document);
            SetDefaultHeaderFooter(
                document,
                competition,
                null,
                string.Format(
                    "{0} Participants",
                    participants.Count));
            SetDefaultPageSetup(
                document,
                model);

            var partsByCompetitionId = participants
                .GroupBy(
                    x => x.CompetitionClassId,
                    (compClassId, parts) => new
                    {
                        CompetitionClassId = compClassId,
                        Participants = parts
                            .OrderBy(
                                x => x.StartNumber)
                            .ThenBy(
                                x => x.NamePartA)
                            .ThenBy(
                                x => x.NamePartB)
                            .ToList()
                    });

            var (useWidht, _) = GetEffectiveContentSizes(
                document.LastSection.PageSetup);

            var sectionsAdded = 0;

            var curCompClass = string.Empty;
            foreach (var curPartsByCompClass in partsByCompetitionId)
            {
                var firstPart = curPartsByCompClass.Participants.First();

                var curSection = sectionsAdded == 0
                    ? document.LastSection
                    : document.AddSection();

                sectionsAdded++;
                var curPar = curSection.AddParagraph(
                    string.Format(
                        "{0} - {1} Participants",
                        firstPart.CompetitionClass.GetCompetitionClassName(),
                        curPartsByCompClass.Participants.Count),
                    StyleNames.Heading1);
                curPar.Format.SpaceAfter = Unit.FromMillimeter(2);
                curPar.Format.Font.Bold = true;

                var table = new Table
                {
                    Borders =
                    {
                        Width = 0.75,
                        // Visible = true,
                        Visible = false,
                    },
                    // LeftPadding = useMargin,
                };

                // -- prepare the table...
                var helpLayouter = new SimplePdfTableLayouter(
                    useWidht);
                helpLayouter.AddColumn(
                    Unit.FromCentimeter(2));
                helpLayouter.AddColumn(
                    0.0);
                helpLayouter.ApplyTo(
                    table);

                foreach (var curPart in curPartsByCompClass.Participants)
                {
                    var curRow = table.AddRow();
                    curRow.KeepWith = 1;
                    curRow.TopPadding = Unit.FromMillimeter(2);

                    curRow.Cells[0].AddParagraph(
                        curPart.StartNumber.ToString("D0"));
                    curRow.Cells[0].MergeDown = 1;
                    curRow.Cells[1].AddParagraph(
                        curPart.GetNames());

                    curRow = table.AddRow();
                    curRow[1].AddParagraph(
                        curPart.ClubName ?? "-");

                    curRow.Borders.Bottom.Visible = true;
                    curRow.BottomPadding = Unit.FromMillimeter(2);

                }

                // ----
                curSection.Add(
                    table);
            }

            // ----
            return ToPdfStream(
                document,
                model);
        }

        [Obsolete("do not use! Just a test dummy!")]
        public Stream GetDummyPdf(
            Competition? competition,
            CompetitionClass? competitionClass,
            PdfViewModel model)
        {
            var document = CreateDefaultDocument(
                "Dummy PDF",
                "just a test",
                competition);
            SetDefaultStyle(
                document);
            SetDefaultHeaderFooter(
                document,
                competition,
                competitionClass,
                null);
            SetDefaultPageSetup(
                document,
                model);

            document.LastSection.AddParagraph(
                "Test",
                StyleNames.Heading1);


            var table = new Table
            {
                Borders =
                {
                    Width = 0.75,
                },
            };

            // document.LastSection.PageSetup = document.DefaultPageSetup.Clone();
            var (tableWidth, _) = GetEffectiveContentSizes(
                document.LastSection.PageSetup);

            var column = table.AddColumn(tableWidth * 0.33);
            column.Format.Alignment = ParagraphAlignment.Center;

            table.AddColumn(tableWidth * 0.66);

            var row = table.AddRow();
            row.Shading.Color = Colors.PaleGoldenrod;
            row.HeadingFormat = true;
            var cell = row.Cells[0];
            cell.AddParagraph("Itemus");
            cell = row.Cells[1];
            cell.AddParagraph("Descriptum");

            row = table.AddRow();
            cell = row.Cells[0];
            cell.AddParagraph("1");
            cell = row.Cells[1];
            cell.AddParagraph("FillerText.ShortText");

            row = table.AddRow();
            cell = row.Cells[0];
            cell.AddParagraph("2");
            cell = row.Cells[1];
            cell.AddParagraph("FillerText.Text");

            table.SetEdge(
                0,
                0,
                2,
                3,
                Edge.Box,
                BorderStyle.Single,
                0.75,
                Colors.Black);

            document.LastSection.Add(table);

            return ToPdfStream(
                document,
                model);
        }

        #endregion Get PDFs stuff
    }
}
