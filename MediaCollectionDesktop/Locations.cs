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
	public partial class Locations : Form
	{
		public Locations()
		{
			InitializeComponent();
			var locations = LocationPersistence.ListBases();
			LVLocations.VirtualMode = false;
			LVLocations.SetObjects(locations);
		}

		private void Locations_Shown(object sender, EventArgs e)
		{
			
		}

		private void LVLocations_CellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e)
		{
			int width = e.Column.Width;
			e.Control.Width = width;
		}


		private void LVLocations_CellEditFinished(object sender, BrightIdeasSoftware.CellEditEventArgs e)
		{
			var um = e.RowObject as UpdatableModel;
			if (um != null) um.Set();
				
		}

		private void BtnAdd_Click(object sender, EventArgs e)
		{
			var d = new LocationBase { Kind = LocationBaseKind.Local, Name = "" };
			LVLocations.AddObject(d);
		}

		private void BtnDelete_Click(object sender, EventArgs e)
		{
			var item = LVLocations.SelectedItem;
			if (item != null)
			{
				var d = item.RowObject as LocationBase;
				if (d != null)
				{
					if (MessageBox.Show("Do you want to delete " + d.Name + "?", "Confirm Location Removal", MessageBoxButtons.OKCancel) == DialogResult.OK)
					{
						d.Delete();
						item.Remove();
					}
				}
			}
		}

		private void LVLocations_SelectedIndexChanged(object sender, EventArgs e)
		{

		}
	}
}
