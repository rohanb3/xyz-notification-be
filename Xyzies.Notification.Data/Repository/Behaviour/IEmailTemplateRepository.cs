﻿using System;
using Xyzies.Notification.Data.Common.Interfaces;
using Xyzies.Notification.Data.Entity;

namespace Xyzies.Notification.Data.Repository.Behaviour
{
    public interface IEmailTemplateRepository: IRepository<Guid, EmailTemplate>
    {
    }
}
