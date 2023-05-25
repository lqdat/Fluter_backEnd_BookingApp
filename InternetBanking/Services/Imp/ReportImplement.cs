using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace BookingApp.Services.Imp
{
    public class ReportImplement : IReportService
    {
        public string RenderReportHtml(string folder, object model)
        {
            string fileIndex = HttpContext.Current.Server.MapPath("~/template/html/" + folder + "/index.cshtml");
            string fileDetail = HttpContext.Current.Server.MapPath("~/template/html/" + folder + "/detail.cshtml");
            string rs = "";
            if (File.Exists(fileIndex))
            {
                var templateDetail = Regex.Replace(File.ReadAllText(fileDetail, System.Text.Encoding.UTF8), @"[\n\t\r]+", "");
                string templateHtml;
                templateHtml = templateDetail;
                if (Engine.Razor.IsTemplateCached(folder, null))
                {
                    rs = Engine.Razor.Run(folder, null, model);
                }
                else
                    rs = Engine.Razor.RunCompile(templateHtml, folder, null, model);
            }

            return rs;
        }
        
    }
}