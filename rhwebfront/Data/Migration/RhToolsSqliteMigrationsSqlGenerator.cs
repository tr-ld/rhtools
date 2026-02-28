using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace RHWebFront.Data.Migration;

public class RhToolsSqliteMigrationsSqlGenerator(MigrationsSqlGeneratorDependencies dependencies, IRelationalAnnotationProvider migrationsAnnotations)
    : SqliteMigrationsSqlGenerator(dependencies, migrationsAnnotations)
{
    protected override void Generate(CreateTableOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
    {
        base.Generate(operation, model, builder, terminate);

        AddUpdatedAtDateTrigger(operation, builder);
    }

    private static void AddUpdatedAtDateTrigger(CreateTableOperation operation, MigrationCommandListBuilder builder)
    {
        // Simple convention: any column named "UpdatedAt" gets a trigger. EF Core annihilates ClrType during migrations, so reflection can't be used.
        if (operation.Columns.Any(c => c.Name == "UpdatedAt"))
        {
            builder.AppendLine();
            builder.Append($@"
CREATE TRIGGER IF NOT EXISTS Update{operation.Name}_UpdatedAt
AFTER UPDATE ON {operation.Name}
FOR EACH ROW
BEGIN
    UPDATE {operation.Name} SET UpdatedAt = DATETIME('now') WHERE Id = NEW.Id;
END;");
            builder.EndCommand();
        }
    }

    protected override void Generate(DropTableOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate = true)
    {
        builder.Append($"DROP TRIGGER IF EXISTS Update{operation.Name}_UpdatedAt;");
        builder.EndCommand();
        
        base.Generate(operation, model, builder, terminate);
    }
}