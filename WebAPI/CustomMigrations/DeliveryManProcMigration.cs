using DAL;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebAPI.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("CustomMigration_DeliveryManProc")]
    public class DeliveryManProcMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "./CreateDeliveryManProc.txt");

            migrationBuilder.Sql(@"
                USE DeliveryDb
                GO

                CREATE PROC GetAllDeliveryMen AS
                SELECT * FROM DeliveryMen
                GO

                CREATE PROC GetDeliveryMan(@id int) AS
                SELECT * FROM DeliveryMen WHERE Id = @id
                GO

                CREATE PROC DeleteDeliveryMan(@id int) AS
                BEGIN
                DELETE FROM DeliveryMen WHERE Id = @id
                END
                GO

                CREATE PROC AddDeliveryMan(
					                @firstName varchar(50),
					                @lastName varchar(50),
					                @phone varchar(10)) AS
                INSERT INTO DeliveryMen VAlUES(@firstName, @lastName, @phone)
                GO

                CREATE PROC UpdateDeliveryMan(
					                @id int,
					                @firstName varchar(50),
					                @lastName varchar(50),
					                @phone varchar(10)) AS
                UPDATE DeliveryMen
                SET [FirstName] = @firstName,
	                [LastName] = @lastName,
	                [Phone] = @phone
                WHERE Id = @id
                GO"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                USE DeliveryDb
                GO

                DROP PROC IF EXISTS GetAllDeliveryMen
                GO

                DROP PROC IF EXISTS GetDeliveryMan
                GO

                DROP PROC IF EXISTS DeleteDeliveryMan
                GO

                DROP PROC IF EXISTS AddDeliveryMan
                GO

                DROP PROC IF EXISTS UpdateDeliveryMan
                GO
            ");
        }
    }
}
