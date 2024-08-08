using Big.Emtelco.EmtelcoPoints.Common.Models;
using FPV.API.Core.Repositories.GenericWorker.Interfaces;
using FPV.Common.Helper.Diagnostics;
using FPV.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace FPV.API.Controllers.Base
{

    [Route("api/[controller]")]
    [Produces("application/json")]
    public class GenericBaseController<T, TEntityId> : Controller where T : class
    {
        private readonly IGenericWorker _iGenericWorker;

        public GenericBaseController(IGenericWorker iGenericWorker)
        {
            _iGenericWorker = iGenericWorker;
        }

        [HttpGet]
        public async Task<Response<List<T>>> Get()
        {
            try
            {
                var list = await _iGenericWorker.Repository<T>().GetAll();
                return new Response<List<T>>() { Status = true, ObjectResponse = list.ToList() };
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return new Response<List<T>>() { Status = false, message = new List<MessageResult>() { new MessageResult(ex.Message) } };
            }
        }

        [HttpGet("{id}")]
        public async Task<Response<T>> Get(TEntityId id)
        {
            try
            {
                var list = await _iGenericWorker.Repository<T>().GetById(id);
                return new Response<T>() { Status = true, ObjectResponse = list };
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return new Response<T>() { Status = false, message = new List<MessageResult>() { new MessageResult(ex.Message) } };
            }
        }

        [HttpPost()]
        public async Task<Response<bool>> Post([FromBody] T value)
        {
            try
            {
                var list = await _iGenericWorker.Repository<T>().Add(value);
                bool save = list != null;
                return new Response<bool>() { Status = save, ObjectResponse = save };
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return new Response<bool>() { Status = false, message = new List<MessageResult>() { new MessageResult(ex.Message) } };
            }
        }

        [HttpPut]
        public async Task<Response<bool>> Put([FromBody] T value)
        {
            try
            {
                var list = await _iGenericWorker.Repository<T>().Update(value);
                bool save = list != null;
                return new Response<bool>() { Status = save, ObjectResponse = save };
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return new Response<bool>() { Status = false, message = new List<MessageResult>() { new MessageResult(ex.Message) } };
            }
        }

        [HttpDelete]
        public async Task<Response<bool>> Delete([FromBody] T value)
        {
            try
            {
                var list = await _iGenericWorker.Repository<T>().Delete(value);
                bool save = list != null;
                return new Response<bool>() { Status = save, ObjectResponse = save };
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return new Response<bool>() { Status = false, message = new List<MessageResult>() { new MessageResult(ex.Message) } };
            }
        }

        [HttpDelete("{id}")]
        public async Task<Response<bool>> Delete(TEntityId id)
        {
            try
            {
                var list = await _iGenericWorker.Repository<T>().DeleteById(id);
                bool save = list != null;
                return new Response<bool>() { Status = save, ObjectResponse = save };
            }
            catch (Exception ex)
            {
                ExceptionLogging.LogException(ex);
                return new Response<bool>() { Status = false, message = new List<MessageResult>() { new MessageResult(ex.Message) } };
            }
        }

    }
}
