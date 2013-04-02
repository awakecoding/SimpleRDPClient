/**
 * FreeRDP: A Remote Desktop Protocol Implementation
 *
 * Copyright 2013 Benoit LeBlanc <benoit.leblanc@awakecoding.com>
 * Copyright 2013 Awake Coding Consulting Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AwakeCoding
{
	public partial class Form1 : Form
	{
		private bool connected;
		private string server;
		private string userName;
		private string domain;
		private string clearTextPassword;

		public bool Connected
		{
			get {
				return connected;
			}
			set {
				connected = value;
				toolStripButtonConnect2.Enabled = !connected;
				toolStripButtonDisconnect2.Enabled = connected;
			}
		}

		public Form1 ()
		{
			InitializeComponent ();
			Connected = false;
			ReadSettings (@"/tmp/connect.txt");
		}

		private void ReadSettings (string path)
		{
			Dictionary<string, string> settings = new Dictionary<string, string> ();

			try {
				using (StreamReader reader = new StreamReader(path)) {
					string line = reader.ReadLine ();
	                
					while (line != null) {
						string[] tokens = line.Split (new char[] { '=' }, 2);
						if (tokens.Length == 2) {
							settings [tokens [0].Trim ().ToLower ()] = tokens [1].Trim ();
						}
						line = reader.ReadLine ();
					}
				}

				server = settings ["server"];
				userName = settings ["username"];
				domain = settings ["domain"];
				clearTextPassword = settings ["password"];
			} catch (Exception e) {

			}
		}

		private void toolStripButtonDisconnect2_Click (object sender, EventArgs e)
		{
			rdpClientFrame1.Disconnect ();
		}

		private void toolStripButtonConnect2_Click (object sender, EventArgs e)
		{
			rdpClientFrame1.DesktopWidth = rdpClientFrame1.Width;
			rdpClientFrame1.DesktopHeight = rdpClientFrame1.Height;

			rdpClientFrame1.Server = server;
			rdpClientFrame1.UserName = userName;
			rdpClientFrame1.Domain = domain;
			rdpClientFrame1.AdvancedSettings.ClearTextPassword = clearTextPassword;
			rdpClientFrame1.Connect ();
		}

		private void rdpConnection_OnConnected (object sender, EventArgs e)
		{
			Connected = true;
		}

		private void rdpClientFrame_Connected (object sender, EventArgs e)
		{
			Connected = true;
		}

		private void rdpClientFrame_Disconnected (object sender, AwakeCoding.Common.DisconnectedEventArgs e)
		{
			Connected = false;
		}
	}
}
