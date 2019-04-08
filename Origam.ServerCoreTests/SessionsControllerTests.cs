#region license
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
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Origam.Extensions;
using Origam.Server;
using Origam.ServerCore;
using Origam.ServerCore.Controllers;
using Origam.ServerCore.Models;

namespace Origam.ServerCoreTests
{
    [TestFixture]
    class SessionsControllerTests : ControllerTests
    {
        private SessionsController sut;
        private Guid sessionId;
        private Guid rowId;


        public SessionsControllerTests()
        {
            sut = new SessionsController(sessionObjects);
        }

        [Test, Order(301)]
        public void ShouldCreateNewEmptySession()
        {
            var actionResult = sut.New(new NewSessionData
            {
                MenuId = new Guid("f38fdadb-4bba-4bb7-8184-c9109d5d40cd"),
                InitializeStructure = true
            });

            Assert.IsInstanceOf<OkObjectResult>(actionResult); // OkObjectResult means code 200 will be returned
            OkObjectResult okObjectResult = (OkObjectResult) actionResult;

            Assert.IsInstanceOf<Guid>(okObjectResult.Value);
            Guid newSessionId = (Guid) okObjectResult.Value;

            Assert.That(sessionObjects.SessionManager.HasFormSession(newSessionId));
            sessionId = newSessionId;
        }


        [Test, Order(302)]
        public void ShouldCreateNewRowInProducers()
        {
            var actionResult = sut.CreateRow(new NewRowData
            {
                SessionFormIdentifier = sessionId,
                Entity = "Producer",
                Values = new Dictionary<string, object>(),
                Parameters = new Dictionary<string, object>(),
                RequestingGridId = new Guid("36aa15e1-5fb8-4ca6-8804-e83ac09945d1")
            });
            Assert.IsInstanceOf<OkObjectResult>(actionResult);
            OkObjectResult okObjectResult = (OkObjectResult) actionResult;

            Assert.IsInstanceOf<ArrayList>(okObjectResult.Value);
            var values = (ArrayList) okObjectResult.Value;
            Assert.That(values, Has.Count.EqualTo(1));

            Assert.IsInstanceOf<ChangeInfo>(values[0]);
            var changeInfo = (ChangeInfo) values[0];

            Assert.IsInstanceOf<Guid>(changeInfo.ObjectId);
            var newRowId = (Guid) changeInfo.ObjectId;
            Assert.That(newRowId, Is.Not.EqualTo(Guid.Empty));
            rowId = newRowId;
        }


        [Test, Order(303)]
        public void ShouldModifyTheNewRow()
        {
            sut.UpdateRow(new UpdateRowData
            {
                SessionFormIdentifier = sessionId,
                Entity = "Producer",
                Id = rowId,
                Property = "Name",
                NewValue = "testName"
            });
            sut.UpdateRow(new UpdateRowData
            {
                SessionFormIdentifier = sessionId,
                Entity = "Producer",
                Id = rowId,
                Property = "NameAndAddress",
                NewValue = "testNameAndAddress"
            });
            var actionResult = sut.UpdateRow(new UpdateRowData
            {
                SessionFormIdentifier = sessionId,
                Entity = "Producer",
                Id = rowId,
                Property = "Email",
                NewValue = "test@test.test"
            });

            Assert.IsInstanceOf<OkObjectResult>(actionResult);
            OkObjectResult okObjectResult = (OkObjectResult) actionResult;

            Assert.IsInstanceOf<ArrayList>(okObjectResult.Value);
            var values = (ArrayList) okObjectResult.Value;
            Assert.That(values, Has.Count.EqualTo(1));

            Assert.IsInstanceOf<ChangeInfo>(values[0]);
            var changeInfo = (ChangeInfo) values[0];

            Assert.IsInstanceOf<ArrayList>(changeInfo.WrappedObject);
            var currentRowValues = (ArrayList) changeInfo.WrappedObject;

            Assert.That(currentRowValues[3], Is.EqualTo("testName"));
            Assert.That(currentRowValues[4], Is.EqualTo("test@test.test"));
            Assert.That(currentRowValues[5], Is.EqualTo("testNameAndAddress"));
        }

        [Test, Order(304)]
        public void ShouldSaveSession()
        {
            var actionResult = sut.Save(new SaveSessionData
            {
                SessionId = sessionId
            });
            Assert.IsInstanceOf<OkObjectResult>(actionResult);

            var session = (FormSessionStore)sessionObjects.SessionManager.GetSession(sessionId);

            // Saving the session should result in saving it's contained objects to database.
            // Data Controller will retrieve the saved object from the database so that it's
            // contents can be checked.  

            AssertObjectIsPersisted(
                menuId: session.MenuItem.Id, 
                dataStructureId: new Guid("93053357-745b-4a8c-91d2-d60389d0f22e"), 
                id: rowId, 
                columnNames: new string[] { "Id", "Name", "NameAndAddress", "Email" }, 
                expectedColumnValues: new object[] { rowId, "testName", "testNameAndAddress", "test@test.test" });
        }

        [Test, Order(305)]
        public void ShouldDeleteSession()
        {
            var actionResult = sut.Delete(new DeleteSessionData
            {
                SessionId = sessionId
            });
            Assert.IsInstanceOf<OkResult>(actionResult);
            Assert.IsFalse(sessionObjects.SessionManager.HasFormSession(sessionId));
        }
    }
}