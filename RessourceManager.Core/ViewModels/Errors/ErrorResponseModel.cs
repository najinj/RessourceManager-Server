﻿using Newtonsoft.Json;

namespace RessourceManager.Core.ViewModels.Errors
{
    public  class ErrorResponseModel
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}