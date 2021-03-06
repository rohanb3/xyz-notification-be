﻿using System;

namespace Xyzies.Notification.Data.Common.Interfaces
{
    public interface IAccessPointProvider<TProvider> : IDisposable where TProvider : class, IDisposable
    {
        /// <summary>
        /// Data access provider
        /// </summary>
        TProvider Provider { get; }
    }
}
