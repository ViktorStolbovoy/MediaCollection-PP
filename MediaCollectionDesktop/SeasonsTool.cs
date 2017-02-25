using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Stolbovoy.Utils;

namespace MediaCollection
{
	public partial class SeasonsTool : Form
	{
		
		public SeasonsTool(Title title)
		{
			InitializeComponent();
			m_title = null;
			string seriesName = ""; 
			switch (title.Kind)
			{
				case TitleKind.Series:
				{
					BtnCreateSeries.Enabled = false;
					seriesName = title.TitleName;
                    m_title = title;
                    break;
				}
				case TitleKind.Season:
				{
					seriesName = title.TitleName;
					int idx = seriesName.IndexOf(" season", StringComparison.InvariantCultureIgnoreCase);
					if (idx > 0) seriesName = seriesName.Substring(0, idx);
                    CheckSeries(seriesName);
                    break;
				}
				case TitleKind.Disk:
				{
					seriesName = StoredItem.ParseSeasonDiskEpisode(title.TitleName).Name;
                    CheckSeries(seriesName);
                    break;
				}
				case TitleKind.Episode:
				{
					seriesName = title.TitleName;
					int idx = seriesName.IndexOf(" episode", StringComparison.InvariantCultureIgnoreCase);
					if (idx > 0)
					{
						seriesName = seriesName.Substring(0, idx);
                        CheckSeries(seriesName);
                    }
					break;
				}
			}
			
			SetSeasonPattern(seriesName);
            TbxSeries.Text = seriesName;
        }

		private Title m_title;

		private void SetSeasonPattern(string seriesName)
		{
			TbxSeasonPattern.Text = seriesName + " %";
        }

        private void CheckSeries(string seriesName)
        {
            m_title = null;
            var ss = TitlePersistence.ListTitles(seriesName, TitleKind.Series);
            if (ss != null && ss.Count == 1)
            {
                m_title = ss[0];
                BtnCreateSeasons.Enabled = false;
            }
        }

		List<Title> foundSeasons = new List<Title>();
		private void BtnSearchForSeasons_Click(object sender, EventArgs e)
		{
            var seasonNumbersFound = new HashSet<int>();
            var seasonNumbersNotFound = new HashSet<int>();
            string sqlPattern = TbxSeasonPattern.Text;
            CheckSeries(TbxSeries.Text);

			//Search for pattern
			foundSeasons = TitlePersistence.ListTitles(sqlPattern, TitleKind.Season);
			ClbSeasons.Items.Clear();
			foreach (var title in foundSeasons)
			{
				ClbSeasons.Items.Add(title, !title.ParentTitleId.HasValue);
                seasonNumbersFound.Add(title.Season);
			}


            //Search for pattern
            var titlesDisks = TitlePersistence.ListTitles(sqlPattern, TitleKind.Disk);

            var titlesEpisodes = TitlePersistence.ListTitles(sqlPattern, TitleKind.Episode);

            ClbEpisodes.Items.Clear();
            foreach (var d in titlesDisks)
            {
                ClbEpisodes.Items.Add(d, !d.ParentTitleId.HasValue);
                int s = d.Season;
                if (s > 0 && !seasonNumbersFound.Contains(s)) seasonNumbersNotFound.Add(s);
            }
            foreach (var ep in titlesEpisodes)
            {
                ClbEpisodes.Items.Add(ep, !ep.ParentTitleId.HasValue);
                int s = ep.Season;
                if (s > 0 && !seasonNumbersFound.Contains(s)) seasonNumbersNotFound.Add(s);
            }

            var sb = new StringBuilder(seasonNumbersNotFound.Count * 2);

            TbxSeasonsToCreate.Text = string.Join(", ", seasonNumbersNotFound.OrderBy(i => i));

            BtnCreateSeasons.Enabled = seasonNumbersNotFound.Count > 0;
        }

		private void BtnSearchDiscs_Click(object sender, EventArgs e)
		{
			
        }

		private void BtnCreateSeasons_Click(object sender, EventArgs e)
		{
            if (m_title == null)
            {
                MessageBox.Show("Need to create series first");
                return;
            }

            string command = TbxSeasonsToCreate.Text;
			string pattern = TbxSeasonPattern.Text.Replace("%", "Season {0}");
			if (string.IsNullOrWhiteSpace(command)) return;
			var tokens1 = command.Trim().Split(',', ';');
			var seasons = new List<int>();
			foreach (string t in tokens1)
			{
				var tokens2 = t.Trim().Split('-');
				switch (tokens2.Length)
				{
					case 1: seasons.Add(tokens2[0].To<int>(0)); break;
					case 2: 
						for (int i = tokens2[0].To<int>(0); i <= tokens2[1].To<int>(0); i++)
						{
							seasons.Add(i);
						}
						break;
					default:
						MessageBox.Show("Invalid session string: " + tokens2);
						return;
				}
			}

			foreach(int i in seasons) 
			{
				bool found = false;
				foreach (var title in foundSeasons)
				{
					if (title.Season == i)
					{
						found = true;
						break;
					}
				}
				if (found) continue;

				var season = TitlePersistence.AddTitle(string.Format(pattern, i), TitleKind.Season, i, 0 , 0 , m_title.Id);
				ClbSeasons.Items.Add(season, true);
				TbxSeasonsToCreate.Text = "";
			}


            BtnCreateSeasons.Enabled = false;
        }

