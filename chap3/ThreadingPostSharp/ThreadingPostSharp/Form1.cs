using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PostSharp.Aspects;

namespace ThreadingPostSharp
{
    public partial class Form1 : Form
    {
        TwitterService _twitter;

        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            _twitter = new TwitterService();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            GetNewTweet();
        }

        [WorkerThread]
        void GetNewTweet()
        {
            var tweet = _twitter.GetTweet();
            UpdateTweetListBox(tweet);
        }

        [UIThread]
        void UpdateTweetListBox(string tweet)
        {
            listTweets.Items.Add(tweet);
        }
    }

    [Serializable]
    public class WorkerThread : MethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var thread = new Thread(args.Proceed);
            thread.Start();
            //var task = new Task(args.Proceed);
            //task.Start();
        }
    }

    [Serializable]
    public class UIThread : MethodInterceptionAspect
    {
        public override void OnInvoke(MethodInterceptionArgs args)
        {
            var form = (Form) args.Instance;
            if (form.InvokeRequired)
                form.Invoke(new Action(args.Proceed));
            else
                args.Proceed();
        }
    }

    public class TwitterService
    {
        public string GetTweet()
        {
            Thread.Sleep(3000); // simulate slow web service
            return "Tweet from " + DateTime.Now.TimeOfDay;
        }
    }
}
