﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Origam.Security.Identity;
using Origam.Server;
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

        public UIServiceController(
            SessionObjects sessionObjects, 
            IServiceProvider serviceProvider)
        {
            this.sessionObjects = sessionObjects;
            IdentityServiceAgent.ServiceProvider = serviceProvider;
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
                    registerSession: true, 
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
    }
}
