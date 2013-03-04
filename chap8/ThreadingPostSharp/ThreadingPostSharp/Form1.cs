using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PostSharp.Aspects;
using PostSharp.Extensibility;

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
        }
    }

    public class NotAWindowsForm
    {
        [UIThread]
        public void MyMethod()
        {
            
        }
    }

    [Serializable]
    public class UIThread : MethodInterceptionAspect
    {
        public override bool CompileTimeValidate(MethodBase method)
        {
            if (!typeof (Form).IsAssignableFrom(method.DeclaringType))
            {
                var errorMessage =
                    string.Format("UIThread aspect must be used in a Form. [Assembly: {0}, Class: {1}, Method: {2}]",
                    method.DeclaringType.Assembly.FullName,
                    method.DeclaringType.FullName,
                    method.Name);
                PostSharp.Extensibility
                    .Message.Write(method,
                    SeverityType.Error,
                    "UIThreadFormError01",
                    errorMessage);
                return false;
            }
            return true;
        }

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
