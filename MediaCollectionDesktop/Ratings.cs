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
	public partial class Ratings : Form
	{
		public Ratings()
		{
			InitializeComponent();
			var ratings = GeneralPersistense.FetchAll<RatingProvider>();
			LVRatings.VirtualMode = false;
			LVRatings.SetObjects(ratings);

			olvColumnRatingStep.AspectPutter = (object o, object val) => {
				var r = o as RatingProvider;
				if (r != null)
				{
					r.RatingStep = Convert.ToSingle(val);
				}
			};
			olvColumnRatingMin.AspectPutter = (object o, object val) => {
				var r = o as RatingProvider;
				if (r != null)
				{
					r.RatingMax = Convert.ToSingle(val);
				}
			};
			olvColumnRatingMax.AspectPutter = (object o, object val) => {
				var r = o as RatingProvider;
				if (r != null)
				{
					r.RatingMax = Convert.ToSingle(val);
				}
			};
		}


		private void Locations_Shown(object sender, EventArgs e)
		{
			
		}

		private void LVLocations_CellEditStarting(object sender, BrightIdeasSoftware.CellEditEventArgs e)
		{
			int width = e.Column.Width;
			e.Control.Width = width;
			var numEditor = e.Control as BrightIdeasSoftware.FloatCellEditor;
			if (numEditor != null)
			{
				numEditor.Minimum = 0m;
				numEditor.Maximum = 999m;
				numEditor.Increment = 0.1m;
				numEditor.DecimalPlaces = 1;
			}
			
		}


		private void LVLocations_CellEditFinished(object sender, BrightIdeasSoftware.CellEditEventArgs e)
		{
			var um = e.RowObject as UpdatableModel;
			if (um != null) um.Set();
		}

		private void BtnAdd_Click(object sender, EventArgs e)
		{
			var d = new RatingProvider { RatingKind = 0, RatingName = "New Rating", RatingMax = 10, RatingMin = 1, RatingStep = 1 };
			LVRatings.AddObject(d);
		}

		private void BtnDelete_Click(object sender, EventArgs e)
		{
			var item = LVRatings.SelectedItem;
			if (item != null)
			{
				var d = item.RowObject as RatingProvider;
				if (d != null)
				{
					if (MessageBox.Show("Do you want to delete " + d.RatingName + "?", "Confirm Rating Removal", MessageBoxButtons.OKCancel) == DialogResult.OK)
					{
						d.Delete();
						item.Remove();
					}
				}
			}
		}
	}
}
