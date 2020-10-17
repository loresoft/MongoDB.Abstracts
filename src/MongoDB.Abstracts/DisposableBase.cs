﻿using System;
using System.Threading;

namespace MongoDB.Abstracts
{
    /// <summary>
    /// A base class that implements <see cref="IDisposable"/>
    /// </summary>
    public abstract class DisposableBase : IDisposable
    {
        private int _disposeState;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            // set state to disposing
            if (Interlocked.CompareExchange(ref _disposeState, 1, 0) != 0)
                return;

            if (disposing)
                DisposeManagedResources();

            DisposeUnmanagedResources();

            // set state to disposed
            Interlocked.Exchange(ref _disposeState, 2);
        }

        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> if this instance is disposed.
        /// </summary>
        protected void AssertDisposed()
        {
            if (_disposeState != 0)
                throw new ObjectDisposedException(GetType().FullName);
        }

        /// <summary>
        /// Get the disposed state of the object.
        /// </summary>
        protected bool IsDisposed
        {
            get { return _disposeState != 0; }
        }

        /// <summary>
        /// Disposes the managed resources.
        /// </summary>
        protected virtual void DisposeManagedResources()
        { }

        /// <summary>
        /// Disposes the unmanaged resources.
        /// </summary>
        protected virtual void DisposeUnmanagedResources()
        { }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="DisposableBase"/> is reclaimed by garbage collection.
        /// </summary>
        ~DisposableBase()
        {
            Dispose(false);
        }
    }

}