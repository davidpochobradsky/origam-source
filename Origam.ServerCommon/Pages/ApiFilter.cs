﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Origam.DA;
using Origam.DA.Service;
using Origam.Rule;
using Origam.Schema.EntityModel;
using Origam.Schema.GuiModel;
using Origam.Schema.WorkflowModel;
using Origam.Server;
using Origam.Workbench.Services;

namespace Origam.ServerCommon.Pages
{
    public class ApiFilter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region IHttpModule Members

        public void Dispose()
        {

        }

        public void MapRequestHandler(IHttpContext context)
        {
            if (context.Request.AppRelativeCurrentExecutionFilePath.StartsWith("~/assets/")) return;

            string mimeType = context.Request.ContentType.Split(";".ToCharArray())[0];
            if (mimeType == "application/x-amf") return;

            string resultContentType = "text";

            if (Analytics.Instance.IsAnalyticsEnabled)
            {
                Analytics.Instance.SetProperty("ContentType", mimeType);
//                Analytics.Instance.SetProperty("HttpMethod", context.Request.HttpMethod);
//                Analytics.Instance.SetProperty("RawUrl", context.Request.RawUrl);
//                Analytics.Instance.SetProperty("Url", context.Request.Url);
//                Analytics.Instance.SetProperty("UrlReferrer", context.Request.UrlReferrer);
//                Analytics.Instance.SetProperty("UserAgent", context.Request.UserAgent);
//                Analytics.Instance.SetProperty("BrowserName", context.Request.Browser.Browser);
//                Analytics.Instance.SetProperty("BrowserVersion", context.Request.Browser.Version);
//                Analytics.Instance.SetProperty("UserHostAddress", context.Request.UserHostAddress);
//                Analytics.Instance.SetProperty("UserHostName", context.Request.UserHostName);
//                Analytics.Instance.SetProperty("UserLanguages", context.Request.UserLanguages);
//                foreach (string key in context.Request.Params.Keys)
//                {
//                    Analytics.Instance.SetProperty("Parameter_" + key, context.Request.Params[key]);
//                }
            }

            try
            {
                Dictionary<string, string> urlParameters;
                AbstractPage page = ResolvePage(context, out urlParameters);

                if (page != null)
                {
                    if (Analytics.Instance.IsAnalyticsEnabled)
                    {
                        Analytics.Instance.SetProperty("OrigamPageId", page.Id);
                        Analytics.Instance.SetProperty("OrigamPageName", page.Name);
                        foreach (KeyValuePair<string, string> pair in urlParameters)
                        {
                            Analytics.Instance.SetProperty("Parameter_" + pair.Key, pair.Value);
                        }

                        Analytics.Instance.Log("PAGE_ACCESS");
                    }

                    if (page.MimeType != "?")
                    {
                        if (page.MimeType == "text/calendar")
                        {
                            context.Response.BufferOutput = false;
                        }
                        context.Response.ContentType = page.MimeType;
                        // set result content type for exception handling
                        // so the exception is formatted as expected, e.g. in json
                        resultContentType = page.MimeType;
                    }
                    if (page.CacheMaxAge == null)
                    {
                        context.Response.CacheSetMaxAge(new TimeSpan(0));
                    }
                    else
                    {
                        IParameterService parameterService = ServiceManager.Services.GetService(
                            typeof(IParameterService)) as IParameterService;
                        double maxAge = Convert.ToDouble(parameterService.GetParameterValue(
                            page.CacheMaxAgeDataConstantId));
                        context.Response.CacheSetMaxAge(TimeSpan.FromSeconds(maxAge));
                    }
                    context.Response.ContentEncoding = Encoding.UTF8;
                    Dictionary<string, object> mappedParameters = MapParameters(
                        context, urlParameters, page, mimeType);
                    IPageRequestHandler handler = HandlePage(page);
                    handler.Execute(page, mappedParameters, context.Request, context.Response);
                    //context.Response.Flush();
                    context.Response.End();
                }
            }
            catch (ThreadAbortException)
            {
            }
            catch (Exception ex)
            {
                if (log.IsErrorEnabled) log.Error(
                    string.Format("Error occured ({0}) for request: {1}: {2}",
                        ex.GetType().ToString(),
                        context.Request.AbsoluteUri,
                        ex.Message)
                    , ex);
                if (log.IsDebugEnabled) log.DebugFormat("Result Content Type: {0}", resultContentType);

                string message;
                if (resultContentType == "application/json")
                {
                    context.Response.TrySkipIisCustomErrors = true;

                    if (ex is RuleException)
                    {
                        message = String.Format("{{\"Message\" : {0}, \"RuleResult\" : {1}}}",
                            JsonConvert.SerializeObject(ex.Message),
                            JsonConvert.SerializeObject(((RuleException)ex).RuleResult));
                    }
                    else
                    {
                        message = JsonConvert.SerializeObject(ex);
                    }
                }
                else
                {
                    message = ex.Message + ex.StackTrace;
                }

                context.Response.StatusCode = 400;

                context.Response.ContentEncoding = Encoding.UTF8;
                context.Response.ContentType = resultContentType;
                context.Response.Clear();
                // context.Response.StatusDescription = ex.Message;
                context.Response.Write(message);
                //context.Response.Flush();
                context.Response.End();
            }
        }

