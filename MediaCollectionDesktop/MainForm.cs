using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stolbovoy.Utils;

using BrightIdeasSoftware;

namespace MediaCollection
{
	[Flags]
	enum ResourceKind { None = 0, Audio = 1, Video = 2};
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();
			LVLocations.VirtualMode = false;
			LVRatings.VirtualMode = false;

			AppDomain.CurrentDomain.UnhandledException += (o, e) => {
				var ex = e.ExceptionObject as Exception;
				string msg = ex == null ? "Unhandled error" : ex.Message;
				MessageBox.Show(msg, "Application Error");
			};
			Application.ThreadException += (o, e) => {
				string msg = e.Exception == null ? "Unhandled error (thread)" : e.Exception.Message;
				MessageBox.Show(msg, "Application Error");
			};

			TVTitles.CanExpandGetter = (o) => {
				var t = o as Title;
				if (t == null) return false;
				return t.Kind != TitleKind.Episode && t.Kind != TitleKind.Track && t.Kind != TitleKind.Title;
			};

			TVTitles.ChildrenGetter = (o) => {
				var t = o as Title;
				if (t == null) return null;
				return TitlePersistence.ListTitlesByParent(t.Id);
			};

			OlvColumnName.ImageGetter = (o) => {
				var t = o as Title;
				if (t == null) return -1;
				return (int)t.Kind;
			};

			OlvBtnPlay.AspectGetter = (o) => {
				var l = o as LocationForDisplay;
				if (l != null && l.LocationKind != LocationBaseKind.Shelf) return "Play";
				return null;
			};

			var sink = (BrightIdeasSoftware.SimpleDropSink)TVTitles.DropSink;
			sink.AcceptExternal = false;
			sink.CanDropBetween = false;
			sink.CanDropOnBackground = false;
			sink.CanDropOnItem = true;
			sink.CanDropOnSubItem = true;

			sink.CanDrop += (sender, e) => {
				e.Handled = true;
				e.Effect = DragDropEffects.None;
				var models = GetModelsFromDropEvent(e);

				if (CanDrop(models.Item1, models.Item2))
				{
					e.Effect = DragDropEffects.Move;
				}
			};

			sink.Dropped += (sender, e) => {
				var models = GetModelsFromDropEvent(e);
				if (models.Item1 == null || models.Item2 == null) return;
				models.Item1.ParentTitleId = models.Item2.Id;
				GeneralPersistense.Upsert(models.Item1);
				e.Effect = DragDropEffects.Move;
				TVTitles.RemoveObject(models.Item1);
				TVTitles.RefreshObject(models.Item2);
			};

			TVTitles.ModelFilter = new ModelFilter((m) => {
				var t = m as Title;
				if (t == null) return false;

				string filter = TbxSearch.Text.Trim().ToLower();
				if (filter.Length == 0) return true;

				return t.TitleName.ToLower().Contains(filter);
			});

			olvColumnRatingValue.AspectPutter = (object o, object val) => {
				var r = o as TitleRatingWithName;
				if (r != null)
				{
					r.RatingValue = Convert.ToSingle(val);
				}
			};



			CbxDevices.Items.AddRange(DevicePersistense.ListForPalyback().ToArray());
			if (CbxDevices.Items.Count > 0) CbxDevices.SelectedIndex = 0;

