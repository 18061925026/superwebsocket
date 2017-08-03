using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WebSocket4Net;
using System.Threading;

namespace Client
{
    public partial class CFrmMain : Form
    {
        WebSocket websocket ;
            
        public CFrmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void websocket_MessageReceived(object sender, MessageReceivedEventArgs e)
        {            
            this.listBox1.Invoke(new EventHandler(ShowMessage), e.Message);

        }

        private void ShowMessage(object sender, EventArgs e)
        {           
            this.listBox1.Items.Add(sender.ToString());
        }

        private void websocket_Closed(object sender, EventArgs e)
        {
            websocket.Send("一个客户端 下线");
        }

         void websocket_Opened(object sender, EventArgs e)
        {
            websocket.Send("一个客户端 上线");            
        }

         private void button1_Click(object sender, EventArgs e)
         {
             websocket = new WebSocket("ws://" + textBox1.Text + ":" + textBox2.Text);
             websocket.Opened += websocket_Opened;
             websocket.Closed += websocket_Closed;
             websocket.MessageReceived += websocket_MessageReceived;
             websocket.Open();
         }

         private void button2_Click(object sender, EventArgs e)
         {
             websocket.Send(textBox1.Text + ":" + textBox2.Text + "发送：" + textBox3.Text);
         }
    }
}