        private static IPageRequestHandler HandlePage(AbstractPage page)
        {
            IPageRequestHandler handler;

            if (page is XsltDataPage)
            {
                handler = new XsltPageRequestHandler();
            }
            else if (page is WorkflowPage)
            {
                handler = new WorkflowPageRequestHandler();
            }
            else if (page is FileDownloadPage)
            {
                handler = new FileDownloadPageRequestHandler();
            }
            else if (page is ReportPage)
            {
                handler = new ReportPageRequestHandler();
            }
            else
            {
                throw new ArgumentOutOfRangeException("page", page, Resources.ErrorUnknownPageType);
            }
            return handler;
        }

        private static Dictionary<string, object> MapParameters(IHttpContext context, Dictionary<string, string> urlParameters, AbstractPage page, string requestMimeType)
        {
            IParameterService parameterService = ServiceManager.Services.GetService(typeof(IParameterService)) as IParameterService;
            Dictionary<string, object> mappedParameters = new Dictionary<string, object>();

            // add url and content parts mapped to parameters

            // firstly process normal parameters and then after that process
            // process content parameters, because filling a parameter from content
            // to datastructure could make a use of other parameters while applying
            // dynamic defaults to newly created dataset.
            List<PageParameterMapping> contentParameters = new List<PageParameterMapping>();
            foreach (PageParameterMapping ppm in page.ChildItemsByType(PageParameterMapping.ItemTypeConst))
            {
                PageParameterFileMapping fileMapping = ppm as PageParameterFileMapping;

                if (fileMapping != null)
                {
                    // files
                    MapFileToParameter(context, mappedParameters, ppm, fileMapping);

                }
                else if (ppm.MappedParameter == null || ppm.MappedParameter == "")
                {
                    contentParameters.Add(ppm);
                }
                else
                {
                    MapOtherParameters(context, urlParameters, parameterService, mappedParameters, ppm, requestMimeType);
                }
            }
            // now process content parameters
            foreach (PageParameterMapping ppm in contentParameters)
            {
                MapContentToParameter(context, page, requestMimeType, mappedParameters, ppm);
            }

            string pageSize = context.Request.Params["_pageSize"];
            string pageNumber = context.Request.Params["_pageNumber"];
            if (pageSize != null)
            {
                mappedParameters.Add("_pageSize", pageSize);
            }
            if (pageNumber != null)
            {
                mappedParameters.Add("_pageNumber", pageNumber);
            }
            return mappedParameters;
        }

