using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Infrastructure.Interfaces;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Models;

#pragma warning disable 1998

namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private const int Timeout = 5;
        
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;

        public EmployeeRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory,
            IChangeTracker changeTracker)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
        }

        public async Task<Employee?> CreateAsync(Employee itemToCreate, CancellationToken token)
        {
            const string sql = @"
                INSERT INTO employees (name, clothing_size_id, email_address, manager_email_address)
                VALUES (@Name, @ClothingSizeId, @EmailAddress, @ManagerEmailAddress);";

            var parameters = new
            {
                Name = itemToCreate.Name.Value,
                ClothingSizeId = itemToCreate.ClothingSize?.Id,
                EmailAddress = itemToCreate.EmailAddress?.Value,
                ManagerEmailAddress = itemToCreate.ManagerEmailAddress?.Value
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

        public async Task<Employee?> UpdateAsync(Employee itemToUpdate, CancellationToken cancellationToken = default)
        {
            const string sql = @"
                UPDATE employees
                SET name = @Name, clothing_size_id = @ClothingSizeId, 
                    email_address = @EmailAddress, manager_email_address = @ManagerEmailAddress
                WHERE id = @EmployeeId;";

            var parameters = new
            {
                EmployeeId = itemToUpdate.Id,
                Name = itemToUpdate.Name.Value,
                ClothingSizeId = itemToUpdate.ClothingSize?.Id,
                EmailAddress = itemToUpdate.EmailAddress?.Value,
                ManagerEmailAddress = itemToUpdate.ManagerEmailAddress?.Value
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

        public async Task<Employee?> FindByIdAsync(long id, CancellationToken token = default)
        {
            const string sql = @"
                SELECT e.id, e.name, e.clothing_size_id, e.email_address, e.manager_email_address,
                       cs.id, cs.name, md.id, md.merch_pack_type_id, md.merch_delivery_status_id, 
                       md.status_change_date, mpt.id, mpt.name, mds.id, mds.name
                FROM employees e
                LEFT JOIN clothing_sizes cs ON e.clothing_size_id = cs.clothing_size_id
                LEFT JOIN employee_merch_delivery_maps emdm ON e.id = emdm.employee_id
                LEFT JOIN merch_deliveries md ON emdm.merch_delivery_id = md.id
                LEFT JOIN merch_pack_types mpt ON md.merch_pack_type_id = mpt.id
                LEFT JOIN merch_delivery_statuses mds ON md.merch_delivery_status_id = mds.id
                WHERE e.id = @Id";

            var parameters = new
            {
                Id = id
            };
            
            var commandDefinition = new CommandDefinition(
                sql,
                parameters,
                commandTimeout: Timeout,
                cancellationToken: token);
            
            var connection = await _dbConnectionFactory.CreateConnection(token);
            var employees = await connection.QueryAsync<Models.Employee, Models.ClothingSize, 
                Models.MerchDelivery, Models.MerchPackType, Models.MerchDeliveryStatus, Employee>(commandDefinition,
                ())
        }

        public async Task<Employee?> FindByEmailAsync(string email, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Employee?> FindByDeliveryId(int deliveryId, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<Employee>> GetByMerchDeliveryStatus(int statusId, CancellationToken token = default)
        {
            throw new System.NotImplementedException();
        }
    }
}