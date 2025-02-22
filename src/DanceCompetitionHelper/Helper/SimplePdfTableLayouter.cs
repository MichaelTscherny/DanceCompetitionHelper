using DanceCompetitionHelper.Extensions;

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;

namespace DanceCompetitionHelper.Helper
{
    public class SimplePdfTableLayouter
    {
        public Unit TableWidth { get; set; } = Unit.Empty;

        private readonly List<double> _columnWidthsPercent = new List<double>();
        public List<double> ColumnWidthsPercent => _columnWidthsPercent.ToList();

        private readonly List<Unit> _columnWidths = new List<Unit>();
        public List<Unit> ColumnWidths => _columnWidths.ToList();

        public SimplePdfTableLayouter(
            Unit? tableWidth = null)
        {
            if (tableWidth != null)
            {
                TableWidth = tableWidth.Value;
            }
        }

        public SimplePdfTableLayouter AddColumn(
            Unit columnWidth)
        {
            _columnWidths.Add(
                columnWidth /* ?? Unit.Empty */);
            _columnWidthsPercent.Add(
                0.0);

            return this;
        }

        public SimplePdfTableLayouter AddColumn(
            double columnWidhtPercent)
        {
            _columnWidths.Add(
                Unit.Empty);
            _columnWidthsPercent.Add(
                Math.Max(
                    0.0,
                    columnWidhtPercent));

            return this;
        }

        public SimplePdfTableLayouter AddColumns(
            int columnCount)
        {
            for (var idx = 0; idx < columnCount; idx++)
            {
                AddColumn(
                    Unit.Empty);
            }

            return this;
        }

        public SimplePdfTableLayouter AddColumns(
            int columnCount,
            Unit columnWidth)
        {
            for (var idx = 0; idx < columnCount; idx++)
            {
                AddColumn(
                    columnWidth);
            }

            return this;
        }

        public SimplePdfTableLayouter AddColumns(
            int columnCount,
            double columnWidhtPercent)
        {
            for (var idx = 0; idx < columnCount; idx++)
            {
                AddColumn(
                    columnWidhtPercent);
            }

            return this;
        }

        public void Validate()
        {
            if (_columnWidths.Count != _columnWidthsPercent.Count)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(_columnWidths),
                    string.Format(
                        "Columns Widths/Percentages '{0}'/'{1}' does not match",
                        _columnWidths.Count,
                        _columnWidthsPercent.Count));
            }

            if (_columnWidths.Count > 0
                && TableWidth.IsZeroOrEmpty())
            {
                throw new ArgumentOutOfRangeException(
                    nameof(TableWidth),
                    "No TableWidth defined!");
            }
        }

        public void DoLayout()
        {
            Validate();

            var countAuto = 0;
            var curAutoPercentLeft = 100.0;
            var lastAnyIndex = -1;

            for (var idx = 0; idx < _columnWidths.Count; idx++)
            {
                var curWidthPercent = _columnWidthsPercent[idx];

                var curWidth = _columnWidths[idx];
                var curWidhtEmpty = curWidth.IsZeroOrEmpty();

                if (curWidthPercent > 0.0
                    && curWidhtEmpty)
                {
                    _columnWidths[idx] = TableWidth * (curWidthPercent / 100.00);
                    curAutoPercentLeft -= curWidthPercent;
                    continue;
                }

                if (curWidthPercent <= 0.0
                    && curWidhtEmpty == false)
                {
                    // use "same" types
                    curWidth.ConvertType(
                        TableWidth.Type);

                    var curPercent = Math.Round(
                        curWidth.Value / TableWidth.Value,
                        2,
                        MidpointRounding.AwayFromZero) * 100.0;

                    _columnWidthsPercent[idx] = curPercent;
                    curAutoPercentLeft -= curPercent;

                    continue;
                }

                countAuto++;
                lastAnyIndex = idx;
            }

            if (_columnWidths.Count == 1
                && curAutoPercentLeft < 100.0)
            {
                _columnWidthsPercent[0] = 100.0;
                _columnWidths[0] = TableWidth;

                return;
            }

            // calc the rest...
            var usePercentLeft = Math.Round(
                Math.Max(
                    0.0,
                    curAutoPercentLeft / countAuto),
                2,
                MidpointRounding.AwayFromZero);
            var useWidthLeft = new Unit(
                Math.Round(
                    TableWidth.Value / 100.0 * usePercentLeft,
                    2,
                    MidpointRounding.AwayFromZero),
                TableWidth.Type);

            // paste calced values and calc "fixes"...
            var sumPercentages = 0.0;
            var sumWidths = Unit.Zero;
            sumWidths.ConvertType(
                TableWidth.Type);

            for (var idx = 0; idx < _columnWidths.Count; idx++)
            {
                var curWidthPercent = _columnWidthsPercent[idx];

                var curWidth = _columnWidths[idx];
                var curWidhtEmpty = curWidth.IsZeroOrEmpty();

                if (curWidthPercent <= 0.0
                    && curWidhtEmpty)
                {
                    _columnWidthsPercent[idx] = usePercentLeft;
                    _columnWidths[idx] = useWidthLeft;

                    sumPercentages += usePercentLeft;
                    sumWidths += useWidthLeft;
                }

                // use "same" types
                curWidth.ConvertType(
                    TableWidth.Type);

                sumPercentages += curWidthPercent;
                sumWidths += curWidth;
            }

            if (lastAnyIndex >= 0)
            {
                var diffPercentage = 100.0 - sumPercentages;
                var diffWidthLeft = new Unit(
                    Math.Round(
                        (TableWidth - sumWidths).Value,
                        2,
                        MidpointRounding.AwayFromZero),
                    TableWidth.Type);

                _columnWidthsPercent[lastAnyIndex] = Math.Round(
                    _columnWidthsPercent[lastAnyIndex] + diffPercentage,
                    2,
                    MidpointRounding.AwayFromZero);
                _columnWidths[lastAnyIndex] += diffWidthLeft;
            }
        }

        public void ApplyTo(
            Table table)
        {
            DoLayout();

            for (var idx = 0; idx < _columnWidths.Count; idx++)
            {
                table.AddColumn(
                    _columnWidths[idx]);
            }
        }
    }
}
