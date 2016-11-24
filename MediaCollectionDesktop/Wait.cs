using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Stolbovoy.Utils;


namespace MediaCollection
{
	public partial class Wait : Form
	{
		public Wait(Func<CancellationTokenSource, Task> taskFactory, string messageFormat)
		{
			m_ownsCancellationTokenSource = true;
			m_messageFormat = messageFormat;
			m_cancellationTokenSource = new CancellationTokenSource();
			m_task = taskFactory(m_cancellationTokenSource);
			Init();

		}
		public Wait(Task task, CancellationTokenSource cancellationTokenSource, string messageFormat)
		{
			m_ownsCancellationTokenSource = false;
			m_messageFormat = messageFormat;
			m_task = task;
			m_cancellationTokenSource = cancellationTokenSource;
			Init();
		}

		private void Init()
		{
			InitializeComponent();
			m_startedTime = DateTime.UtcNow;
			TimerSpent_Tick(null, null);
			m_task.ContinueWith((t) => {
				DialogResult = (m_task.IsCanceled) ? System.Windows.Forms.DialogResult.Cancel : System.Windows.Forms.DialogResult.OK;
				this.Close();
			}, TaskScheduler.FromCurrentSynchronizationContext());
		}

		private string m_messageFormat;
		private Task m_task;
		private DateTime m_startedTime;
		private bool m_ownsCancellationTokenSource;
		CancellationTokenSource m_cancellationTokenSource;

		private void TimerSpent_Tick(object sender, EventArgs e)
		{
			LblStatus.Text = string.Format(m_messageFormat, (DateTime.UtcNow - m_startedTime).TotalSeconds).Wrap(40);
		}

		private void BtnCancel_Click(object sender, EventArgs e)
		{
			m_cancellationTokenSource.Cancel();
		}

		private void Wait_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (m_ownsCancellationTokenSource && m_cancellationTokenSource != null)
			{
				m_cancellationTokenSource.Dispose();
				m_cancellationTokenSource = null;
			}
		}
	}
}
