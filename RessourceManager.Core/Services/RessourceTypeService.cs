using MongoDB.Driver;
using RessourceManager.Core.Exceptions.RessourceType;
using RessourceManager.Core.Helpers;
using RessourceManager.Core.Models.V1;
using RessourceManager.Core.Repositories;
using RessourceManager.Core.Repositories.Interfaces;
using RessourceManager.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RessourceManager.Core.Services
{
    public class RessourceTypeService : IRessourceTypeService
    {
        private readonly IRessourceTypeRepository _ressourceTypeRepository;
        private readonly IErrorHandler _errorHandler;

        public RessourceTypeService(IRessourceTypeRepository ressourceTypeRepository, IErrorHandler errorHandler)
        {
            _ressourceTypeRepository = ressourceTypeRepository;
            _errorHandler = errorHandler;
        }

        public async Task<List<RessourceType>> Get()
        {
            var ressourceTypes = await _ressourceTypeRepository.GetAll();
            return ressourceTypes.ToList();
        }

        public async Task<RessourceType> Get(string id)
        {
            var ressourceType = await _ressourceTypeRepository.GetById(Guid.Parse(id));
            return ressourceType;
        }


        public async Task<List<RessourceType>> GetByType(int type)
        {
            var ressourceTypes = await _ressourceTypeRepository.GetByType(type);
            return ressourceTypes.ToList();
        }
        public async Task<RessourceType> Create(RessourceType ressourceTypeIn)
        {
            try
            {
                ressourceTypeIn.Count = 0; // Make sure the count is 0 at creation
                await _ressourceTypeRepository.Add(ressourceTypeIn);               
            }
            catch(MongoWriteException mwx)
            {
                if (mwx.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    var regex = @"/\{.*\:\{.*\:.*\}\}/g";
                    Match result = Regex.Match(mwx.Message, regex);

                    var field = mwx.Message.Split(".$")[1];
                    // now we have `email_1 dup key`
                    field = field.Split(" dup key")[0];
                    field = field.Substring(0, field.LastIndexOf('_')); // returns email

                    throw new RessourceTypeRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.DuplicateKey),
                       nameof(RessourceType), field));
                }
            }
            return ressourceTypeIn;
        }

        public void Update(RessourceType ressourceTypeIn) => _ressourceTypeRepository.Update(ressourceTypeIn);

        
        public void Remove(string id) =>
            _ressourceTypeRepository.Remove(Guid.Parse(id));
    }
}
