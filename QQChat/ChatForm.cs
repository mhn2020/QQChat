﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebQQ2.WebQQ2;

namespace QQChat
{
    public partial class ChatForm : Form
    {
        public ChatForm()
        {
            InitializeComponent();
        }

        public QQ_Smart QQ { get; internal set; }
        private int item = 0;

        private void ChatForm_Load(object sender, EventArgs e)
        {
            this.listBox1.Click += (s, v) => { item = 0; };
            this.listBox2.Click += (s, v) => { item = 1; };
            Task.Factory.StartNew(() =>
            {
                QQ.MessageFriendReceived += QQ_MessageFriendReceived;
                QQ.MessageGroupReceived += QQ_MessageGroupReceived;
                QQ.GetUserList();
                QQ.GetUserOnlineList();
                QQ.GetGroupList();
                Task.Factory.StartNew(() =>
                {
                    foreach (var group in QQ.User.QQGroups.GroupList.Values.ToArray())
                    {
                        QQ.RefreshGroupInfo(group);
                    }
                });
                BeginInvoke((Action)RefreshList);
                foreach (var msg in QQ.DoMessageLoop())
                {
                }
            });
        }

        private void RefreshList()
        {
            this.listBox1.Items.Clear();
            this.listBox1.DisplayMember = "LongNameWithStatus";
            foreach (var f in QQ.User.QQFriends.FriendList)
            {
                this.listBox1.Items.Add(f.Value);
            }
            this.listBox2.Items.Clear();
            this.listBox2.DisplayMember = "LongName";
            foreach (var f in QQ.User.QQGroups.GroupList)
            {
                this.listBox2.Items.Add(f.Value);
            }
        }

        private void QQ_MessageGroupReceived(object sender, GroupEventArgs e)
        {
            this.AppendText(e.Group.ShortName + " -> " + e.Member.ShortName,e.MsgContent);
        }

        private void QQ_MessageFriendReceived(object sender, FriendEventArgs e)
        {
            this.AppendText(e.User.nick,e.MsgContent);
        }

        private void AppendText(string name,string text)
        {
            if(InvokeRequired)
            {
                BeginInvoke((Action<string,string>)AppendText,name,text);
                return;
            }
            this.richTextBox1.Select(this.richTextBox1.TextLength, 0);
            this.richTextBox1.SelectionColor = Color.Blue;
            this.richTextBox1.SelectedText = (name + System.Environment.NewLine);
            this.richTextBox1.Select(this.richTextBox1.TextLength, 0);
            this.richTextBox1.SelectionColor = Color.Black;
            this.richTextBox1.SelectedText = (text + System.Environment.NewLine);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch(item)
            {
                case 1:
                    {
                        var grp = listBox2.SelectedItem as QQGroup;
                        if (grp != null)
                        {
                            if (QQ.SendQunMessage(grp.gid, new QQ_Smart.StringContent { Content = richTextBox2.Text }))
                            {
                                richTextBox2.Clear();
                            }
                        }
                    }
                    break;
                default:
                    {
                        var frd = listBox1.SelectedItem as QQFriend;
                        if (frd != null)
                        {
                            if (QQ.SendBuddyMessage(frd.uin, new QQ_Smart.StringContent { Content = richTextBox2.Text }))
                            {
                                richTextBox2.Clear();
                            }
                        }
                    }
                    break;
            }
        }
    }
}