﻿#region license
/*
Copyright 2005 - 2021 Advantage Solutions, s. r. o.

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
using System.Net;
using System.Text;
using System.Windows.Forms;
using Origam;
using Origam.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Origam.Licensing;


namespace OrigamArchitect
{
	public partial class LoginForm : Form
	{
		/*
		public RegisterForm RegisterForm
		{
			get; set;
		}
		*/
		public bool LoginSuccessfull
		{
			get; set;
		}
		public CookieCollection LoginCookies
		{
			get; set;
		}
		public LoginForm()
		{
			InitializeComponent();
			LoginSuccessfull = false;
			try
			{
				txtUsername.Text = Origam.Licensing.License.Load(
					OrigamLicenseHelper.GetLicenseFromRegistry()).Customer.Email;
			}
			catch { }
		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			StringBuilder err = new StringBuilder();
			if (string.IsNullOrEmpty(txtUsername.Text.Trim()))
			{
				err.Append(strings.LoginForm_UsernameIsMandatory);
				err.AppendLine();
			}
			if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
			{
				err.Append(strings.LoginForm_PasswordIsMandatory);
				err.AppendLine();
			}
			string errString = err.ToString();
			if (!string.IsNullOrEmpty(errString))
			{
				MessageBox.Show(errString,
					strings.LoginForm_Warning_Label,
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation);
				return;
			}

			Cursor.Current = Cursors.WaitCursor;
			JObject jobj = new JObject(
				new JProperty("username", txtUsername.Text.Trim())
				, new JProperty("password", txtPassword.Text.Trim())
			);

			string s = jobj.ToString();

			string output = null;
			try
			{
				using (WebResponse webResponse = HttpTools.GetResponse(
					url: string.Format("{0}AjaxLogin", frmMain.ORIGAM_COM_API_BASEURL),
					method: "POST",
					content: jobj.ToString(),
					contentType: "application/json",
					headers: new Hashtable{ { "Accept-Encoding", "gzip,deflate"} },
					authenticationType: null,
					userName: null,
					password: null,
					timeoutMs: 10000,
					cookies: null, 
					ignoreHTTPSErrors: frmMain.IgnoreHTTPSErrors))
				{
					HttpWebResponse httpWebResponse = webResponse as HttpWebResponse;
					output = HttpTools.ReadResponseTextRespectionContentEncoding(httpWebResponse);

					JObject jResult = (JObject)JsonConvert.DeserializeObject(output);
					if (httpWebResponse.StatusCode == HttpStatusCode.OK && (int)jResult["Status"] == 200)
					{

						// success
						LoginCookies = httpWebResponse.Cookies;
						DialogResult res = GetAndStoreLicense(LoginCookies);
						if (res == DialogResult.OK
							|| this.DialogResult == DialogResult.Abort)
						{
							this.DialogResult = res;
							this.Close();
						}
						return;
					}
					else
					{
						MessageBox.Show((string)jResult["Message"],
							strings.LoginForm_Unsuccessfull_Label,
							MessageBoxButtons.OK, MessageBoxIcon.Error);
						return;
					}
				}
			}
			catch (System.Net.WebException wex)
			{
				if (wex.Status == WebExceptionStatus.Timeout || wex.Status == WebExceptionStatus.ConnectFailure)
				{
					AsMessageBox.ShowError(null,
						string.Format(strings.RegisterAndLoginForm_TemporaryHttpError,
						wex.Message),
						strings.LoginForm_Error_Label,
						wex);
					return;
				}
				AsMessageBox.ShowError(null,
					strings.RegisterLoginForm_UnexpectedError_Message,
					strings.LoginForm_Error_Label, wex);
			}
		}

		private DialogResult GetAndStoreLicense(CookieCollection loginCookies)
		{
			StringBuilder queryStringSb = new StringBuilder();
			queryStringSb.AppendFormat("machineId={0}",
				MyUri.EscapeUriString(
					OrigamLicenseHelper.GetComputerUUID().ToString()
				)
			);

			string output = null;
			try
			{
				using (WebResponse webResponse = HttpTools.GetResponse(
					string.Format("{0}private/CreateClientLicense", frmMain.ORIGAM_COM_API_BASEURL),
				"POST", queryStringSb.ToString(), "application/x-www-form-urlencoded",
				new Hashtable()
				{ { "Accept-Encoding", "gzip,deflate"} }
				, null, null, null, 10000,
				loginCookies, frmMain.IgnoreHTTPSErrors))
				{
					HttpWebResponse httpWebResponse = webResponse as HttpWebResponse;
					output = HttpTools.ReadResponseTextRespectionContentEncoding(httpWebResponse);
					if (httpWebResponse.StatusCode == HttpStatusCode.OK)
					{
						// check license
						JObject jResult = (JObject)JsonConvert.DeserializeObject(output);
                        Origam.Licensing.License license = Origam.Licensing.License.Load(
							jResult["ROOT"]["Releases_ClientLicense_API"][0]["LicenseXmlString"].ToString());
						/* List<IValidationFailure> failures =
							license.Validate().Signature()
								.And().ExpirationDate()
							.AssertValidLicense().ToList();
							*/

						OrigamLicenseHelper.StoreLicenseToRegistry(license.ToString());
						return DialogResult.OK;
					}
				}
			}
			catch (System.Net.WebException wex)
			{
				if (wex.Status == WebExceptionStatus.Timeout || wex.Status == WebExceptionStatus.ConnectFailure)
				{
					AsMessageBox.ShowError(null,
						string.Format(strings.RegisterAndLoginForm_TemporaryHttpError,
						wex.Message),
						strings.LoginForm_ErrorGettingLicense_Label, wex);
					return DialogResult.Retry;
				}
				string errorInfo = null;
				using (HttpWebResponse httpWebResponse = wex.Response as HttpWebResponse)
				{
					if (httpWebResponse != null && httpWebResponse.StatusCode == HttpStatusCode.BadRequest)
					{
						errorInfo = HttpTools.ReadResponseTextRespectionContentEncoding(httpWebResponse);
						JObject jResult = (JObject)JsonConvert.DeserializeObject(errorInfo);
						AsMessageBox.ShowError(null,
							string.Format(
								strings.LoginForm_ErrorGettingLicense_Message,
								jResult["Message"].ToString()),
							strings.LoginForm_ErrorGettingLicense_Label, wex);
						return DialogResult.Abort;
					}
				}
				throw wex;
			}
			return DialogResult.Abort;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			//this.Close();	
			RegisterForm registerForm = new RegisterForm();
			DialogResult dr = registerForm.ShowDialog();
			if (dr == DialogResult.OK)
			{
				txtUsername.Text = registerForm.Email.ToString();
				txtPassword.Text = "";
			}
		}

		private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			

		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{			
			string url = string.Format("{0}Recover", frmMain.ORIGAM_COM_API_BASEURL);
			if (!string.IsNullOrEmpty(txtUsername.Text))
			{
				url = string.Format("{0}?email={1}", url, Uri.EscapeUriString(txtUsername.Text));
			}
			System.Diagnostics.Process.Start(url);
		}
	}
}