﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Services
{
    public interface IReportService
    {
        string RenderReportHtml(string folder, object model);
    }
}
