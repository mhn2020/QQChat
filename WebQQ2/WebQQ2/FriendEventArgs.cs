﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebQQ2.WebQQ2
{
    public class FriendEventArgs : EventArgs
    {
        private QQFriend _user = null;
        private long _msg_id = 0;
        private DateTime _time;
        private MessageEventType _mtype = MessageEventType.MESSAGE_UNKNOW;

        public MessageEventType Mtype
        {
            get { return _mtype; }
        }

        public long Msg_id
        {
            get { return _msg_id; }
        }

        private Dictionary<string, object> _msgs = null;

        public QQFriend User
        {
            get { return _user; }
        }

        public DateTime Time
        {
            get { return _time; }
        }

        public Dictionary<string, object> Msgs
        {
            get { return _msgs; }
        }

        public string MsgContent
        {
            get
            {
                if (_msgs.Count == 1 && _msgs.Keys.Contains("content"))
                {
                    ArrayList content = _msgs["content"] as ArrayList;
                    if (content == null)
                        return null;
                    var contentList = new List<string>(content.Count - 1);
                    for (int i = 1; i < content.Count; i++)
                    {
                        contentList.Add(GetSimpleMsg(content[i]));
                    }
                    return string.Join(System.Environment.NewLine, contentList);
                }
                return null;
            }
        }
        public string[] MsgContents
        {
            get
            {
                var contentList = new List<string>();
                if (_msgs.Count == 1 && _msgs.Keys.Contains("content"))
                {
                    ArrayList content = _msgs["content"] as ArrayList;
                    if (content == null)
                        return null;
                    for (int i = 1; i < content.Count; i++)
                    {
                        contentList.Add(GetSimpleMsg(content[i]));
                    }
                }
                return contentList.ToArray();
            }
        }

        private string GetSimpleMsg(object o)
        {
            if (o is Dictionary<string, object>)
            {
                StringBuilder sb = new StringBuilder();
                foreach (KeyValuePair<string, object> p in o as Dictionary<string, object>)
                {
                    sb.AppendLine(string.Format("{0}:{1}", p.Key, GetSimpleMsg(p.Value)));
                }
                return sb.ToString().TrimEnd();
            }
            if (o is ArrayList)
            {
                StringBuilder sb = new StringBuilder("[");
                foreach (object p in o as ArrayList)
                {
                    sb.Append(GetSimpleMsg(p));
                    sb.Append(",");
                }
                if (sb.Length > 1)
                {
                    sb.Length--;
                }
                sb.Append("]");
                return sb.ToString().TrimEnd();
            }
            return o.ToString();
        }

        internal FriendEventArgs(QQFriend user, long msgid, DateTime time, MessageEventType mtype, Dictionary<string, object> msgs)
        {
            this._user = user;
            this._msg_id = msgid;
            this._time = time;
            this._mtype = mtype;
            this._msgs = msgs;
        }

        internal FriendEventArgs(QQFriend user, long msgid, DateTime time, MessageEventType mtype, string singleMsg)
        {
            this._user = user;
            this._msg_id = msgid;
            this._time = time;
            this._mtype = mtype;
            this._msgs = new Dictionary<string, object>() { { "message", singleMsg } };
        }

        internal FriendEventArgs(QQFriend user, long msgid, DateTime time, MessageEventType mtype)
        {
            this._user = user;
            this._msg_id = msgid;
            this._time = time;
            this._mtype = mtype;
            this._msgs = new Dictionary<string, object>();
        }
    }

}
