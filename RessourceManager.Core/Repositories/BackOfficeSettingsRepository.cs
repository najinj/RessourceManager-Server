﻿using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Infrastructure.Context;

namespace RessourceManager.Core.Repositories
{
    public class BackOfficeSettingsRepository : Repository<BackOfficeSettings>, IBackOfficeSettingsRepository
    {
        public BackOfficeSettingsRepository(IMongoContext context) : base(context)
        {

        }
    }
}
