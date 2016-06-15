namespace PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http
{
    using Contracts;
    using DataAccess.Contracts;

    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class IdBaseApiControllerBase<TContract, TDto, TId> : SecurityApiControllerBase where TContract : IdContractBase<TId>
                                                                                           where TDto : IdDtoBase<TId>
    {
        internal IDalBase<TDto, TId> Dal { get; }
        internal IContractValidator<TContract> ContractValidator { get; }
        internal IMapper Mapper { get; }
        protected internal string CreatedRouteName { get; set; }
        protected internal string ControllerName { get; set; }

#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public IdBaseApiControllerBase(IDalBase<TDto, TId> dal, IMapper mapper, IContractValidator<TContract> contractValidator)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            Dal = dal;
            Mapper = mapper;
            ContractValidator = contractValidator;
        }

        public virtual async Task<IHttpActionResult> Get(TId id)
        {
            try
            {
                return Ok(Mapper.Map<TContract>(await Dal.Read(id)));
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }

        public virtual async Task<IHttpActionResult> Post(TContract contract)
        {
            return await Post(contract, null);
        }

        public async Task<IHttpActionResult> Post(TContract contract, IEnumerable<string> validationErrors)
        {
            if(validationErrors == null) validationErrors = new List<string>();
            validationErrors = validationErrors.Concat(await ContractValidator.ValidateContract(contract));
            if (validationErrors.Any()) return BadRequest(string.Join("\n", validationErrors));

            contract.CreatedByUserId = IdPrincipal.Id;
            contract.CreatedOn = DateTimeOffset.UtcNow;
            contract.LastUpdatedByUserId = null;
            contract.LastUpdatedOn = null;

            var createdTDto = await Dal.Create(Mapper.Map<TDto>(contract));
            var createdT = Mapper.Map<TContract>(createdTDto);
            return Created(Url.Link(CreatedRouteName, new { id = createdT.Id, controller = ControllerName }), createdT);
        }

        public virtual async Task<IHttpActionResult> Put(TContract contract)
        {
            return await Put(contract, null);
        }

        public async Task<IHttpActionResult> Put(TContract contract, IEnumerable<string> validationErrors)
        {
            if (validationErrors == null) validationErrors = new List<string>();
            validationErrors = validationErrors.Concat(await ContractValidator.ValidateContract(contract));
            if (validationErrors.Any()) return BadRequest(string.Join("\n", validationErrors));

            try
            {
                contract.LastUpdatedByUserId = IdPrincipal.Id;
                contract.LastUpdatedOn = DateTimeOffset.UtcNow;
                var updatedTDto = await Dal.Update(Mapper.Map<TDto>(contract));
                var updatedT = Mapper.Map<TContract>(updatedTDto);
                return Ok(updatedT);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }

        public virtual async Task<IHttpActionResult> Delete(TId id)
        {
            try
            {
                await Dal.Delete(id);
                return this.NoContent();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
