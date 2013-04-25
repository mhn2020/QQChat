﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MeIn
{
    public partial class setting : Form
    {
        public setting()
        {
            InitializeComponent();
        }

        internal iniItem SaveItem;

        private void button1_Click(object sender, EventArgs e)
        {
            Int32 min, max, span,top;
            if (!Int32.TryParse(textBoxMin.Text, out min))
            {
                MessageBox.Show("请输入有效最小值");
                textBoxMin.Focus();
                return;
            }
            if (!Int32.TryParse(textBoxMax.Text, out max))
            {
                MessageBox.Show("请输入有效最大值");
                textBoxMax.Focus();
                return;
            }
            if (max < 0 || min > max)
            {
                MessageBox.Show("请输入有效数值范围");
                textBoxMin.Focus();
                return;
            }
            if (!Int32.TryParse(textBoxSpan.Text, out span))
            {
                MessageBox.Show("请输入有效时间间隔");
                textBoxSpan.Focus();
                return;
            }
            if (span < 0)
            {
                MessageBox.Show("请输入有效时间间隔");
                textBoxSpan.Focus();
                return;
            }
            if (!Int32.TryParse(textBoxTop.Text, out top))
            {
                MessageBox.Show("请输入有效排名个数");
                textBoxTop.Focus();
                return;
            }
            if (top < 1)
            {
                MessageBox.Show("请输入正整数");
                textBoxTop.Focus();
                return;
            }
            SaveItem.min = min;
            SaveItem.mintomax = max - min;
            SaveItem.top = top;
            SaveItem.timespan = new TimeSpan(TimeSpan.TicksPerMinute * span);
            SaveItem.item = textBoxName.Text;
            SaveItem.pdata = richTextBox1.Text;
            SaveItem.autoIn = checkBox1.Checked;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Reset()
        {
            textBoxMin.Text = SaveItem.min.ToString();
            textBoxMax.Text = (SaveItem.min + SaveItem.mintomax).ToString();
            textBoxSpan.Text = ((Int32)SaveItem.timespan.TotalMinutes).ToString();
            textBoxTop.Text = SaveItem.top.ToString();
            textBoxName.Text = SaveItem.item;
            richTextBox1.Text = SaveItem.pdata;
            checkBox1.Checked = SaveItem.autoIn;
            this.DialogResult = DialogResult.None;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void setting_Shown(object sender, EventArgs e)
        {
            Reset();
        }

        private void setting_Load(object sender, EventArgs e)
        {

        }
    }
}
