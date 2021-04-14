﻿namespace EstateManagement.Common.Examples
{
    using DataTransferObjects;
    using DataTransferObjects.Responses;
    using Swashbuckle.AspNetCore.Filters;

    public class ContractProductTransactionFeeResponseExample : IExamplesProvider<ContractProductTransactionFee>
    {
        public ContractProductTransactionFee GetExamples()
        {
            return new ContractProductTransactionFee
                   {
                       Value = 0.05m,
                       Description = ExampleData.MerchantFixedFeeDescription,
                       TransactionFeeId = ExampleData.TransactionFeeId,
                       CalculationType = CalculationType.Fixed,
                       FeeType = FeeType.Merchant
                   };
        }
    }
}