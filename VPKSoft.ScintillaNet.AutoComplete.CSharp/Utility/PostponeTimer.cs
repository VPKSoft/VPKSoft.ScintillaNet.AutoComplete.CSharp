#region License
/*
MIT License

Copyright(c) 2020 Petteri Kautonen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion

using System;
using System.Threading;

namespace VPKSoft.ScintillaNet.AutoComplete.CSharp.Utility
{
    /// <summary>
    /// A timer which can be postponed by a specified value.
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class PostponeTimer: IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostponeTimer"/> class.
        /// </summary>
        public PostponeTimer()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostponeTimer"/> class.
        /// </summary>
        /// <param name="interval">The interval to be used with this timer.</param>
        public PostponeTimer(int interval) : this(interval, 10)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PostponeTimer"/> class.
        /// </summary>
        /// <param name="interval">The interval to be used with this timer.</param>
        /// <param name="resolution">The resolution to be used with this timer.</param>
        public PostponeTimer(int interval, int resolution)
        {
            this.interval = interval;
            this.resolution = resolution;
            TimerThread = new Thread(TimerThreadMethod);
            TimerThread.Start();
        }

        /// <summary>
        /// Gets the timer thread.
        /// </summary>
        /// <value>The timer thread.</value>
        private Thread TimerThread { get; }

        /// <summary>
        /// The <see cref="PostponeTimer"/> thread method.
        /// </summary>
        private void TimerThreadMethod()
        {
            #pragma warning disable IDE0054 // Reason: Suspicious 'volatile' field usage: compound operation is not atomic. 'Interlocked' class can be used instead.
            try
            {
                while (!disposed)
                {
                    // ReSharper disable once ConvertToCompoundAssignment, Reason: Suspicious 'volatile' field usage: compound operation is not atomic. 'Interlocked' class can be used instead.
                    spentTime = spentTime + resolution;
                    if (spentTime > interval)
                    {
                        spentTime = 0;
                        if (enabled)
                        {
                            var dt = DateTime.Now;
                            Timer?.Invoke(this, new PostponeTimerEventArgs {Interval = interval});
                            if (addEventTime)
                            {
                                // ReSharper disable once ConvertToCompoundAssignment, Reason: Suspicious 'volatile' field usage: compound operation is not atomic. 'Interlocked' class can be used instead.
                                spentTime = spentTime + (int) (dt - DateTime.Now).TotalMilliseconds;
                            }
                        }
                    }

                    Thread.Sleep(resolution);
                }
            }
            catch
            {
                // ignored..
            }
#pragma warning restore IDE0054 // Reason: Suspicious 'volatile' field usage: compound operation is not atomic. 'Interlocked' class can be used instead.
        }

        /// <summary>
        /// Postpones the timer with the specified amount in milliseconds.
        /// </summary>
        /// <param name="value">The amount in milliseconds to postpone the timer.</param>
        public void Postpone(int value)
        {
            SpentTime = spentTime - value < 0 ? 0 : spentTime - value;
        }

        /// <summary>
        /// Gets or sets the interval of this <see cref="PostponeTimer"/> instance.
        /// </summary>
        /// <value>The interval of this <see cref="PostponeTimer"/> instance.</value>
        public int Interval { get => interval; set => interval = value; }

        /// <summary>
        /// Gets or sets the spent time (the time before the ).
        /// </summary>
        /// <value>The spent time.</value>
        public int SpentTime { get => spentTime; set => spentTime = value; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PostponeTimer"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled {get => enabled; set => enabled = value; }

        /// <summary>
        /// Gets or sets the resolution of this timer.
        /// </summary>
        /// <value>The resolution of this timer.</value>
        public int Resolution { get => resolution; set => resolution = value; }

        /// <summary>
        /// Gets or sets a value indicating whether to add the time spent in the event to the timer's <see cref="SpentTime"/> value.
        /// </summary>
        /// <value><c>true</c> if to add the time spent in the event to the timer's <see cref="SpentTime"/> value.; otherwise, <c>false</c>.</value>
        public bool AddEventTime { get => addEventTime; set => addEventTime = value; }

        private volatile int interval = 1000;
        private volatile int spentTime;
        private volatile bool enabled;
        private volatile bool disposed;
        private volatile int resolution = 10;
        private volatile bool addEventTime;

        /// <summary>
        /// An event which occurs when the timer interval has been spent.
        /// </summary>
        internal EventHandler<PostponeTimerEventArgs> Timer { get; set; }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                // free managed resources
                disposed = true;
                if (!TimerThread.Join(3000))
                {
                    TimerThread.Abort();
                }
            }

            disposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="PostponeTimer"/> class.
        /// </summary>
        ~PostponeTimer()
        {
            Dispose(false);
        }
    }
}
