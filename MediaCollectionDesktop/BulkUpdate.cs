using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaCollection
{
	public partial class BulkUpdate : Form
	{
		public BulkUpdate()
		{
			InitializeComponent();
			OlvColumnDataType.SetupEnumColumn<MediaType>();
			
		}

		private async void CbxDevice_SelectedIndexChanged(object sender, EventArgs e)
		{
			m_rescanResults = null;
			CbxLocation.Items.Clear();
			var selected = GetSelectedDevice();
			if (selected != null)
			{
				CbxLocation.Items.Add(new LocationBase { Id = -1, Kind = LocationBaseKind.Local, Name = "All" });
				CbxLocation.Items.AddRange((await DevicePersistense.GetLocationsForTitleUpdate(selected.Id)).ToArray());
			}
			CbxLocation.Enabled = true;
			BtnApply.Enabled = false;
		}

		private async void BulkUpdate_Shown(object sender, EventArgs e)
		{
			CbxDevice.Items.AddRange((await DevicePersistense.ListForTitleUpdate()).ToArray());
			if (CbxDevice.Items.Count == 1)
			{
				CbxDevice.SelectedIndex = 0;
				CbxDevice_SelectedIndexChanged(null, null);
			}
			CbxDevice.Enabled = true;
		}

		private void CbxLocation_SelectedIndexChanged(object sender, EventArgs e)
		{
			m_rescanResults = null;
			BtnScan.Enabled = true;
			BtnApply.Enabled = false;
		}

		private Device GetSelectedDevice()
		{
			return CbxDevice.SelectedItem as Device;
		}

		private async void BtnScan_Click(object sender, EventArgs e)
		{
			var device = GetSelectedDevice();
			LVMissing.ClearObjects();
			LVNew.ClearObjects();
			if (device != null)
			{
				if (CbxLocation.SelectedIndex == 0) //All
				{
					for(int i = 1; i < CbxLocation.Items.Count; i ++)
					{
						await ScanLocation(CbxLocation.Items[i] as LocationBase, device);
					}
				}
				else 
				{
					await ScanLocation(CbxLocation.SelectedItem as LocationBase, device);
				}
				FindMoved();
			}
			LVNew.EmptyListMsg = "No New Items Found";
			LVMissing.EmptyListMsg = "Nothing Missing";
			BtnApply.Enabled = true;
		}

		RescanResults m_rescanResults;
		private async Task ScanLocation(LocationBase location, Device device)
		{
			UpdateWaitStatus("Processing " + location.Name + " ...");
			try
			{
				if (location == null || device == null) return;
				m_rescanResults = await RescanResults.Run(location.Id, device.Id);
				LVNew.AddObjects(m_rescanResults.NewFiles);
				LVMissing.AddObjects(m_rescanResults.MissingFiles);
				Application.DoEvents();
			}
			catch (Exception err)
			{
				MessageBox.Show(err.Message);
			}
		}

		private void FindMoved()
		{
			UpdateWaitStatus("Processing moved files...");
			//TODO: For each in missing find the new one with the same dir or file name
			UpdateWaitStatus(null);
		}

		private void UpdateWaitStatus(string message)
		{
			if (string.IsNullOrEmpty(message))
			{
				PanelUpdateStatus.Visible = false;
			}
			else
			{
				PanelUpdateStatus.Left = (Width - PanelUpdateStatus.Width) / 2;
				
				LblUpdateStatus.Text = message;
				PanelUpdateStatus.Visible = true;
			}
			Application.DoEvents();
			LblUpdateStatus.BringToFront();
			Application.DoEvents();
		}

		private async void BtnApply_Click(object sender, EventArgs e)
		{
			if (m_rescanResults == null) return;
			
			//Remove missing
			UpdateWaitStatus("Removing missing...");
			foreach(var mf in m_rescanResults.MissingFiles)
			{
				if (mf.ShouldDelete) await mf.Delete();
				else 
				{
					if (mf.NewLocationBaseId > 0 && !string.IsNullOrEmpty(mf.NewLocationData))
					{
						await mf.SetNewLocation();
					}
				}
			}
			LVMissing.ClearObjects();
			

			//Add new
			UpdateWaitStatus("Adding new...");
			foreach(var nf in m_rescanResults.NewFiles)
			{
				await nf.Save();
			}
			LVNew.ClearObjects();

			m_rescanResults = null;

			UpdateWaitStatus(null);
		}
	}
}
