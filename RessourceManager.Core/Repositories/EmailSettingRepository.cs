

using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Infrastructure.Context;

namespace RessourceManager.Core.Repositories
{
    public class EmailSettingRepository : Repository<EmailSettings> , IEmailSettingRepository
    {
        public EmailSettingRepository(IMongoContext context) : base(context)
        {

        }
    }
}
