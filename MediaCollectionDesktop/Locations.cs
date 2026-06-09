using System;
using System.Windows.Forms;

namespace MediaCollection
{
	public partial class Locations : Form
	{
		public Locations()
		{
			InitializeComponent();
			var locations = LocationPersistence.ListBases().GetAwaiter().GetResult();
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
			if (um != null) um.Set().GetAwaiter().GetResult();
				
		}

		private void BtnAdd_Click(object sender, EventArgs e)
		{
			var d = new LocationBase { Kind = LocationBaseKind.Local, Name = "" };
			LVLocations.AddObject(d);
		}

		private async void BtnDelete_Click(object sender, EventArgs e)
		{
			var item = LVLocations.SelectedItem;
			if (item != null)
			{
				var d = item.RowObject as LocationBase;
				if (d != null)
				{
					if (MessageBox.Show("Do you want to delete " + d.Name + "?", "Confirm Location Removal", MessageBoxButtons.OKCancel) == DialogResult.OK)
					{
						await d.Delete();
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
