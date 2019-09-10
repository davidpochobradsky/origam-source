﻿#region license
/*
Copyright 2005 - 2019 Advantage Solutions, s. r. o.

This file is part of ORIGAM (http://www.origam.org).

ORIGAM is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

ORIGAM is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with ORIGAM. If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Origam.BI;
using Origam.Schema;
using Origam.Schema.GuiModel;
using Origam.Server;
using Origam.Workbench.Services;
using Origam.Workbench.Services.CoreServices;

namespace Origam.ServerCore.Controller
{
    [Authorize]
    [ApiController]
    [Route("internalApi/[controller]")]
    public class ReportController : AbstractController
    {
        private readonly SessionObjects sessionObjects;
        private readonly IStringLocalizer<SharedResources> localizer;
        private readonly CoreHttpTools httpTools = new CoreHttpTools();
        public ReportController(
            SessionObjects sessionObjects, 
            IStringLocalizer<SharedResources> localizer,
            ILogger<AbstractController> log) : base(log)
        {
            this.sessionObjects = sessionObjects;
            this.localizer = localizer;
        }

        [HttpGet("{reportRequestId:guid}")]
        public IActionResult Get(Guid reportRequestId)
        {
            try
            {
                ReportRequest reportRequest = sessionObjects.SessionManager
                    .GetReportRequest(reportRequestId);
                if(reportRequest == null)
                {
                    return NotFound(localizer["ErrorReportNotAvailable"]);
                }
                else
                {
                    reportRequest.TimesRequested++;
                    // get report model data
                    IPersistenceService persistenceService = ServiceManager
                        .Services.GetService<IPersistenceService>();
                    AbstractReport report 
                        = persistenceService.SchemaProvider.RetrieveInstance(
                        typeof(AbstractReport), 
                        new ModelElementKey(new Guid(reportRequest.ReportId))) 
                        as AbstractReport;
                    if(report is WebReport webReport)
                    {
                        return HandleWebReport(reportRequest, webReport);
                    }
                    else if(report is FileSystemReport fileSystemReport)
                    {
                        return HandleFileSystemReport(
                            reportRequest, fileSystemReport);
                    }
                    else 
                    {
                        return HandleReport(reportRequest, report.Name);
                    }
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }
            finally
            {
                RemoveRequest(reportRequestId);
            }
        }
        private IActionResult HandleReport(
            ReportRequest reportRequest, string reportName)
        {
            byte[] report = ReportService.GetReport(
                new Guid(reportRequest.ReportId),
                null,
                reportRequest.DataReportExportFormatType.GetString(),
                reportRequest.Parameters,
                null);
            Response.Headers.Add(
                HeaderNames.ContentDisposition,
                "filename=\"" + reportName + "."
                + reportRequest.DataReportExportFormatType.GetExtension() 
                + "\"");
            return File(report, 
                reportRequest.DataReportExportFormatType.GetContentType());
        }
        private IActionResult HandleWebReport(
            ReportRequest reportRequest, 
            WebReport webReport)
        {
            ReportHelper.PopulateDefaultValues(
                webReport, reportRequest.Parameters);
            string url = HttpTools.BuildUrl(
                webReport.Url, reportRequest.Parameters, 
                webReport.ForceExternalUrl,
                webReport.ExternalUrlScheme, webReport.IsUrlEscaped);
            return Redirect(url);
        }
        private IActionResult HandleFileSystemReport(
            ReportRequest reportRequest, 
            FileSystemReport report)
        {
            ReportHelper.PopulateDefaultValues(
                report, reportRequest.Parameters);
            string filePath = BuildFileSystemReportFilePath(
                report.ReportPath, reportRequest.Parameters);
            string mimeType = HttpTools.GetMimeType(filePath);
            Response.Headers.Add(
                HeaderNames.ContentDisposition,
                httpTools.GetFileDisposition(
                    new CoreRequestWrapper(Request), 
                    Path.GetFileName(filePath)));
            using (Stream stream = new FileStream(filePath, FileMode.Open))
            {
                return File(stream, mimeType);
            }
        }

        private string BuildFileSystemReportFilePath(
            string filePath, Hashtable parameters)
        {
            foreach (DictionaryEntry entry in parameters)
            {
                string sKey = entry.Key.ToString();
                string sValue = null;
                if (entry.Value != null)
                {
                    sValue = entry.Value.ToString();
                }
                string replacement = "{" + sKey + "}";
                if (filePath.IndexOf(replacement) > -1)
                {
                    if (sValue == null)
                    {
                        throw new Exception(
                            localizer["FilePathPartParameterNull"]);
                    }
                    filePath = filePath.Replace(replacement, sValue);
                }
            }
            return filePath;
        }
        private void RemoveRequest(Guid reportRequestId)
        {
            ReportRequest reportRequest = sessionObjects.SessionManager
                .GetReportRequest(reportRequestId);
            if((reportRequest == null) 
            || (reportRequest.TimesRequested >= 2)
            || (Request.Headers.ContainsKey(HeaderNames.UserAgent)
            && (Request.Headers[HeaderNames.UserAgent].ToString()
            .IndexOf("Edge") == -1)))
            {
                sessionObjects.SessionManager.RemoveReportRequest(
                    reportRequestId);
            }
        }
    }
}