        private static void MapOtherParameters(IHttpContext context, Dictionary<string, string> urlParameters,
            IParameterService parameterService, Dictionary<string, object> mappedParameters, PageParameterMapping ppm,
            string requestMimeType)
        {
            string paramValue = null;

            // URL parameter
            if (urlParameters.ContainsKey(ppm.MappedParameter))
            {
                paramValue = urlParameters[ppm.MappedParameter];
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("Mapping URL parameter {0}, value: {1}.", ppm.Name, paramValue);
                }
            }
            else
            // POST parameters
            {
                paramValue = context.Request.Params[ppm.MappedParameter];

                if (paramValue != null)
                {
                    if (log.IsDebugEnabled)
                    {
                        log.DebugFormat("Mapping POST parameter {0}, value: {1}.", ppm.Name, paramValue);
                    }
                }
            }

            // if empty, set default value
            if (paramValue == null && ppm.DefaultValue != null)
            {
                paramValue = (string)parameterService.GetParameterValue(ppm.DataConstantId, Origam.Schema.OrigamDataType.String);
            }

            if (paramValue != null && ppm.IsList)
            {
                // we convert csv to proper arrays
                ArrayList list = new ArrayList();
                string separator = ",";

                if (ppm.ListSeparator != null)
                {
                    separator = (string)parameterService.GetParameterValue(ppm.SeparatorDataConstantId, Origam.Schema.OrigamDataType.String);
                }

                list.AddRange(paramValue.Split(separator.ToCharArray()));

                mappedParameters.Add(ppm.Name, list);
            }
            else if (paramValue != null)
            {
                // simple data types
                mappedParameters.Add(ppm.Name, paramValue);
            }

            string systemParameterValue = null;
            switch (ppm.MappedParameter)
            {
                case "Server_ContentType":
                    systemParameterValue = requestMimeType;
                    break;
                case "Server_HttpMethod":
                    systemParameterValue = context.Request.HttpMethod;
                    break;
                case "Server_RawUrl":
                    systemParameterValue = context.Request.RawUrl;
                    break;
                case "Server_Url":
                    systemParameterValue = context.Request.Url.ToString();
                    break;
                case "Server_UrlReferrer":
                    systemParameterValue = context.Request.UrlReferrer.ToString();
                    break;
                case "Server_UserAgent":
                    systemParameterValue = context.Request.UserAgent;
                    break;
                case "Server_BrowserName":
                    systemParameterValue = context.Request.Browser;
                    break;
                case "Server_BrowserVersion":
                    systemParameterValue = context.Request.BrowserVersion;
                    break;
                case "Server_UserHostAddress":
                    systemParameterValue = context.Request.UserHostAddress;
                    break;
                case "Server_UserHostName":
                    systemParameterValue = context.Request.UserHostName;
                    break;
                case "Server_UserLanguages":
                    systemParameterValue = string.Join(",", context.Request.UserLanguages);
                    break;
            }

