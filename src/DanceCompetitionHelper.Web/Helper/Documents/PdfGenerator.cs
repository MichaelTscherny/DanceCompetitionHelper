using DanceCompetitionHelper.Database.DisplayInfo;
using DanceCompetitionHelper.Database.Extensions;
using DanceCompetitionHelper.Database.Tables;
using DanceCompetitionHelper.Web.Extensions;
using DanceCompetitionHelper.Web.Models.Pdfs;

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;

using PdfSharp.Drawing;
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
            // #if CORE
            // Core build does not use Windows fonts, so set a FontResolver that handles the fonts our samples need.
            GlobalFontSettings.FontResolver = new UserFontResolver();
            // #endif
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
            document.LastSection.PageSetup.Orientation = model.Orientation;

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
                    PageLayout = model.PdfPageLayout,
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
            var dispCunksSize = GetMaxColumnsCount(
                document.LastSection.PageSetup);
            var helpAllDisplayInfos = ViewModelExtensions
                .ExtractDisplayInfo(
                    multipleStarters)
                .ToList();
            var allDisplayInfos = helpAllDisplayInfos
                .Chunk(
                    dispCunksSize)
                .ToList();

            foreach (var displayInfos in allDisplayInfos)
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
                table.AddColumn(
                    Unit.FromCentimeter(
                        6));

                foreach (var curDisplayInfo in displayInfos)
                {
                    table.AddColumn();
                }

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
                        helpAllDisplayInfos.Count));

                foreach (var curDisplayInfo in displayInfos)
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

                    foreach (var curDisplayInfo in displayInfos)
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

            // remove last section...
            document.Sections.RemoveObjectAt(
                document.Sections.Count - 1);

            // ----
            return ToPdfStream(
                document,
                model);
        }

        public Stream GetMultipleStartersGroupedClassesView(
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
                    "Group Classes for Multiple Starters",
                    multipleStarters.Count));
            SetDefaultPageSetup(
                document,
                model);

            // ----
            var dispCunksSize = GetMaxColumnsCount(
                document.LastSection.PageSetup);
            var (linkedClassIdsAndParticipants, allDisplayInfos) = ViewModelExtensions
                .ExtractMultipleStarterLinkedClasses(
                    multipleStarters);

            foreach (var (curClassIds, curMultipleStarters) in linkedClassIdsAndParticipants)
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
                    table.AddColumn(
                        Unit.FromCentimeter(
                            6));

                    foreach (var curDisplayInfo in curClassesByChunk)
                    {
                        table.AddColumn();
                    }

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
                            curClassIds.Count));

                    foreach (var curDisplayInfo in allDisplayInfos)
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

                        foreach (var curDisplayInfo in allDisplayInfos)
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
            var (helpIncludedClasses, maxDisplayClasses) = ViewModelExtensions
                .ExtractPossiblePromotionCompetitionClasses(
                    possiblePromotions);
            var displayInfoCompClasses = ViewModelExtensions.ExtractDisplayInfo(
                helpIncludedClasses);
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
                table.AddColumn(
                    Unit.FromCentimeter(
                        6));

                // -- prepare columns
                foreach (var curColumn in curClassesByClassId)
                {
                    table.AddColumn(
                        Unit.FromCentimeter(
                            3));
                }

                // -- fill header
                var curCellId = 0;
                var curRow = table.AddRow();
                curRow.HeadingFormat = true;
                curRow.Format.Font.Bold = true;
                curRow[curCellId].AddParagraph(
                    "Name");

                foreach (var curColumn in curClassesByClassId)
                {
                    curCellId++;
                    var classInfo = string.Empty;

                    foreach (var curClassId in displayInfoCompClassesIds)
                    {
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
                    }

                    curRow[curCellId].AddParagraph(
                        classInfo);
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

            // ----
            return ToPdfStream(
                document,
                model);
        }

        public Stream GetNumberCards(
            Competition? competition,
            List<Participant> possiblePromotions,
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

            // why?..
            curSection.PageSetup.LeftMargin = useMargin * 2; // Unit.Zero;

            // ----
            var table = new Table
            {
                Borders =
                {
                    Width = 0.75,
                    Visible = true, // false,
                },
                LeftPadding = useMargin,
            };

            // -- prepare the table...
            var tableColumn = table.AddColumn(
                useWidht);
            tableColumn.Format.Alignment = ParagraphAlignment.Center;

            var numberInfoHeight = Unit.FromCentimeter(1);
            var numberFitHeight = Unit.FromMillimeter(3);
            var useNumberFontSize = Unit.FromPoint(250);
            var useNumberInfoFontSize = Unit.FromPoint(8);

            var test = GlobalFontSettings.FontResolver;
            var test02 = GlobalFontSettings.DefaultFontEncoding;

            var testFont = new XFont(
                "Arial",
                useNumberFontSize,
                XFontStyleEx.Bold);

            // ------
            // first row...
            var firstNumber = table.AddRow();
            firstNumber[0].VerticalAlignment = VerticalAlignment.Center;
            firstNumber.Height = (useHeight / 2.0) - numberInfoHeight - numberFitHeight;
            firstNumber.HeightRule = RowHeightRule.Exactly;
            var firstNumberPara = firstNumber.Cells[0].AddParagraph(
                "1234");
            firstNumberPara.Format.Alignment = ParagraphAlignment.Center;
            firstNumberPara.Format.Font.Size = useNumberFontSize;

            // firstNumberPara.Format.Font.Name = "segoe ui";
            // firstNumberPara.Format.Font.Name = "Arial Narrow";
            // firstNumberPara.Format.Font.Name = "Noto Sans";
            // firstNumberPara.Format.Font.Name = "formiena";
            // firstNumberPara.Format.Font.Name = "fs-regulate";
            // firstNumberPara.Format.Font.Size = Unit.FromPoint(380); // useNumberFontSize;
            firstNumberPara.Format.Font.Bold = true;

            var firstNumberInfo = table.AddRow();
            firstNumberInfo[0].VerticalAlignment = VerticalAlignment.Center;
            firstNumberInfo.Height = numberInfoHeight;
            firstNumberInfo.HeightRule = RowHeightRule.Exactly;

            var firstNumberInfoPara = firstNumberInfo.Cells[0].AddParagraph(
                "Some number info");
            firstNumberInfoPara.Format.Font.Size = useNumberInfoFontSize;

            // ------
            // filler row...
            var filler = table.AddRow();
            filler.Height = Unit.FromMillimeter(3);
            filler.HeightRule = RowHeightRule.Exactly;

            // ------
            // second row...
            var secondNumber = table.AddRow();
            secondNumber[0].VerticalAlignment = VerticalAlignment.Center;
            secondNumber.Height = (useHeight / 2.0) - numberInfoHeight - numberFitHeight;
            secondNumber.HeightRule = RowHeightRule.Exactly;
            var secondNumberPara = secondNumber.Cells[0].AddParagraph(
                "5678");

            // secondNumberPara.Format.Borders.Distance = Unit.Zero;
            // secondNumberPara.Format.Borders.Left.Width = Unit.Zero;
            // secondNumberPara.Format.Font.Name = "segoe ui";
            secondNumberPara.Format.Font.Size = useNumberFontSize;
            secondNumberPara.Format.Font.Bold = true;

            var secondNumberInfo = table.AddRow();
            secondNumberInfo[0].VerticalAlignment = VerticalAlignment.Center;
            secondNumberInfo.Height = numberInfoHeight;
            secondNumberInfo.HeightRule = RowHeightRule.Exactly;
            var secondNumberInfoPara = secondNumberInfo.Cells[0].AddParagraph(
                "other number info");
            secondNumberInfoPara.Format.Font.Size = useNumberInfoFontSize;

            // ----
            curSection.Add(
                table);

            // ----
            return ToPdfStream(
                document,
                model);
        }

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
