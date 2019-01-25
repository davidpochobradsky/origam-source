﻿#region license
/*
Copyright 2005 - 2017 Advantage Solutions, s. r. o.

This file is part of ORIGAM.

ORIGAM is free software: you can redistribute it and/or modify
it under the terms of the GNU Affero General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

ORIGAM is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
GNU Affero General Public License for more details.

You should have received a copy of the GNU Affero General Public License
along with ORIGAM.  If not, see<http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;
using log4net;
using log4net.Core;

namespace Origam.Server
{
    public class Analytics
    {
        private static readonly ILog perfLog = LogManager.GetLogger(typeof(Analytics));
        public const string PropertyNamePrefix = "log4net_app_";

        #region Factory methods

        private static IAdaptivePropertyProvider Create(string propertyName, object propertyValue)
        {
            Type type = Type.GetType("Origam.Server.AdaptivePropertyProvider,Origam.Server");
            return type != null
                ? (IAdaptivePropertyProvider)Activator.CreateInstance(type, new object[] { propertyName, propertyValue })
                : new NullPropertyProvider();
        }

        public static void SetProperty(string propertyName, object value)
        {
            log4net.ThreadContext.Properties[propertyName] = Create(propertyName, value);
        }

        public static void Log(string message)
        {
            if (perfLog.IsInfoEnabled)
            {
                perfLog.Info(message);
            }
        }

        public static void Log(Type type, string message, IDictionary<string, string> properties)
        {
            if (perfLog.IsInfoEnabled)
            {
                LoggingEvent loggingEvent = new LoggingEvent(
                  type,
                  perfLog.Logger.Repository,
                  perfLog.Logger.Name,
                  Level.Info,
                  message,
                  null);

                foreach (KeyValuePair<string, string> item in properties)
                {
                    loggingEvent.Properties[item.Key] = item.Value;
                }
                perfLog.Logger.Log(loggingEvent);                
            }
        }

        public static bool IsAnalyticsEnabled
        {
            get
            {
                return perfLog.IsInfoEnabled;
            }
        }
        #endregion
    }

    class NullPropertyProvider : IAdaptivePropertyProvider
    {
    }
}