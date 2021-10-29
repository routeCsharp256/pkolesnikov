using System.Threading.Tasks;
using Grpc.Core;
using OzonEdu.MerchandiseApi.Grpc;

namespace OzonEdu.MerchandiseApi.GrpcServices
{
    public class MerchandiseApiGrpcService : MerchandiseApiGrpc.MerchandiseApiGrpcBase
    {
        public MerchandiseApiGrpcService()
        {
        }

        public override async Task<GetMerchResponse> GetMerch(GetMerchRequest request, ServerCallContext context)
        {
            return await Task.Run(() 
                => new GetMerchResponse{ Description = "Id = " + request.Id}, context.CancellationToken);
        }

        public override async Task<GetMerchIssuanceResponse> GetMerchIssuance(GetMerchIssuanceRequest request, 
            ServerCallContext context)
        {
            return await Task.Run(() 
                => new GetMerchIssuanceResponse{ Description = "Id = " + request.Id}, context.CancellationToken);
        }
    }
}