using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate;
using OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate;
using OzonEdu.MerchandiseApi.Infrastructure.Repositories.Infrastructure.Interfaces;
using ClothingSize = OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate.ClothingSize;
using Domain = OzonEdu.MerchandiseApi.Domain.AggregationModels.EmployeeAggregate.Employee;
using MerchDelivery = OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate.MerchDelivery;
using MerchDeliveryStatus = OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate.MerchDeliveryStatus;
using MerchPackType = OzonEdu.MerchandiseApi.Domain.AggregationModels.MerchDeliveryAggregate.MerchPackType;

#pragma warning disable 1998

namespace OzonEdu.MerchandiseApi.Infrastructure.Repositories.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private const int Timeout = 5;
        
        private readonly IDbConnectionFactory<NpgsqlConnection> _dbConnectionFactory;
        private readonly IChangeTracker _changeTracker;
        private readonly IMerchDeliveryRepository _merchDeliveryRepository;

        public EmployeeRepository(IDbConnectionFactory<NpgsqlConnection> dbConnectionFactory,
            IChangeTracker changeTracker,
            IMerchDeliveryRepository merchDeliveryRepository)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _changeTracker = changeTracker;
            _merchDeliveryRepository = merchDeliveryRepository;
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

        public async Task<Employee?> FindByIdAsync(int id, CancellationToken token = default)
        {
           var sql = @"
                SELECT e.id, e.name, e.clothing_size_id, e.email_address, e.manager_email_address,
                       cs.id, cs.name
                FROM employees e
                LEFT JOIN clothing_sizes cs ON e.clothing_size_id = cs.clothing_size_id                
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
            var employees = await connection.QueryAsync<Models.Employee, Models.ClothingSize, Employee>(
                commandDefinition,
                (employee, size) => new Employee(
                    new Name(employee.Name),
                    employee.EmailAddress is null 
                        ? null 
                        : new EmailAddress(employee.EmailAddress),
                    employee.ManagerEmailAddress is null 
                        ? null 
                        : new EmailAddress(employee.ManagerEmailAddress),
                    size?.Id is null 
                        ? null 
                        : new ClothingSize(size.Id.Value, size.Name ?? string.Empty)));

            var employee = employees.FirstOrDefault();

            if (employee is null)
                return employee;

            var merchDeliveries = await _merchDeliveryRepository
                .GetByEmployeeIdAsync(employee.Id, token);

            if (merchDeliveries is null)
                return employee;
            
            foreach (var delivery in merchDeliveries)
                employee.AddMerchDelivery(delivery);

            return employee;
        }

        public async Task<Employee?> FindByEmailAsync(string email, CancellationToken token = default)
        {
            var sql = @"
                SELECT e.id, e.name, e.clothing_size_id, e.email_address, e.manager_email_address,
                       cs.id, cs.name
                FROM employees e
                LEFT JOIN clothing_sizes cs ON e.clothing_size_id = cs.clothing_size_id                
                WHERE e.email_address = @Email";

            var parameters = new
            {
                Email = email
            };
            
            var commandDefinition = new CommandDefinition(
                sql,
                parameters,
                commandTimeout: Timeout,
                cancellationToken: token);
            
            var connection = await _dbConnectionFactory.CreateConnection(token);
            var employees = await connection.QueryAsync<Models.Employee, Models.ClothingSize, Employee>(
                commandDefinition,
                (employee, size) => new Employee(
                    new Name(employee.Name),
                    employee.EmailAddress is null 
                        ? null 
                        : new EmailAddress(employee.EmailAddress),
                    employee.ManagerEmailAddress is null
                        ? null 
                        : new EmailAddress(employee.ManagerEmailAddress),
                    size is null ? null : new ClothingSize(size.Id.Value, size.Name)));

            var employee = employees.FirstOrDefault();

            if (employee is null)
                return employee;

            var merchDeliveries = await _merchDeliveryRepository
                .GetByEmployeeIdAsync(employee.Id, token);

            if (merchDeliveries is null)
                return employee;
            
            foreach (var delivery in merchDeliveries)
                employee.AddMerchDelivery(delivery);

            return employee;
        }

        public async Task<IEnumerable<Employee>> GetByMerchDeliveryStatusAndSkuCollection(int statusId, 
            IEnumerable<long> skuIdCollection,
            CancellationToken token = default)
        {
            var sql = @"
                SELECT e.id, e.name, e.clothing_size_id, e.email_address, e.manager_email_address,
                       cs.id, cs.name
                FROM employees e
                INNER JOIN employee_merch_delivery_maps emdm ON e.id = emdm.employee_id
                INNER JOIN merch_deliveries md ON emdm.merch_delivery_id = md.id
                INNER JOIN merch_delivery_sku_maps mdsm ON md.id = mdsm.merch_delivery_id
                LEFT JOIN clothing_sizes cs ON e.clothing_size_id = cs.clothing_size_id                
                WHERE md.merch_delivery_status_id = @StatusId
                AND mdsm.sku_id = ANY(@SkuIds);";

            var parameters = new
            {
                StatusId = statusId,
                SkuIds = skuIdCollection.ToArray()
            };
            
            var commandDefinition = new CommandDefinition(
                sql,
                parameters,
                commandTimeout: Timeout,
                cancellationToken: token);
            
            var connection = await _dbConnectionFactory.CreateConnection(token);
            var employees = await connection.QueryAsync<Models.Employee, Models.ClothingSize, Employee>(
                commandDefinition,
                (employee, size) => new Employee(
                    new Name(employee.Name),
                    employee.EmailAddress is null 
                        ? null 
                        : new EmailAddress(employee.EmailAddress),
                    employee.ManagerEmailAddress is null
                        ? null 
                        : new EmailAddress(employee.ManagerEmailAddress),
                    size is null ? null : new ClothingSize(size.Id.Value, size.Name)));

            return employees;
        }
    }
}