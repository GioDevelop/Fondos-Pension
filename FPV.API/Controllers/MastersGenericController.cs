using FPV.API.Controllers.Base;
using FPV.API.Core.Context;
using FPV.API.Core.Repositories.GenericWorker.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace FPV.API.Controllers
{
    public class MastersGenericController : Controller
    {
        [ApiController]
        public class MailTypeController : GenericBaseController<TransactionType, int>
        {
            private readonly IGenericWorker _iGenericWorker;

            public MailTypeController(IGenericWorker iGenericWorker) : base(iGenericWorker)
            {
                _iGenericWorker = iGenericWorker;
            }
        }
    }
}
