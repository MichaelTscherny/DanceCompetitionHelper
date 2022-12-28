﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DanceCompetitionHelper.Database.Config
{
    public class SqLiteDbConfig : IDbConfig
    {
        public string SqLiteDbFile { get; init; } = default!;
    }
}
