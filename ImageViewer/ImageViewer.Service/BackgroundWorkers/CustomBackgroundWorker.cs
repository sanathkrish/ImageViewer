using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Service.BackgroundWorkers
{
    public class CustomBackgroundWorker<T> : Task
    {

        public delegate void DoWork<T>(T input);
        public delegate T Interceptor();
        public delegate void Notify<T>(T outpute);


        public DoWork<T> Action { get; set; }
        public Interceptor IAction { get; set; }
        public Notify<T> NAction { get; set; }
        protected bool IsRunning { get; set; }
        public CustomBackgroundWorker() : base(() => { })
        {
        }

        //public CustomBackgroundWorker(DoWork<T> doWorkAction, Interceptor<T> interceptorAction, Notify<T> notifyAction, T input) : base(() => { })
        //{
        //    DoWorkAction = doWorkAction;
        //    InterceptorAction = interceptorAction;
        //    NotifyAction = notifyAction;
        //}


        public virtual void DoWorkAction(T input)
        {
            
        }

        public virtual T InterceptorAction()
        {
            return default(T);
        }

        public virtual void NotifyAction(T output)
        {
        }

        public virtual void Initialize()
        {
            if (!IsRunning)
            {
            Action = DoWorkAction;
            IAction = InterceptorAction;
            NAction = NotifyAction;
            }
        }

        public void Run(T input)
        {
            if (!IsRunning)
            {
                Task.Run(() =>
                {
                    Action?.Invoke(input);
                });
            }
        }

    }
}
