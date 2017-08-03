using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using SuperSocket.Common;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketEngine;
using SuperSocket.SocketBase.Logging;
using SuperWebSocket;
using log4net;
namespace Server
{
    public partial class FrmMain : Form
    {
        WebSocketServer  appServer = new WebSocketServer();
        ServerConfig  serverConfig = new ServerConfig
            {
                Port = 8080,//set the listening port
                MaxConnectionNumber = 10000
            };
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {        
          if (!appServer.Setup(serverConfig)) //Setup the appServer
            {
                System.Windows.Forms.MessageBox.Show("开启服务器失败");
                return;
            }

            if (!appServer.Start())//Try to start the appServer
            {
                System.Windows.Forms.MessageBox.Show("开启服务器失败");
                return;
            }
            //注册事件
            appServer.NewSessionConnected += appServer_NewSessionConnected;//客户端连接
            appServer.NewMessageReceived += appServer_NewMessageReceived;//客户端接收消息
            appServer.SessionClosed += appServer_SessionClosed;//客户端关闭
        }
        void appServer_NewSessionConnected(WebSocketSession session)
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;  //屏蔽线程间空间操作报错问题，也可以选择委托方法解决

            session.Send("连接成功");
            listBox1.Items.Add("Host:"+session.Host);//服务器的ip
            listBox1.Items.Add("Uri:"+session.UriScheme);
            listBox1.Items.Add("Path:"+session.Path);
            listBox1.Items.Add("CurrentToken:" + session.CurrentToken);
            listBox1.Items.Add("SessionID:" + session.SessionID);
            listBox1.Items.Add("Connection" + session.Connection);
            listBox1.Items.Add("Origin"+session.Origin);
            listBox1.Items.Add("LocalEndPoint"+session.LocalEndPoint);
            listBox1.Items.Add("RemoteEndPoint" + session.RemoteEndPoint);
            
        }
        void appServer_NewMessageReceived(WebSocketSession session, string value)
        {
            session.Send("服务端收到了客户端发来的消息");
            this.listBox1.Items.Add(value);

        }
        void appServer_SessionClosed(WebSocketSession session, SuperSocket.SocketBase.CloseReason value)
        {
            this.listBox1.Items.Add(value.ToString());
            session.Close();
        }
    }
}
