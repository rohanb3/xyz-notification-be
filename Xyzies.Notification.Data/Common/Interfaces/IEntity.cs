﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Xyzies.Notification.Data.Common.Interfaces
{
    /// <summary>
    /// Entity requirements
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IEntity<TKey>
        where TKey : IEquatable<TKey>, IComparable<TKey>, IComparable
    {
        /// <summary>
        /// ID of entity
        /// </summary>
        TKey Id { get; set; }

        /// <summary>
        /// Equals by key (ID)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool EqualsByKey(TKey key);
    }
}
