CREATE or alter PROCEDURE sp_ListarProductos
AS
BEGIN
    SELECT 
        p.id_producto AS IdProducto,
        p.nombre AS Producto,
        p.descripcion AS Descripcion,
        c.nombre AS Categoria,
        p.fecha_fabricacion AS FechaFabricacion,
        p.fecha_vencimiento AS FechaVencimiento,
        p.precio AS Precio,
        p.stock AS Stock
    FROM Producto p
    INNER JOIN Categoria c ON p.id_categoria = c.id_categoria;
END;
GO
exec dbo.sp_ListarProductos
go

CREATE PROCEDURE sp_ObtenerProductoPorId
    @IdProducto INT
AS
BEGIN
    SELECT 
        id_producto AS IdProducto,
        nombre AS Producto,
        descripcion AS Descripcion,
        id_categoria AS IdCategoria,
        fecha_fabricacion AS FechaFabricacion,
        fecha_vencimiento AS FechaVencimiento,
        precio AS Precio,
        stock AS Stock
    FROM Producto
    WHERE id_producto = @IdProducto;
END;
GO


CREATE OR ALTER PROCEDURE sp_GuardarProducto
    @IdProducto INT,
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255),
    @IdCategoria INT,
    @FechaFabricacion DATE,
    @FechaVencimiento DATE,
    @Precio DECIMAL(10, 2),
    @Stock INT
AS
BEGIN
    MERGE INTO Producto AS target
    USING (SELECT @IdProducto AS IdProducto, @Nombre AS Nombre, @Descripcion AS Descripcion,
                  @IdCategoria AS IdCategoria, @FechaFabricacion AS FechaFabricacion, 
                  @FechaVencimiento AS FechaVencimiento, @Precio AS Precio, 
                  @Stock AS Stock) AS source
    ON target.id_producto = source.IdProducto
    WHEN MATCHED THEN
        UPDATE SET 
            target.nombre = source.Nombre,
            target.descripcion = source.Descripcion,
            target.id_categoria = source.IdCategoria,
            target.fecha_fabricacion = source.FechaFabricacion,
            target.fecha_vencimiento = source.FechaVencimiento,
            target.precio = source.Precio,
            target.stock = source.Stock
    WHEN NOT MATCHED BY TARGET THEN
        INSERT (nombre, descripcion, id_categoria, fecha_fabricacion, fecha_vencimiento, precio, stock)
        VALUES (source.Nombre, source.Descripcion, source.IdCategoria, source.FechaFabricacion, 
                source.FechaVencimiento, source.Precio, source.Stock);
END;
GO

CREATE PROCEDURE sp_EliminarProducto
    @IdProducto INT
AS
BEGIN
    DELETE FROM Producto
    WHERE id_producto = @IdProducto;
END;
GO

CREATE PROCEDURE sp_ListarCategorias
AS
BEGIN
    SELECT 
        id_categoria AS IdCategoria,
        nombre AS NombreCategoria
    FROM Categoria;
END
GO

INSERT INTO Categoria (nombre)
VALUES 
    ('Medicamentos'),
	('Higiene Personal'),
    ('Cuidado de la Piel'),
    ('Cuidado Capilar'),
    ('Cosméticos'),
    ('Productos Naturales')
GO
