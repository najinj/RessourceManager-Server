using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RessourceManager.Core.Exceptions.RessourceType;
using RessourceManager.Core.Helpers;
using RessourceManager.Core.Models.V1;
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
            var ressourceType = await _ressourceTypeRepository.GetById(id);
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
                await _ressourceTypeRepository.Add(ressourceTypeIn);               
            }
            catch(MongoWriteException mwx)
            {
                if (mwx.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    var pattern = @"\{(?:[^{*}])*\}";
                    Match result = Regex.Match(mwx.Message, pattern);  // get the dublicated feild from the string error msg 
                    JObject duplicatedField = JsonConvert.DeserializeObject<JObject>(result.Value); // parse it  to get the field 
                    throw new RessourceTypeRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.DuplicateKey),
                       nameof(RessourceType), duplicatedField.First.Path), duplicatedField.First.Path);
                }
            }
            return ressourceTypeIn;
        }

        public async Task<RessourceType> Update(RessourceType ressourceTypeIn) {
            try
            {
                await _ressourceTypeRepository.Update(ressourceTypeIn);
            }
            catch (MongoWriteException mwx)
            {
                if (mwx.WriteError.Category == ServerErrorCategory.DuplicateKey)
                {
                    var pattern = @"\{(?:[^{*}])*\}";
                    Match result = Regex.Match(mwx.Message, pattern);  // get the dublicated feild from the string error msg 
                    JObject duplicatedField = JsonConvert.DeserializeObject<JObject>(result.Value); // parse it  to get the field 
                    throw new RessourceTypeRepositoryException(string.Format(_errorHandler.GetMessage(ErrorMessagesEnum.DuplicateKey),
                       nameof(RessourceType), duplicatedField.First.Path), duplicatedField.First.Path);
                }
            }
            return ressourceTypeIn;
        } 

        
        public void Remove(string id) =>
            _ressourceTypeRepository.Remove(id);
    }
}
