using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceCompetitionHelper.Database.Test.Pocos
{
    internal class DanceCompetitionHelperDbContextPoco : IDisposable
    {
        private bool disposedValue;

        public DanceCompetitionHelperDbContext DanceCompetitionHelperDbContext { get; }
        public string DbFilePath { get; }

        public DanceCompetitionHelperDbContextPoco(
            DanceCompetitionHelperDbContext danceCompetitionHelperDbContext,
            string dbFilePath)
        {
            DanceCompetitionHelperDbContext = danceCompetitionHelperDbContext
                ?? throw new ArgumentNullException(
                    nameof(danceCompetitionHelperDbContext));

            DbFilePath = dbFilePath
                ?? throw new ArgumentNullException(
                    nameof(dbFilePath));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposedValue)
            {
                return;
            }

            disposedValue = true;

            DanceCompetitionHelperDbContext.Dispose();
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
