using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Util;

namespace Assets.UI
{
    /// <summary>
    /// Manages the way notifications will be displayed in sequential order.
    /// </summary>
    public static class NotificationManager
    {
        private static Queue<string> Queue = new Queue<string>();

        private static Notification Notification { get; set; }

        public static void Init(Notification notification)
        {
            notification.Init();
            Notification = notification;
        }

        public static void AddNotification(string message)
        {
            Queue.Enqueue(message);
        }

        public static void RunFrame(float deltaTime)
        {
            if (Queue.Any() && Notification.AnimationFinished)
            {
                string message = Queue.Dequeue();
                Notification.Activate(message);
            }
        }
    }
}
