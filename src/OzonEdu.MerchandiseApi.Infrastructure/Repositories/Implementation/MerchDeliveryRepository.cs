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
                INSERT INTO merch_deliveries (merch_delivery_status_id, merch_pack_type_id, status_change_date)
                VALUES (@MerchDeliveryStatusId, @MerchPackTypeId, @StatusChangeDate);";

            var parameters = new
            {
                MerchDeliveryStatusId = itemToCreate.Status.Id,
                MerchPackTypeId = itemToCreate.MerchPackType.Id,
                StatusChangeDate = itemToCreate.StatusChangeDate.Value
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
                UPDATE merch_deliveries (merch_delivery_status_id, merch_pack_type_id, status_change_date)
                SET merch_delivery_status_id = @MerchDeliveryStatusId, 
                    merch_pack_type_id = @MerchPackTypeId, 
                    status_change_date = @StatusChangeDate
                WHERE id = @MerchDeliveryId;
                
                DELETE FROM merch_delivery_sku_maps
                WHERE merch_delivery_id = @MerchDeliveryId;
                
                INSERT INTO merch_delivery_sku_maps(merch_delivery_id, sku_id)
                SELECT @MerchDeliveryId, sku_id
                FROM unnest(@SkuIds) sku_id;
                ";

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

        // public async Task<MerchDelivery?> FindByIdAsync(int id, CancellationToken token = default)
        // {
        //     throw new System.NotImplementedException();
        // }
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