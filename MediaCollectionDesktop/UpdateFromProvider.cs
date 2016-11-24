using System;
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

namespace MediaCollection
{
	public partial class UpdateFromProvider : Form
	{
		public UpdateFromProvider(List<Title> titlesToUpdate)
		{
			m_titlesToUpdate = titlesToUpdate;
			InitializeComponent();
			Setup();
			BtnNext.Enabled = false;
			BtnPrevious.Enabled = titlesToUpdate.Count > 1;
			m_currentTitleIndex = 0;
			if (titlesToUpdate.Count > 0)
			{	
				PopulateUI(titlesToUpdate[0]);
			}
		}

		private List<Title> m_titlesToUpdate;
		private int m_currentTitleIndex;

		public UpdateFromProvider(Title titleToUpdate)
		{
			InitializeComponent();
			Setup();
			PopulateUI(titleToUpdate);
			BtnNext.Enabled = false;
			BtnPrevious.Enabled = false;
		}

		private void Setup()
		{
			OlvPoster.AspectGetter = (x) => {
				var t = x as TmdbResult;
				if (t == null) return null;
				return string.Format("{0} ({1:yyyy})",t.Title, t.ReleaseDate);
			};
			/*OlvPoster.AspectToStringConverter = (x) => {
				return " ";
			};*/

			OlvPoster.ImageGetter = (x) => {
				string key = ((TmdbResult)x).PosterPath;
				if (!string.IsNullOrEmpty(key) && ImageListPosters.Images.ContainsKey(key)) return key;
				return null;
			};
		}

		
		Title m_currentTitle;
		public void PopulateUI(Title title)
		{
			m_currentTitle = title;
			TbxName.Text = title.TitleName;
			bool isTv = title.Kind == TitleKind.Series || title.Kind == TitleKind.Season || title.Kind == TitleKind.Episode;
			RbnTv.Checked = isTv;
			Search(title.TitleName);
			
		}

		private void BtnSearch_Click(object sender, EventArgs e)
		{
			string name = TbxName.Text;
			Search(name);
		}

		private void Search(string name)
		{
			if (string.IsNullOrEmpty(name)) return;
			LVResults.ClearObjects();
			Task<TmdbData> t = null;
			var waitForm = new Wait((cts)=>{return t = TmdbData.Get(name.Trim(), RbnTv.Checked, cts.Token);}, "Retrivering title information: {0} seconds elapsed");
			if (waitForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{

				var res = t.Result;

				ImageListPosters.Images.Clear();
				foreach (var item in res.Results)
				{
					if (item.Poster != null)
					{
						using (var ms = new MemoryStream(item.Poster))
						{
							using (var img = Image.FromStream(ms))
							{
								using (var scaledImage = img.ResizeKeepAspectRatio(ImageListPosters.ImageSize.Width, ImageListPosters.ImageSize.Height, LVResults.BackColor))
								{
									ImageListPosters.Images.Add(item.PosterPath, scaledImage);
								}
							}
						}
					}
				}

				LVResults.AddObjects(res.Results);
			}
			
			
		}

		private void TbxName_TextChanged(object sender, EventArgs e)
		{

		}

		private void LVResults_CellToolTipShowing(object sender, BrightIdeasSoftware.ToolTipShowingEventArgs e)
		{
			var item = e.Model as TmdbResult;
			if (item == null) return;
			e.Text = item.Overview.Wrap(50);
		}

		private void SetTitle(TmdbResult searchResult)
		{
			if (m_currentTitle == null) return;

			if (string.IsNullOrWhiteSpace(searchResult.ImdbId))
			{
				//TODO: make it async
				(new Wait((cts)=>searchResult.GetMore(cts.Token), "Retrieving additional info: {0} seconds elapsed")).ShowDialog();
			}
			m_currentTitle.DateModifiedUtc = GeneralPersistense.GetTimestamp();
			if (CbxOverrideDescription.Checked && !string.IsNullOrWhiteSpace(searchResult.Overview)) m_currentTitle.Description = searchResult.Overview;
			if (CbxOverrideTitle.Checked) m_currentTitle.TitleName = searchResult.Title;
			if (CbxOverrideYear.Checked && searchResult.ReleaseDate.HasValue) m_currentTitle.Year = searchResult.ReleaseDate.Value.Year;
			if (!string.IsNullOrWhiteSpace(searchResult.ImdbId)) m_currentTitle.ImdbId = searchResult.ImdbId;
			
			
				/*StoredItem.SeasonPersistence,
				StoredItem.DiskPersistence,
				StoredItem.EpisodePersistence*/
			
			GeneralPersistense.Upsert(m_currentTitle);
			
			if (searchResult.Poster != null && searchResult.Poster.Length > 0)
			{
				MediaSamplePersistence.AddSample(searchResult.Poster, m_currentTitle.Id, MediaSampleKind.Image, Path.GetExtension(searchResult.PosterPath));
			}
		}

		private void LVResults_DoubleClick(object sender, EventArgs e)
		{
			var res = LVResults.SelectedObject as TmdbResult;
			if (res != null)
			{
				SetTitle(res);
				if (BtnNext.Enabled)
				{
					BtnNext_Click(sender, e);
				}
				else
				{
					DialogResult = System.Windows.Forms.DialogResult.OK;
					Close();
				}
			}
		}

		private void BtnNext_Click(object sender, EventArgs e)
		{
			if (m_currentTitleIndex + 1 < m_titlesToUpdate.Count)
			{
				m_currentTitleIndex++;
				PopulateUI(m_titlesToUpdate[m_currentTitleIndex]);
				BtnPrevious.Enabled = m_currentTitleIndex < m_currentTitleIndex + 1;
			}
		}

		private void BtnPrevious_Click(object sender, EventArgs e)
		{
			if (m_currentTitleIndex > 0)
			{
				m_currentTitleIndex --;
				if (m_titlesToUpdate.Count > m_currentTitleIndex)
				{
					PopulateUI(m_titlesToUpdate[m_currentTitleIndex]);
				}
				BtnPrevious.Enabled = m_currentTitleIndex != 0;
			}
		}
	}
}
