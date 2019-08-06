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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Origam.Security.Identity;
using Origam.Server;
using Origam.ServerCore.Model.UIService;
using System;
using System.ComponentModel.DataAnnotations;

namespace Origam.ServerCore.Controller
{
    [Authorize]
    [ApiController]
    [Route("internalApi/[controller]")]
    public class UIServiceController : ControllerBase
    {
        private readonly SessionObjects sessionObjects;
        private readonly IStringLocalizer<SharedResources> localizer;

        public UIServiceController(
            SessionObjects sessionObjects, 
            IServiceProvider serviceProvider,
            IStringLocalizer<SharedResources> localizer)
        {
            this.sessionObjects = sessionObjects;
            IdentityServiceAgent.ServiceProvider = serviceProvider;
            this.localizer = localizer;
        }

        [HttpGet("[action]")]
        public IActionResult InitPortal([FromQuery][Required]string locale)
        {
            Analytics.Instance.Log("UI_INIT");
            //TODO: find out how to setup locale cookies and incorporate
            // locale resolver
            /*// set locale
            locale = locale.Replace("_", "-");
            Thread.CurrentThread.CurrentCulture = new CultureInfo(locale);
            // set locale to the cookie
            Response.Cookies.Append(
                ORIGAMLocaleResolver.ORIGAM_CURRENT_LOCALE, locale);*/
            try
            {
                //TODO: findout how to get request size limit
                return Ok(sessionObjects.UIService.InitPortal(4));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("[action]")]
        public IActionResult InitUI([FromBody]UIRequest request)
        {
            // registerSession is important for sessionless handling
            try
            {
                return Ok(sessionObjects.UIManager.InitUI(
                    request: request,
                    addChildSession: false,
                    parentSession: null,
                    basicUIService: sessionObjects.UIService));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("[action]")]
        public IActionResult DestroyUI(
            [FromQuery][Required]Guid sessionFormIdentifier)
        {
            try
            {
                sessionObjects.UIService.DestroyUI(sessionFormIdentifier);
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("[action]")]
        public IActionResult RefreshData(
            [FromQuery][Required]Guid sessionFormIdentifier)
        {
            try
            {
                return Ok(sessionObjects.UIService.RefreshData(
                    sessionFormIdentifier, localizer));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("[action]")]
        public IActionResult SaveDataQuery(
            [FromQuery][Required]Guid sessionFormIdentifier)
        {
            try
            {
                return Ok(sessionObjects.UIService.SaveDataQuery(
                    sessionFormIdentifier));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("[action]")]
        public IActionResult SaveData(
            [FromQuery][Required]Guid sessionFormIdentifier)
        {
            try
            {
                return Ok(sessionObjects.UIService.SaveData(
                    sessionFormIdentifier));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("[action]")]
        public IActionResult CreateObject([FromBody]CreateObjectData data)
        {
            try
            {
                return Ok(sessionObjects.UIService.CreateObject(data));
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
