using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BrightIdeasSoftware;

namespace MediaCollection
{
	public static class UIExtensions
	{
		public static void SetupEnumColumn<T>(this OLVColumn column) where T : struct, IComparable
		{
			Type t = typeof(T);
			string keyPrefix = t.Name + "_";
			var values = new System.Collections.ArrayList();
			foreach (T k in Enum.GetValues(t))
			{
				string key = keyPrefix + Enum.GetName(t, k);
				string v = Resources.ResourceManager.GetString(key);
				if (!string.IsNullOrEmpty(v)) values.Add(new ComboBoxItem(k, v));
			}


			column.AspectToStringConverter = (o) => {
				
				string key = keyPrefix + Enum.GetName(t, o);
				return Resources.ResourceManager.GetString(key);
			};

			ObjectListView.EditorRegistry.Register(t, delegate(Object modelPlaceholder, OLVColumn columnPlaceholder, Object valuePlaceholder)
			{
				var c = new ComboBox();
				c.DropDownStyle = ComboBoxStyle.DropDownList;
				c.ValueMember = "Key";
				c.DisplayMember = "Description";
				c.DataSource = values;
				return c;
			});
		}


		public static void SetupComboBox<T>(this ComboBox cbx, string prefix = "") where T : struct, IComparable
		{
			Type t = typeof(T);
			string keyPrefix = (prefix ?? "") + t.Name + "_";
			var values = new System.Collections.ArrayList();
			foreach (T k in Enum.GetValues(t))
			{
				string key = keyPrefix + Enum.GetName(t, k);
				string v = Resources.ResourceManager.GetString(key);
				if (!string.IsNullOrEmpty(v)) cbx.Items.Add(new ComboBoxItem(k, v));
			}
		}

		public static void SetSelectedKey<T>(this ComboBox cbx, T value) where T : struct, IComparable
		{
			for(int i = 0; i < cbx.Items.Count; i ++)
			{
				var item = cbx.Items[i] as ComboBoxItem;
				if (item != null && value.Equals(item.Key))
				{
					cbx.SelectedIndex = i;
					return;
				}
			}
		}

		public static T GetSelectedKey<T>(this ComboBox cbx, T defaultValue = default(T)) where T : struct, IComparable
		{
			var item = cbx.SelectedItem as ComboBoxItem;
			return item == null ? defaultValue : (T)item.Key;
		}



		public static TitlePropertyAccessor AssignControl(this TitlePropertyAccessor accessor, TextBox textbox, bool hideZero = true)
		{
			accessor.SetAccessors(() => textbox.Text, (val) => { textbox.Text = (hideZero && val == "0") ? "" : val; });
			return accessor;
		}

		public static Image ResizeKeepAspectRatio(this Image image, int targetWidth, int targetHeight, Color bgColor)
		{
			if (image == null) return null;
			double kW = ((double)targetWidth)/ image.Width;
			double kH = ((double)targetHeight) / image.Height;

			int offsetX;
			int offsetY;
			int newWidth;
			int newHeight;
			if (kW < kH)
			{
				newWidth = targetWidth;
				newHeight = (int)(image.Height * kW);
				offsetX = 0;
				offsetY = (targetHeight - newHeight) / 2;
			}
			else 
			{
				newWidth = (int)(image.Width * kH);
				newHeight = targetHeight;
				offsetX = (targetWidth - newWidth) / 2;
				offsetY = 0;
			}
			

			var bmp = new Bitmap(targetWidth, targetHeight);
			using (var g = Graphics.FromImage(bmp))
			{
				using (var br = new SolidBrush(bgColor)) 
				{
					g.FillRectangle(br, 0, 0, targetHeight, targetHeight);
				}

				g.DrawImage(image, offsetX, offsetY, newWidth, newHeight);
			}
			return bmp;
		}

		public static void Clear(this PictureBox pb)
		{
			if (pb == null) return;
			if (pb.Image == null) return;
			var img = pb.Image;
			pb.Image = null;
			img.Dispose();
		}
	}
}
