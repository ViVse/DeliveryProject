using Npgsql;

namespace Discount.API.Extensions
{
    public static class HostExtensions
    {
        public static IHost MigrateDatabase<TContext>(this IHost host, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var configuration = services.GetRequiredService<IConfiguration>();
                var logger = services.GetRequiredService<ILogger<TContext>>();

                try
                {
                    logger.LogInformation("Migrating postresql database.");

                    using var connection = new NpgsqlConnection(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
                    connection.Open();

                    using var command = new NpgsqlCommand
                    {
                        Connection = connection
                    };

                    //Coupon Table
                    command.CommandText = "DROP TABLE IF EXISTS Coupon";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE Coupon(
	                    ID SERIAL PRIMARY KEY NOT NULL,
	                    Code VARCHAR (10) UNIQUE,
	                    Description TEXT,
	                    IsPercent Boolean NOT NULL,
	                    Amount Int NOT NULL
                    )
                    ";
                    command.ExecuteNonQuery();


                    command.CommandText = "INSERT INTO Coupon (Code, Description, IsPercent, Amount) VALUES ('QywBrIap8D', 'Coupon for first purchase', FALSE, 100);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO Coupon (Code, Description, IsPercent, Amount) VALUES ('vldIsDXPPu', 'New coupon', TRUE, 5);";
                    command.ExecuteNonQuery();
                    logger.LogInformation("Coupon table created.");

                    //ProductDiscount Table
                    command.CommandText = "DROP TABLE IF EXISTS ProductDiscount";
                    command.ExecuteNonQuery();

                    command.CommandText = @"CREATE TABLE ProductDiscount(
	                    ID SERIAL PRIMARY KEY NOT NULL,
	                    ProductId INT UNIQUE,
	                    IsPercent Boolean NOT NULL,
	                    Amount Int NOT NULL
                    )
                    ";
                    command.ExecuteNonQuery();


                    command.CommandText = "INSERT INTO ProductDiscount (ProductId, IsPercent, Amount) VALUES (1, FALSE, 100);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO ProductDiscount (ProductId, IsPercent, Amount) VALUES (2, TRUE, 5);";
                    command.ExecuteNonQuery();
                    logger.LogInformation("ProductDiscount table created.");

                    logger.LogInformation("Migrated postresql database.");
                }
                catch (NpgsqlException ex)
                {
                    logger.LogError(ex, "An error occurred while migrating the postresql database");

                    if(retryForAvailability < 50)
                    {
                        retryForAvailability++;
                        System.Threading.Thread.Sleep(2000);
                        MigrateDatabase<TContext>(host, retryForAvailability);
                    }
                }
            }

            return host;
        }
    }
}
