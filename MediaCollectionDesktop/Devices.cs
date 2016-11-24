using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MediaCollection
{
	public partial class Devices : Form
	{
		public Devices()
		{
			InitializeComponent();
			TVDevices.CanExpandGetter = (object o) => {return o is Device;};
			TVDevices.ChildrenGetter = (object o) => {
				var dev = o as Device;
				if (dev != null)
				{
					return DevicePersistense.GetLocations(dev.Id);
				}
				return null;
			};

			olvColData.AspectGetter = (object o) => {
				var dev = o as Device;
				if (dev != null)
				{
					return dev.Data;
				}

				var loc = o as LocationBaseDeviceMapping;
				if (loc != null)
				{
					return loc.Mapping;
				}

				return null;
			};

			
			olvColData.AspectPutter = (object o, object val) => {
				var dev = o as Device;
				if (dev != null)
				{
					dev.Data = (string) val;
				}

				var loc = o as LocationBaseDeviceMapping;
				if (loc != null)
				{
					loc.Mapping = (string) val;
				}
			};
			
		}

		private void Devices_Shown(object sender, EventArgs e)
		{
			TVDevices.Roots = DevicePersistense.List();
		}

		private void TVDevices_CellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e)
		{
			int imageWidth = TVDevices.SmallImageSize.Width;
			if (e.RowObject is LocationBaseDeviceMapping)
			{
				if (e.SubItemIndex < 2)
				{
					//Can't edit first 2 columns for mapping
					e.Cancel = true;
					return;
				}
				imageWidth <<= 1;
			}

			int width = e.Column.Width;
			if (e.SubItemIndex == 0) 
			{
				width -= imageWidth;
			}
			else 
			{
				e.Control.Left -= imageWidth;
			}
			e.Control.Width = width;
			
		}
				
		private void TVDevices_CellEditFinishing(object sender, BrightIdeasSoftware.CellEditEventArgs e)
		{
		}

		private void TVDevices_CellEditFinished(object sender, BrightIdeasSoftware.CellEditEventArgs e)
		{
			var um = e.RowObject as UpdatableModel;
			if (um != null) um.Set();
				
		}

		private void BtnAdd_Click(object sender, EventArgs e)
		{
			var d = new Device { Data = "", Id = 0, Name = "New Device" };
			TVDevices.AddObject(d);
		}

		private void BtnDelete_Click(object sender, EventArgs e)
		{
			var item = TVDevices.SelectedItem;
			if (item != null)
			{
				var d = item.RowObject as Device;
				if (d != null)
				{
					if (MessageBox.Show("Do you want to delete " + d.Name + "?", "Confirm Device Removal", MessageBoxButtons.OKCancel) == DialogResult.OK)
					{
						try
						{
							d.Delete();
							Devices_Shown(null, null); //Re-fetch
						}
						catch (Exception err)
						{
							MessageBox.Show(err.Message, "Error");
						}
					}
				}
			}
		}
	}
}
