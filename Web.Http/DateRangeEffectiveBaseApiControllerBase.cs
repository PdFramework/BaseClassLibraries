namespace PeinearyDevelopment.Framework.BaseClassLibraries.Web.Http
{
    using Contracts;
    using DataAccess.Contracts;

    using AutoMapper;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core;
    using System.Threading.Tasks;
    using System.Web.Http;

    public class DateRangeEffectiveBaseApiControllerBase<TContract, TDto, TId> : IdBaseApiControllerBase<TContract, TDto, TId> where TContract : DateRangeEffectiveContractBase<TId>
                                                                                                                               where TDto : DateRangeEffectiveDtoBase<TId>
    {
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public DateRangeEffectiveBaseApiControllerBase(IDalBase<TDto, TId> dal, IMapper mapper, IContractValidator<TContract> contractValidator) : base(dal, mapper, contractValidator)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
        }

        public override async Task<IHttpActionResult> Post(TContract contract)
        {
            contract.EffectiveStartDate = contract.EffectiveStartDate == DateTimeOffset.MinValue ? DateTimeOffset.UtcNow : contract.EffectiveStartDate;

            return await Post(contract, ValidateEffectiveDateRange(contract));
        }

        public override async Task<IHttpActionResult> Put(TContract contract)
        {
            return await Put(contract, ValidateEffectiveDateRange(contract));
        }

        public virtual async Task<IHttpActionResult> SoftDelete(TId id)
        {
            try
            {
                var dto = await Dal.Read(id);
                dto.LastUpdatedByUserId = IdPrincipal.Id;
                dto.LastUpdatedOn = DateTimeOffset.UtcNow;
                dto.EffectiveEndDate = DateTimeOffset.UtcNow;
                await Dal.Update(dto);

                return Ok();
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
        }

        private static IEnumerable<string> ValidateEffectiveDateRange(TContract contract)
        {
            return contract.EffectiveEndDate != null && contract.EffectiveEndDate <= contract.EffectiveStartDate ? new[] { ErrorMessages.InvalidEffectiveDateRange } : new string[0];
        }
    }
}
