using Fundamentals.DataStructures.Queues;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Fundamentals.Timing.TaskScheduling
{
    public class ScheduledTask : Task, IComparable<ScheduledTask>
    {
        public readonly DateTime DueTime;

        public ScheduledTask(DateTime dueTime, Action action) : base(action)
        {
            this.DueTime = dueTime;
        }

        public int CompareTo(ScheduledTask other)
        {
            return this.DueTime.CompareTo(other.DueTime);
        }
        public override string ToString()
        {
            return DueTime.ToString();
        }
    }
           
    public class TaskRunner<T> where T : Task
    {
        public TaskRunner()
        {
        }
        protected void SetStartOverride(Action startOverride)
        {
        }
        public virtual void AddNewTask(T task)
        {
            this.Start(task);
        }
        protected virtual void Start(T task)
        {
            task.Start();
        }
    }
    
    public class RateLimitedTaskRunner<T>: TaskRunner<T> where T:Task
    {
        private Queue<T> taskQueue = new Queue<T>();
        private System.Threading.Timer timer;
        private int interval;
        private bool enabled = true;
        private DateTime lastExecution = DateTime.MinValue;
        public RateLimitedTaskRunner(int interval)
        {
            this.interval = interval - 1;
            this.timer = new System.Threading.Timer(Elapsed, null, 0, interval);
        }

        private void Elapsed(object state)
        {
            if (this.taskQueue.Count == 0)
            {
                this.timer.Change(Timeout.Infinite, interval);
                this.enabled = false;                
            }
            else
            {
                lastExecution = DateTime.UtcNow;
                this.Start(this.taskQueue.Dequeue());
            }
        }

        public override void AddNewTask(T task)
        {
            taskQueue.Enqueue(task);
            if (!this.enabled)
            {
                this.enabled = true;
                var time = DateTime.UtcNow - this.lastExecution;
                if (time.TotalMilliseconds > this.interval)
                {
                    this.timer.Change(0, this.interval);
                }
                else
                {
                    this.timer.Change((int)(this.interval - time.TotalMilliseconds - 1), this.interval);
                }
            }
        }
    }

    public class RateLimitedSequentialTaskRunner<T> : SequentialTaskRunner<T> where T : Task
    {
        private RateLimitedTaskRunner<T> limitedRunner;
        public RateLimitedSequentialTaskRunner(int interval) : base()
        {
            this.limitedRunner = new RateLimitedTaskRunner<T>(interval);
        }

        protected override void Start(T task)
        {
            this.limitedRunner.AddNewTask(task);
        }
    }
    
    public class SequentialTaskRunner<T> : TaskRunner<T> where T:Task
    {
        Task currentExecutingTask;
        object taskLock = new object();

        public SequentialTaskRunner()
        {

        }

        public override void AddNewTask(T newTask)
        {
            if (newTask != null)
            {
                lock (this.taskLock)
                {
                    if (this.currentExecutingTask == null || this.currentExecutingTask.IsCompleted)
                    {
                        this.currentExecutingTask = newTask;
                        this.Start(newTask);
                    }
                    else
                    {
                        this.currentExecutingTask = this.currentExecutingTask.ContinueWith(
                            (task) =>
                            {
                                this.Start(newTask);
                            });
                    }
                }
            }
        }
    }

    public class ScheduledTaskRunner : TaskRunner<ScheduledTask>
    {
        protected IQueue<ScheduledTask> tasksToExecute;
        private object taskLock = new object();
        private CancelableTask currentNextTask;
        public ScheduledTaskRunner()
        {
            this.tasksToExecute = new SimpleBinaryHeap<ScheduledTask>(SimpleBinaryHeap.Min<ScheduledTask>());
        }

        private void OnNewTaskReady(ScheduledTask task)
        {
            this.Start(task);
            lock (taskLock)
            {
                while (this.tasksToExecute.Count() > 0 && this.tasksToExecute.Peek().DueTime <= DateTime.UtcNow)
                {
                    this.Start(this.tasksToExecute.Pop());
                }

                if (this.tasksToExecute.Count() > 0)
                {
                    var delay = this.tasksToExecute.Peek().DueTime - DateTime.UtcNow;
                    this.currentNextTask = new CancelableTask((token) =>
                    {        
                        if (delay > TimeSpan.Zero)
                        {
                            Thread.Sleep(delay);
                        }
                        if (!token.IsCancellationRequested)
                        {
                            lock (taskLock)
                            {
                                if (this.tasksToExecute.Peek().DueTime <= DateTime.UtcNow.AddMilliseconds(1))
                                {
                                    this.OnNewTaskReady(this.tasksToExecute.Pop());
                                }
                            }
                        }
                    });
                }
            }
        }

        public override void AddNewTask(ScheduledTask newTask)
        {
            var time = newTask.DueTime - DateTime.UtcNow;
            if (time > TimeSpan.FromMilliseconds(5))
            {
                lock (taskLock)
                {
                    tasksToExecute.Push(newTask);
                    if (newTask.DueTime <= tasksToExecute.Peek().DueTime)
                    {
                        this.currentNextTask?.Cancel();
                        this.currentNextTask = new CancelableTask(async (token) =>
                        {
                            await Task.Delay(time);
                            if (!token.IsCancellationRequested)
                            {
                                lock (taskLock)
                                {
                                    this.OnNewTaskReady(this.tasksToExecute.Pop());
                                }
                            }
                        });
                    }

                }
            }
            else
            {
                this.Start(newTask);
                return;
            }
        }

        public class CancelableTask : Task
        {
            public CancellationTokenSource cancellationTokenSource;

            public CancelableTask(Action<CancellationToken> action) : this(action, new CancellationTokenSource())
            {
                this.Start();
            }

            private CancelableTask(Action<CancellationToken> action, CancellationTokenSource source) : base(() => action(source.Token))
            {
                this.cancellationTokenSource = source;
            }
            public void Cancel()
            {
                this.cancellationTokenSource.Cancel();
            }
        }
    }    

}