		private void TbxSeries_TextChanged(object sender, EventArgs e)
		{

		}

        private void BtnCreateSeries_Click(object sender, EventArgs e)
        {
            TitlePersistence.AddTitle(TbxSeries.Text, TitleKind.Series, 0, 0, 0, null);
            BtnCreateSeries.Enabled = false;
        }

        private void SeasonsTool_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void BtnAutoMove_Click(object sender, EventArgs e)
        {
            if (m_title == null)
            {
                MessageBox.Show("Need to create series first");
                return;
            }

            var seasons = new Dictionary<int, long>();
            long seriesTitleId = m_title.Id;

            
            int numSeasonItems = ClbSeasons.Items.Count;

            for (int i = 0; i < numSeasonItems; i ++)
            {
                StatusUpdate("seasons", i + 1, numSeasonItems);
                Title s = (Title)ClbSeasons.Items[i];
                if (s != null)
                {
                    if (ClbSeasons.GetItemChecked(i) && !s.ParentTitleId.HasValue)
                    {
                        s.ParentTitleId = seriesTitleId;
                        TitlePersistence.ReparentTitle(s.Id, seriesTitleId);
                    }
                    if (!seasons.ContainsKey(s.Season)) seasons.Add(s.Season, s.Id);
                }
            }
            int numDiskItems = ClbEpisodes.Items.Count;
            for (int i = 0; i < numDiskItems; i++)
            {
                StatusUpdate("episodes and disks", i + 1, numDiskItems);
                Title ep = (Title)ClbEpisodes.Items[i];
                if (ClbEpisodes.GetItemChecked(i) && ep != null)
                {
                    if (!ep.ParentTitleId.HasValue)
                    {
                        long seasonTitleId;
                        if (seasons.TryGetValue(ep.Season, out seasonTitleId))
                        {
                            ep.ParentTitleId = seasonTitleId;
                            TitlePersistence.ReparentTitle(ep.Id, seasonTitleId);
                        }
                    }
                }
            }
            
            BtnSearchForSeasons_Click(sender, e);
            EndUpdate();
        }



        private void StatusUpdate(string what, int current, int total)
        {
            PnlStatus.Text = string.Format("Processing {0} of {1} {2}", current, total, what);
            PnlStatus.Visible = true;
            Application.DoEvents();
        }

        private void EndUpdate()
        {
            PnlStatus.Visible = false;
        }

        private void BtnRecategorise_Click(object sender, EventArgs e)
        {
            if (BtnRecategorise.Text == "Convert")
            {
                ConvertTitles();
                BtnRecategorise.Text = "Recategorise";
            }
            else
            {
                if (FindTitlesToConvert())
                {
                    BtnRecategorise.Text = "Convert";
                }
            }
        }

        private bool FindTitlesToConvert()
        {
            ClbEpisodes.Items.Clear();
            var titles = TitlePersistence.ListTitles(TbxSeasonPattern.Text, TitleKind.Title);
            try
            {
                var regex = new Regex(TbxEpisodesRegexp.Text);
                foreach (var t in titles)
                {
                    var matches = regex.Match(t.TitleName);
                    if (matches.Success && matches.Groups.Count > 2)
                    {
                        int season = matches.Groups["s"].Value.To<int>(0);
                        int disk = matches.Groups["d"].Value.To<int>(0);
                        int episode = matches.Groups["e"].Value.To<int>(0);
                        if (disk > 0 || episode > 0)
                        {
                            t.Season = season;
                            t.Disk = disk;
                            t.EpisodeOrTrack = episode;
                            t.Kind = (episode > 0) ? TitleKind.Episode : TitleKind.Disk;
                            ClbEpisodes.Items.Add(t, true);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error searching fro episodes/disks");
            }
            return ClbEpisodes.Items.Count > 0;
        }

        private void ConvertTitles()
        {
            int numTitles = ClbEpisodes.Items.Count;

            for (int i = 0; i < numTitles; i++)
            {
                StatusUpdate("episodes/disks", i + 1, numTitles);
                Title t = (Title)ClbEpisodes.Items[i];
                if (t != null)
                {
                    if (ClbEpisodes.GetItemChecked(i))
                    {
                        try
                        {
                            GeneralPersistense.Upsert(t);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message, "Error updating title " + t.TitleName);
                        }
                    }
                }
            }
            EndUpdate();
            ClbEpisodes.Items.Clear();
        }
    }
   
}
