﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DbUpdates.Database
{
    interface IDbUpdateRunner
    {
        Task RunUpdateAsync(string updateContent);
    }
}