            if (systemParameterValue != null)
            {
                mappedParameters.Add(ppm.Name, systemParameterValue);
            }
        }

        private static void MapFileToParameter(IHttpContext context, Dictionary<string, object> mappedParameters, PageParameterMapping ppm, PageParameterFileMapping fileMapping)
        {
            HttpPostedFile file = context.Request.FilesGet(ppm.MappedParameter);

            if (file != null)
            {
                if (log.IsDebugEnabled)
                {
                    log.DebugFormat("Mapping File Parameter {0}, FileInfo: {1}", fileMapping.Name, fileMapping.FileInfoType);
                }

                switch (fileMapping.FileInfoType)
                {
                    case PageParameterFileInfo.ContentType:
                        mappedParameters.Add(ppm.Name, file.ContentType);
                        break;
                    case PageParameterFileInfo.FileContent:
                        byte[] fileBytes = GetFileBytes(fileMapping, file);

                        if (fileBytes != null)
                        {
                            mappedParameters.Add(ppm.Name, fileBytes);
                        }
                        break;
                    case PageParameterFileInfo.FileName:
                        mappedParameters.Add(ppm.Name, file.FileName);
                        break;
                    case PageParameterFileInfo.FileSize:
                        mappedParameters.Add(ppm.Name, file.ContentLength);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("FileInfoType", fileMapping.FileInfoType, "Unknown Type");
                }
            }
        }

        private static void MapContentToParameter(IHttpContext context,
                AbstractPage page, string requestMimeType,
                Dictionary<string, object> mappedParameters,
                PageParameterMapping ppm)
        {
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Mapping parameter {0}, Request content type: {1}", ppm.Name, requestMimeType);
            }

            System.IO.StreamReader reader = new System.IO.StreamReader(context.Request.InputStream, context.Request.ContentEncoding);
            IXmlContainer doc = new XmlContainer();
            DataSet data = null;
            WorkflowPage wfPage = page as WorkflowPage;
            XsltDataPage dataPage = page as XsltDataPage;
            if (dataPage != null && dataPage.DataStructure != null && context.Request.HttpMethod == "PUT")
            {
                GetEmptyData(ref doc, ref data, dataPage.DataStructure, mappedParameters, dataPage.DefaultSet);
                if (dataPage.DisableConstraintForInputValidation)
                {
                    data.EnforceConstraints = false;
                }
            }
            else if (wfPage != null)
            {
                ContextStore ctx = wfPage.Workflow.GetChildByName(ppm.Name, ContextStore.ItemTypeConst) as ContextStore;
                if (ctx == null)
                {
                    throw new ArgumentException(String.Format("Couldn't find a context store with " +
                        "the name `{0}' in a workflow `{1}' ({2})'.", ppm.Name, wfPage.Name, wfPage.Id));
                }
                DataStructure ds = ctx.Structure as DataStructure;
                if (ds != null)
                {
                    GetEmptyData(ref doc, ref data, ds, mappedParameters, ctx.DefaultSet);
                    if (ctx.DisableConstraints || wfPage.DisableConstraintForInputValidation)
                    {
                        data.EnforceConstraints = false;
                    }
                }
            }

            if (context.Request.ContentLength != 0)
            {
                // empty parameter name = deserialize body depending on the content-type
                try
                {
                    switch (requestMimeType)
                    {
                        case "text/xml":
                            doc.Xml.Load(reader);
                            break;

                        case "application/json":
                            JsonSerializer js = new JsonSerializer();
                            string body = reader.ReadToEnd();
                            // DataSet ds = JsonConvert.DeserializeObject<DataSet>(body);
                            XmlDocument xd = new XmlDocument();
                            // deserialize from JSON to XML
                            xd = (XmlDocument)JsonConvert.DeserializeXmlNode(body, "ROOT");
                            if (log.IsDebugEnabled)
                            {
                                log.Debug("Intermediate JSON deserialized XML:");
                                log.Debug(xd.OuterXml);
                            }
                            // remove any empty elements because empty guids and dates would
                            // result in errors and empty strings should always be converted
                            // to nulls anyway
                            RemoveEmptyNodes(ref xd);

                            if (log.IsDebugEnabled)
                            {
                                log.Debug("Deserialized XML after removing empty elements:");
                                log.Debug(xd.OuterXml);
                            }
                            if (data == null)
                            {
                                doc = new XmlContainer(xd);
                            }
                            else
                            {
                                foreach (DataTable table in data.Tables)
                                {
                                    foreach (DataColumn col in table.Columns)
                                    {
                                        if (col.DataType == typeof(DateTime))
                                        {
                                            col.DateTimeMode = DataSetDateTime.Local;
                                        }
                                    }
                                }
                                // ignore-schema because the source might not contain some elements (nulls)
                                // and it would try to remove these columns from the target dataset
                                data.ReadXml(new XmlNodeReader(xd), XmlReadMode.IgnoreSchema);
                            }
                            break;

                        default:
                            throw new ArgumentOutOfRangeException("ContentType", requestMimeType, "Unknown content type. Use text/xml or application/json.");
                    }
                }
                catch (ConstraintException)
                {
                    // make the exception far more verbose
                    throw new ConstraintException(DatasetTools.GetDatasetErrors(data));
                }
            }

            mappedParameters.Add(ppm.Name, doc);
            if (log.IsDebugEnabled)
            {
                log.Debug("Result XML:");
                log.Debug(doc.Xml.OuterXml);
            }
        }

        private static void GetEmptyData(ref IXmlContainer doc,
                ref DataSet data, DataStructure ds,
                Dictionary<string, object> mappedParameters)
        {
            GetEmptyData(ref doc, ref data, ds, mappedParameters, null);
        }

        private static void GetEmptyData(ref IXmlContainer doc,
                ref DataSet data, DataStructure ds,
                Dictionary<string, object> mappedParameters,
                DataStructureDefaultSet defaultSet)
        {
            DatasetGenerator dsg = new DatasetGenerator(true);
            data = dsg.CreateDataSet(ds, defaultSet);
            DatasetGenerator.ApplyDynamicDefaults(data, mappedParameters);

            doc = DataDocumentFactory.New(data);

            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Mapping content to data structure: {0}", ds.Path);
            }
        }

        public static void RemoveEmptyNodes(ref XmlDocument doc)
        {
            XmlNodeList nodes = doc.SelectNodes("//*");

            if (log.IsDebugEnabled)
            {
                foreach (XmlNode node in nodes)
                {
                    log.DebugFormat("Node: {0} Value: {1} HasChildNodes: {2}", node.Name, node.Value == null ? "<null>" : node.Value, node.HasChildNodes);
                }
            }

            foreach (XmlNode node in nodes)
            {
                XmlElement xe = node as XmlElement;

                if ((xe != null && xe.IsEmpty) ||
                    (
                        (!node.HasChildNodes || (node.ChildNodes.Count == 1 && node.ChildNodes[0].Name == "#text" && node.ChildNodes[0].Value == ""))
                        && (node.Value == null || node.InnerText == "") &&
                        ((node.Attributes == null) || (node.Attributes.Count == 0)))
                    )
                {
                    if (log.IsDebugEnabled)
                    {
                        log.DebugFormat("Removing empty node: {0}", node.Name);
                    }

                    node.ParentNode.RemoveChild(node);
                }
            }
        }

        private static byte[] GetFileBytes(PageParameterFileMapping fileMapping, HttpPostedFile file)
        {
            byte[] fileBytes = null;

            if (fileMapping.ThumbnailHeight == 0 && fileMapping.ThumbnailWidth == 0)
            {
                // get the original file
                fileBytes = StreamTools.ReadToEnd(file.InputStream);
            }
            else
            {
                // get a thumbnail
                Image img = null;

                try
                {
                    img = Image.FromStream(file.InputStream);
                }
                catch
                {
                    // file is not an image, we return null
                }

                if (img != null)
                {
                    try
                    {
                        fileBytes = BlobUploadHandler.FixedSizeBytes(img, fileMapping.ThumbnailWidth, fileMapping.ThumbnailHeight);
                    }
                    finally
                    {
                        if (img != null) img.Dispose();
                    }
                }
            }
            return fileBytes;
        }

        private static AbstractPage ResolvePage(IHttpContext context, out Dictionary<string, string> urlParameters)
        {
            urlParameters = new Dictionary<string, string>();

            string path = context.Request.AppRelativeCurrentExecutionFilePath;
            if (log.IsDebugEnabled)
            {
                log.DebugFormat("Resolving page {0}.", path);
            }

            IOrigamAuthorizationProvider auth = SecurityManager.GetAuthorizationProvider();
            SchemaService ss = ServiceManager.Services.GetService(typeof(SchemaService)) as SchemaService;
            PagesSchemaItemProvider pages = ss.GetProvider(typeof(PagesSchemaItemProvider)) as PagesSchemaItemProvider;
            string[] requestPath = path.Split(new string[] { "/" }, StringSplitOptions.None);

            foreach (AbstractPage page in pages.ChildItems)
            {
                urlParameters.Clear();

                if (IsPageValidByVerb(page, context))
                {
                    bool invalidRequest = false;

                    string[] pagePath = page.Url.Split(new string[] { "/" }, StringSplitOptions.None);

                    if (requestPath.Length - 1 == pagePath.Length)
                    {
                        for (int i = 0; i < pagePath.Length; i++)
                        {
                            string pathPart = pagePath[i];

                            // parameter
                            if (pathPart.StartsWith("{"))
                            {
                                if (requestPath.Length > i + 1)
                                {
                                    string paramValue = requestPath[i + 1];
                                    if (paramValue.EndsWith(".aspx") && i == requestPath.Length - 2)
                                    {
                                        // remove .aspx from the end of the string
                                        paramValue = paramValue.Substring(0, paramValue.Length - 5);
                                    }

                                    string paramName;

                                    if (pathPart.EndsWith(".aspx"))
                                    {
                                        paramName = pathPart.Substring(1, pathPart.Length - 7);
                                    }
                                    else
                                    {
                                        paramName = pathPart.Substring(1, pathPart.Length - 2);
                                    }

                                    //if (paramName == "action")
                                    //{
                                    //    invalidRequest = true;
                                    //    break;
                                    //}

                                    urlParameters.Add(paramName, paramValue);
                                }
                            }
                            // path
                            else
                            {
                                string paramValue = requestPath[i + 1];
                                if (paramValue.EndsWith(".aspx") && i == requestPath.Length - 2)
                                {
                                    // remove .aspx from the end of the string
                                    paramValue = paramValue.Substring(0, paramValue.Length - 5);
                                }

                                if (pathPart != paramValue)
                                {
                                    // this is not the right page, let's try another
                                    invalidRequest = true;
                                    break;
                                }
                            }
                        }

                        if (!invalidRequest)
                        {
                            if (IsPageValidBySecurity(auth, page))
                            {
                                return page;
                            }
                        }
                    }
                }
            }
            return null;
        }

        private static bool IsPageValidByVerb(AbstractPage page, IHttpContext context)
        {
            switch (context.Request.HttpMethod)
            {
                case "PUT":
                    return page.AllowPUT;
                case "DELETE":
                    return page.AllowDELETE;
                case "OPTIONS":
                    return false;
                default:
                    return true;
            }
        }

        internal static bool IsPageValidBySecurity(IOrigamAuthorizationProvider auth, AbstractPage page)
        {
            return auth.Authorize(SecurityManager.CurrentPrincipal, page.Roles);
        }
        #endregion
    }

    public class HttpPostedFile
    {
        public object ContentType { get; set; }
        public object FileName { get; set; }
        public int ContentLength { get; set; }
        public Stream InputStream { get; set; }
    }

    public interface IHttpContext
    {
        IResponse Response { get; }
        IRequest Request { get; }
    }

    public interface IRequest
    {
        string AppRelativeCurrentExecutionFilePath { get; }
        string ContentType { get;  }
        object AbsoluteUri { get;  }
        NameValueCollection Params { get;  }
        Stream InputStream { get;  }
        string HttpMethod { get;  }
        string RawUrl { get;  }
        object Url { get;  }
        object UrlReferrer { get; }
        string UserAgent { get;  }
        string Browser { get;  }
        string BrowserVersion { get;  }
        string UserHostAddress { get;  }
        string UserHostName { get;}
        IEnumerable<string> UserLanguages { get; }
        bool ContentEncoding { get;  }
        int ContentLength { get;  }
        IEnumerable<DictionaryEntry> BrowserCapabilities { get;  }
        string UrlReferrerAbsolutePath { get;  }
        HttpPostedFile FilesGet(string name);
    }

    public interface IResponse
    {
        bool BufferOutput { get; set; }
        string ContentType { get; set; }
        Encoding ContentEncoding { get; set; }
        bool TrySkipIisCustomErrors { get; set; }
        int StatusCode { get; set; }
        TextWriter Output { get; set; }
        void CacheSetMaxAge(TimeSpan timeSpan);
        void End();
        void Clear();
        void Write(string message);
        void AddHeader(string name, string value);
        void BinaryWrite(byte[] bytes);
        void Redirect(string requestUrlReferrerAbsolutePath);
        void OutputStreamWrite(byte[] buffer, int offset, int count);
    }
}



