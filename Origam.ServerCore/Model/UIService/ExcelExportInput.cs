﻿using System.Collections.Generic;
using Origam.Server;

namespace Origam.ServerCore.Model.UIService
{
    public class ExcelExportInput
    {
        public string Entity { get; set; }

        public List<EntityExportField> Fields { get; set; }

        public List<string> RowIds { get; set; }

        public string SessionFormIdentifier { get; set; }
    }
}