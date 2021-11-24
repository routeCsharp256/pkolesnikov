using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Queries;
using MerchDelivery = OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate.MerchDelivery;
using MerchDeliveryStatus = OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate.MerchDeliveryStatus;
using MerchPackType = OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate.MerchPackType;
using MerchType = OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate.MerchType;

#pragma warning disable 1998

namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Implementation
{
    public class MerchDeliveryRepository : IMerchDeliveryRepository
    {
        private const int Timeout = 5;
        
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;
        
        public MerchDeliveryRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory,
            IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
        }

        public async Task<MerchDelivery?> CreateAsync(MerchDelivery itemToCreate, CancellationToken token)
        {
            var parameters = new
            {
                MerchDeliveryStatusId = itemToCreate.Status.Id,
                MerchPackTypeId = itemToCreate.MerchPackType.Id,
                StatusChangeDate = itemToCreate.StatusChangeDate.Value,
                SkuIds = itemToCreate
                    .SkuCollection
                    .Select(s => s.Value)
                    .ToArray()
            };

            var commandDefinition = new CommandDefinition(
                MerchDeliveryQuery.Insert,
                parameters,
                commandTimeout: Timeout,
                cancellationToken: token);

            var connection = await _dbConnectionFactory.CreateConnection(token);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(itemToCreate);
            return itemToCreate;
        }

        public async Task<MerchDelivery?> UpdateAsync(MerchDelivery itemToUpdate, CancellationToken cancellationToken = default)
        {
            var parameters = new
            {
                MerchDeliveryId = itemToUpdate.Id,
                MerchDeliveryStatusId = itemToUpdate.Status.Id,
                MerchPackTypeId = itemToUpdate.MerchPackType.Id,
                StatusChangeDate = itemToUpdate.StatusChangeDate.Value,
                SkuIds = itemToUpdate
                    .SkuCollection
                    .Select(s => s.Value)
                    .ToArray()
            };
            
            var commandDefinition = new CommandDefinition(
                MerchDeliveryQuery.Update,
                parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(itemToUpdate);
            return itemToUpdate;
        }

        public async Task<IEnumerable<MerchDelivery>?> GetAsync(int employeeId, 
            CancellationToken token = default)
        {
            var parameters = new
            {
                EmployeeId = employeeId
            };
            
            var commandDefinition = new CommandDefinition(
                MerchDeliveryQuery.FilterByEmployeeId,
                parameters,
                commandTimeout: Timeout,
                cancellationToken: token);

            var merchTypes = await GetMerchTypes(token);

            var connection = await _dbConnectionFactory.CreateConnection(token);
            
            return await connection
                .QueryAsync<Models.MerchDelivery, Models.MerchPackType, Models.MerchDeliveryStatus, MerchDelivery>(
                    commandDefinition,
                (delivery, type, status) =>
                    {
                        if (delivery is null || type is null || status is null)
                            return null;

                        return new MerchDelivery(
                                delivery.MerchDeliveryId,
                                new MerchPackType(type.Id,
                                    type.Name,
                                    type
                                        .MerchTypeIds
                                        .Select(id => merchTypes[id])),
                                delivery
                                    .SkuCollection
                                    .Select(s => new Sku(s))
                                    .ToArray(),
                                new MerchDeliveryStatus(status.Id.Value, status.Name));
                    });
        }

        public async Task<MerchDeliveryStatus?> FindStatus(int employeeId, int merchPackTypeId, CancellationToken token)
        {
            var parameters = new
            {
                EmployeeId = employeeId,
                MerchPackTypeId = merchPackTypeId
            };
            
            var commandDefinition = new CommandDefinition(
                MerchDeliveryQuery.FindMerchDeliveryStatusByEmployeeIdAndMerchPackTypeId,
                parameters,
                commandTimeout: Timeout,
                cancellationToken: token);

            var connection = await _dbConnectionFactory.CreateConnection(token);
            
            var dbResult = await connection
                .QueryFirstAsync<Models.MerchDeliveryStatus>(commandDefinition);
            return dbResult is null
                ? null
                : new MerchDeliveryStatus(dbResult.Id.Value, dbResult.Name);
        }

        private async Task<Dictionary<int, MerchType>> GetMerchTypes(CancellationToken token)
        {
            var commandDefinition = new CommandDefinition(
                MerchDeliveryQuery.GetMerchTypes,
                commandTimeout: Timeout,
                cancellationToken: token);

            var connection = await _dbConnectionFactory.CreateConnection(token);
            var types = await connection.QueryAsync<Models.MerchType>(commandDefinition);
            return types
                .Select(t => new MerchType(t.Id.Value, t.Name))
                .ToDictionary(k => k.Id, v => v);
        }
    }
}