			m_imageIndex = 0;
		}

		private List<MediaSample> m_images;
		private int m_imageIndex;

		private static bool CanDrop(Title source, Title destination)
		{
			if (source == null || destination == null) return false;

			switch (destination.Kind)
			{
				case TitleKind.Album: return source.Kind == TitleKind.Track;
				case TitleKind.AlbumArtist: return source.Kind == TitleKind.Track || source.Kind == TitleKind.Album;
				case TitleKind.Episode: return false;
				case TitleKind.Season: return source.Kind == TitleKind.Episode || source.Kind == TitleKind.Title;
				case TitleKind.Series: return source.Kind == TitleKind.Season || source.Kind == TitleKind.Episode || source.Kind == TitleKind.Title;
				default: return false;
			}
		}

		private static Tuple<Title, Title> GetModelsFromDropEvent(BrightIdeasSoftware.OlvDropEventArgs e)
		{
			var dataObject = e.DataObject as BrightIdeasSoftware.OLVDataObject;
			Title source = null;
			Title destination = null;
			if (dataObject != null)
			{
				source = dataObject.ModelObjects.Count > 0 ? dataObject.ModelObjects[0] as Title : null;
				destination = e.DropTargetItem == null ? null : e.DropTargetItem.RowObject as Title;
			}
			return new Tuple<Title, Title>(source, destination);
		}


		private ResourceKind GetResourceKind()
		{
			if (RbnVideo.Checked) return ResourceKind.Video;
			if (RbnAudio.Checked) return ResourceKind.Audio;
			return ResourceKind.None;
		}


		private void Reload()
		{
			var kind = GetResourceKind();
			TVTitles.ClearObjects();
			CbxKind.Items.Clear();

			BtnSave.Enabled = false;

			switch (kind)
			{
				case ResourceKind.Audio:
					TVTitles.Roots = TitlePersistence.ListRootAudio();
					CbxKind.SetupComboBox<TitleKind>("Audio_");
					break;
				case ResourceKind.Video:
					TVTitles.Roots = TitlePersistence.ListRootVideo();
					CbxKind.SetupComboBox<TitleKind>("Video_");
					break;
			}

			m_currentTitle = null;
			m_images = null;
			m_imageIndex = 0;
			SetImageNavigationControls();
			PbxImage.Image = null;
		}

		private void TSMIDevices_Click(object sender, EventArgs e)
		{
			(new Devices()).ShowDialog();
		}

		private void TSMIBulkAddLocations_Click(object sender, EventArgs e)
		{
			(new Locations()).ShowDialog();
		}

		private void TSMIAddManually_Click(object sender, EventArgs e)
		{
			(new Test()).ShowDialog();
		}

		private void TSMIBulkAdd_Click(object sender, EventArgs e)
		{
			(new BulkUpdate()).ShowDialog();
		}

		private void MainForm_Shown(object sender, EventArgs e)
		{
			Reload();
		}

		private void SetEpisodeControlsState(bool isEnabled)
		{
			LblSeason.Enabled = isEnabled;
			LblDisk.Enabled = isEnabled;
			LblEpisode.Enabled = isEnabled;

			TbxSeason.Enabled = isEnabled;
			TbxDisk.Enabled = isEnabled;
			TbxEpisode.Enabled = isEnabled;
		}

		Title m_currentTitle;
		private void DisplayTitleInfo(Title title)
		{
			m_currentTitle = title;
			LVLocations.ClearObjects();
			LVRatings.ClearObjects();
			m_imageIndex = 0;

			if (title != null)
			{
				TbxReleaseYear.Text = title.Year.ToString("##");
				TbxDescription.Text = title.Description;
				TbxImdbId.Text = title.ImdbId;
				TbxSeason.Text = title.Season.ToString("##");
				TbxDisk.Text = title.Disk.ToString("##");
				TbxEpisode.Text = title.EpisodeOrTrack.ToString("##");

				TbxTitleName.Text = title.TitleName;
				CbxKind.SetSelectedKey(title.Kind);

				LVLocations.AddObjects(LocationPersistence.ListTitleLocations(title.Id));
				LVRatings.AddObjects(TitlePersistence.GetRatings(title.Id));
				SetEpisodeControlsState(m_currentTitle.Kind == TitleKind.Episode);
				m_images = MediaSamplePersistence.GetSamples(title.Id, MediaSampleKind.Image);
				DisplayImage();
			}
			else
			{
				TbxReleaseYear.Text = "";
				TbxDescription.Text = "";
				TbxImdbId.Text = "";
				TbxSeason.Text = "";
				TbxDisk.Text = "";
				TbxEpisode.Text = "";

				TbxTitleName.Text = "";
				CbxKind.SelectedIndex = -1;
				SetEpisodeControlsState(false);
				m_images = null;
				SetImageNavigationControls();
				PbxImage.Clear();
			}

		}

		private void SetImageNavigationControls()
		{
			BtnPrevImage.Enabled = m_images != null && m_imageIndex > 0;
			BtnNextImage.Enabled = m_images != null && m_imageIndex + 1 < m_images.Count;
		}
		private void DisplayImage()
		{
			SetImageNavigationControls();
			PbxImage.Clear();
			if (m_images == null || m_imageIndex < 0 || m_imageIndex >= m_images.Count) return;
			var sample = m_images[m_imageIndex];
			if (sample != null)
			{
				using (var img = sample.GetImage())
				{
					var imgResized = img.ResizeKeepAspectRatio(PbxImage.Width, PbxImage.Height, Color.White);
					PbxImage.Image = imgResized;
				}
			}
		}

		private bool IsDirty()
		{
			if (m_currentTitle == null) return false;

			if (TbxTitleName.Text != m_currentTitle.TitleName) return true;
			if (CbxKind.GetSelectedKey<TitleKind>() != m_currentTitle.Kind) return true;

			if (TbxReleaseYear.Text.To<int>(0) != m_currentTitle.Year) return true;
			if (TbxDescription.Text != m_currentTitle.Description) return true;
			if (TbxImdbId.Text != m_currentTitle.ImdbId) return true;
			if (TbxSeason.Text.To<int>(0) != m_currentTitle.Season) return true;
			if (TbxDisk.Text.To<int>(0) != m_currentTitle.Disk) return true;
			if (TbxEpisode.Text.To<int>(0) != m_currentTitle.EpisodeOrTrack) return true;

			return false;
		}

		private void Set()
		{
			if (m_currentTitle == null) return;

			m_currentTitle.DateModifiedUtc = GeneralPersistense.GetTimestamp();
			m_currentTitle.TitleName = TbxTitleName.Text;
			m_currentTitle.Kind = CbxKind.GetSelectedKey<TitleKind>();
			m_currentTitle.Year = TbxReleaseYear.Text.To<int>(0);
			m_currentTitle.Description = TbxDescription.Text;
			m_currentTitle.ImdbId = TbxImdbId.Text;
			m_currentTitle.Season = TbxSeason.Text.To<int>(0);
			m_currentTitle.Disk = TbxDisk.Text.To<int>(0);
			m_currentTitle.EpisodeOrTrack = TbxEpisode.Text.To<int>(0);

			GeneralPersistense.Upsert(m_currentTitle);

			BtnSave.Enabled = false;
		}

		private void TVTitles_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			DisplayTitleInfo(TVTitles.SelectedObject as Title);
		}

		private void BtnSave_Click(object sender, EventArgs e)
		{
			Set();
		}

		private void OnKeyPressNumericOnly(object sender, KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
			{
				e.Handled = true;
			}
		}

		private bool SetControlsFromDirtyState()
		{
			bool isDirty = IsDirty();
			BtnSave.Enabled = isDirty;
			BtnSearhProvider.Enabled = !isDirty;
			BtnNew.Enabled = !isDirty;
			BtnDeleteImage.Enabled = !isDirty;
			BtnAddImage.Enabled = !isDirty;
			BtnAddLocation.Enabled = !isDirty;
			BtnRemoveLocation.Enabled = !isDirty;
			return isDirty;
		}

		private void CbxKind_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetEpisodeControlsState(CbxKind.GetSelectedKey<TitleKind>() == TitleKind.Episode);
			SetControlsFromDirtyState();
		}

		private void OnTextChanged(object sender, EventArgs e)
		{
			SetControlsFromDirtyState();
		}

		private void BtnDiscard_Click(object sender, EventArgs e)
		{
			DisplayTitleInfo(m_currentTitle);
		}

		private void RbnVideo_CheckedChanged(object sender, EventArgs e)
		{
			Reload();
		}

		private void BtnNew_Click(object sender, EventArgs e)
		{
			TitleKind kind;
			switch (GetResourceKind())
			{
				case ResourceKind.Audio: kind = TitleKind.AlbumArtist; break;
				case ResourceKind.Video: kind = TitleKind.Title; break;
				default: return;
			}

			Title newTitle = null;

			foreach (Title item in TVTitles.Objects)
			{
				if (item.Id < 1)
				{
					//Unsaved new item already there
					newTitle = item;
				}
			}
			if (newTitle == null)
			{
				newTitle = new Title() { TitleName = "New", Kind = kind, DateAddedUtc = GeneralPersistense.GetTimestamp() };
				TVTitles.AddObject(newTitle);
			}
			TVTitles.SelectedObject = newTitle;
			DisplayTitleInfo(newTitle);
			TVTitles.EnsureModelVisible(newTitle);
		}

		private void LVLocations_ButtonClick(object sender, BrightIdeasSoftware.CellClickEventArgs e)
		{
			var location = e.Model as LocationForDisplay;
			if (location == null) return;
			var device = CbxDevices.SelectedItem as Device;
			if (device == null) return;
			LocationPersistence.Run(device.Id, location.Id);
		}

		private void BtnSearhProvider_Click(object sender, EventArgs e)
		{
			if (m_currentTitle == null) return;
			var provider = new UpdateFromProvider(m_currentTitle);
			provider.ShowDialog();
			DisplayTitleInfo(m_currentTitle);
		}

		private void TbxSearch_TextChanged(object sender, EventArgs e)
		{
			TVTitles.BeginUpdate();
			var mf = TVTitles.ModelFilter;
			TVTitles.ModelFilter = null;
			TVTitles.ModelFilter = mf;
			TVTitles.EndUpdate();
		}

		private void BtnPrevImage_Click(object sender, EventArgs e)
		{
			if (m_images != null && m_imageIndex > 0)
			{
				m_imageIndex--;
				DisplayImage();
			}
		}

		private void BtnNextImage_Click(object sender, EventArgs e)
		{
			if (m_images != null && m_imageIndex < m_images.Count - 1)
			{
				m_imageIndex++;
				DisplayImage();
			}
		}

		private void BtnDeleteImage_Click(object sender, EventArgs e)
		{
			if (m_images == null || CheckForChanges()) return;
			if (m_imageIndex >= m_images.Count) return;
			if (MessageBox.Show("Are you sure you want to delete current image?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				MediaSamplePersistence.RemoveSample(m_images[m_imageIndex]);
				m_images.RemoveAt(m_imageIndex);
				if (m_imageIndex >= m_images.Count) m_imageIndex--;
				if (m_imageIndex < 0)
				{
					m_imageIndex = 0;
				}
				DisplayImage();
			}
		}

		private bool CheckForChanges()
		{
			if (IsDirty())
			{
				MessageBox.Show("Please save changes first");
				return true;
			}
			return false;
		}

		private void BtnAddImage_Click(object sender, EventArgs e)
		{
			var ofd = new OpenFileDialog();
			ofd.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
			ofd.FilterIndex = 0;
			ofd.RestoreDirectory = true;

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				byte[] data = File.ReadAllBytes(ofd.FileName);
				var img = MediaSamplePersistence.AddSample(data, m_currentTitle.Id, MediaSampleKind.Image, Path.GetExtension(ofd.FileName));
				m_images.Add(img);
				m_imageIndex = m_images.Count - 1;
				DisplayImage();
			}
		}

		private void LVLocations_CellEditStarting(object sender, CellEditEventArgs e)
		{
			var item = e.RowObject as LocationForDisplay;
			if (item != null && e.SubItemIndex == 1)
			{
				var bases = LocationPersistence.ListBases();
				var control = new ComboBox();
				int selectedIndex = -1;
				for(int i = 0; i < bases.Count; i ++)
				{
					var b = bases[i];
					control.Items.Add(new ComboBoxItem(b.Id, b.Name));
					if (b.Id == item.LocationBaseId) selectedIndex = i;
				}
				
				control.SelectedIndex = selectedIndex;
				control.Width = e.Column.Width;
				control.Left = e.Control.Left;
				e.Control = control;
			}
		}

		private void LVLocations_CellEditFinishing(object sender, CellEditEventArgs e)
		{
			var item = e.RowObject as LocationForDisplay;
			if (item != null) 
			{
				switch (e.SubItemIndex)
				{
					case 1:
						{
							var ctrl = e.Control as ComboBox;
							if (ctrl != null)
							{
								var selected = ctrl.SelectedItem as ComboBoxItem;
								if (selected != null)
								{
									item.LocationBaseId = (long)selected.Key;
									e.NewValue = item.LocationBase = selected.ToString();
									item.DateModifiedUtc = GeneralPersistense.GetTimestamp();
									GeneralPersistense.Upsert(new Location(item));
								}
							}
							break;
						}
					case 2:
						item.LocationData = (string)e.NewValue;
						item.DateModifiedUtc = GeneralPersistense.GetTimestamp();
						GeneralPersistense.Upsert(new Location(item));
						break;
				}
			}
		}


		private void TSMISystem_Click(object sender, EventArgs e)
		{

		}

		private void BtnRemoveLocation_Click(object sender, EventArgs e)
		{

		}

		private void RatingsToolStripMenuItem_Click(object sender, EventArgs e)
		{				
			(new Ratings()).ShowDialog();
		}

		private void LVRatings_CellEditStarting(object sender, CellEditEventArgs e)
		{
			int width = e.Column.Width;
			e.Control.Width = width;
			var numEditor = e.Control as BrightIdeasSoftware.FloatCellEditor;
			if (numEditor != null)
			{
				var r = e.RowObject as TitleRatingWithName;
				numEditor.Minimum = (decimal) r.RatingMin;
				numEditor.Maximum = (decimal)r.RatingMax;
				numEditor.Increment = (decimal)r.RatingStep;
				numEditor.DecimalPlaces = 1;
			}
		}

		private void LVRatings_CellEditFinished(object sender, CellEditEventArgs e)
		{
			var r = e.RowObject as TitleRatingWithName;
			if (r != null && m_currentTitle != null) r.Set(m_currentTitle.Id);
		}
	}
}
