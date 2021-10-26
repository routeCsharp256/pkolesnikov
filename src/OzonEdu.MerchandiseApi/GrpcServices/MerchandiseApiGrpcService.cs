using System;
using System.Threading.Tasks;
using Grpc.Core;
using OzonEdu.MerchandiseApi.Grpc;
using OzonEdu.MerchandiseApi.Services.Interfaces;
#pragma warning disable 1591

namespace OzonEdu.MerchandiseApi.GrpcServices
{
    public class MerchandiseApiGrpcService : MerchandiseApiGrpc.MerchandiseApiGrpcBase
    {
        private readonly IMerchandiseService _merchService;

        public MerchandiseApiGrpcService(IMerchandiseService merchService)
        {
            _merchService = merchService;
        }

        public override Task<GetMerchResponse> GetMerch(GetMerchRequest request, ServerCallContext context)
        {
            throw new NotImplementedException();
        }

        public override Task<GetMerchIssuanceResponse> GetMerchIssuance(GetMerchIssuanceRequest request, 
            ServerCallContext context)
        {
            throw new NotImplementedException();
        }
    }
}