using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Domain.Contracts;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Models;
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
            const string sql = @"
                INSERT INTO merch_deliveries (merch_delivery_status_id, merch_pack_type_id, status_change_date, sku_ids)
                VALUES (@MerchDeliveryStatusId, @MerchPackTypeId, @StatusChangeDate, @SkuIds);";

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
                sql,
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
            const string sql = @"
                UPDATE merch_deliveries (merch_delivery_status_id, merch_pack_type_id, status_change_date, sku_ids)
                SET merch_delivery_status_id = @MerchDeliveryStatusId, 
                    merch_pack_type_id = @MerchPackTypeId, 
                    status_change_date = @StatusChangeDate
                    sku_ids = @SkuIds
                WHERE id = @MerchDeliveryId;";

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
                sql,
                parameters,
                commandTimeout: Timeout,
                cancellationToken: cancellationToken);
            
            var connection = await _dbConnectionFactory.CreateConnection(cancellationToken);
            await connection.ExecuteAsync(commandDefinition);
            _changeTracker.Track(itemToUpdate);
            return itemToUpdate;
        }

        public async Task<IEnumerable<MerchDelivery>?> GetByEmployeeIdAsync(int employeeId, 
            CancellationToken token = default)
        {
            const string sql = @"
                SELECT md.id, md.merch_pack_type_id, md.merch_delivery_status_id, md.status_change_date, md.sku_ids,
                    mpt.id, mpt.name
                FROM merch_deliveries md
                INNER JOIN employee_merch_delivery_maps emdm on md.id = emdm.merch_delivery_id
                LEFT JOIN merch_pack_types mpt ON md.merch_pack_type_id = mpt.id
                LEFT JOIN merch_delivery_statuses mds ON md.merch_delivery_status_id = mds.id
                WHERE emdm.employee_id = @EmployeeId
            ";

            var parameters = new
            {
                EmployeeId = employeeId
            };
            
            var commandDefinition = new CommandDefinition(
                sql,
                parameters,
                commandTimeout: Timeout,
                cancellationToken: token);

            var merchTypes = await GetMerchTypes(token);

            var connection = await _dbConnectionFactory.CreateConnection(token);
            return await connection
                .QueryAsync<Models.MerchDelivery, Models.MerchPackType, Models.MerchDeliveryStatus, MerchDelivery>(
                    commandDefinition,
                (delivery, type, status) => new MerchDelivery(
                        delivery.Id.Value,
                        new MerchPackType(type.Id.Value, 
                            type.Name, 
                            type
                                .MerchTypeIds
                                .Select(id => merchTypes[id])),
                        delivery
                            .SkuCollection
                            .Select(s => new Sku(s))
                            .ToArray(),
                        new MerchDeliveryStatus(status.Id.Value, status.Name)));
        }

        private async Task<Dictionary<int, MerchType>> GetMerchTypes(CancellationToken token)
        {
            const string sql = @"
                SELECT mt.id, mt.name
                FROM merch_types mt;
            ";
            
            var commandDefinition = new CommandDefinition(
                sql,
                commandTimeout: Timeout,
                cancellationToken: token);

            var connection = await _dbConnectionFactory.CreateConnection(token);
            var types = await connection.QueryAsync<Models.MerchType>(commandDefinition);
            return types
                .Select(t => new MerchType(t.Id.Value, t.Name))
                .ToDictionary(k => k.Id, v => v);
        }
        
        //
        // public async Task<List<MerchDelivery>> GetAll(CancellationToken token = default)
        // {
        //     throw new System.NotImplementedException();
        // }
        //
        // public async Task<List<MerchDelivery>> GetByStatus(int statusId, CancellationToken token = default)
        // {
        //     throw new System.NotImplementedException();
        // }
    }
}