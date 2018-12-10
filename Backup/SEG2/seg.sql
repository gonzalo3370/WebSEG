drop table DetalleProducto
delete from Imagenes
delete from Productos
delete from Categorias

alter table Productos
alter column Nombre NVARCHAR(255) NOT NULL

alter table Productos
alter column Descripcion NVARCHAR(MAX) NOT NULL

create table Especificaciones
(
	IdEspecificacion int not null,
	Texto NVARCHAR(500) not null,
	Orden int not null,
	CONSTRAINT PK_Especificaciones PRIMARY KEY (IdEspecificacion),
	CONSTRAINT U_Especificaciones_Orden UNIQUE (Orden)
)

create table ProductoEspecificaciones
(
	IdProducto int not null,
	IdEspecificacion int not null,
	Valor NVARCHAR(500) not null,
	CONSTRAINT PK_ProductoEspecificaciones PRIMARY KEY (IdProducto, IdEspecificacion),
	CONSTRAINT FK_ProductoEspecificaciones_Producto FOREIGN KEY (IdProducto) REFERENCES Productos(IdProducto),
	CONSTRAINT FK_ProductoEspecificaciones_Especificaciones FOREIGN KEY (IdEspecificacion) REFERENCES Especificaciones(IdEspecificacion)
)

CREATE TABLE XMLs
(
	nombre NVARCHAR(50) PRIMARY KEY,
	XMLData XML
)

INSERT INTO XMLs(XMLData, nombre)
SELECT CONVERT(XML, BulkColumn) AS BulkColumn, 'categorias'
FROM OPENROWSET(BULK 'C:\temporal\seg\categorias.xml', SINGLE_BLOB) AS x;

INSERT INTO XMLs(XMLData, nombre)
SELECT CONVERT(XML, BulkColumn) AS BulkColumn, 'especificaciones'
FROM OPENROWSET(BULK 'C:\temporal\seg\especificaciones.xml', SINGLE_BLOB) AS x;

INSERT INTO XMLs(XMLData, nombre)
SELECT CONVERT(XML, BulkColumn) AS BulkColumn, 'productos'
FROM OPENROWSET(BULK 'C:\temporal\seg\productos.xml', SINGLE_BLOB) AS x;

INSERT INTO XMLs(XMLData, nombre)
SELECT CONVERT(XML, BulkColumn) AS BulkColumn, 'productosEspecificacion'
FROM OPENROWSET(BULK 'C:\temporal\seg\productosEspecificacion.xml', SINGLE_BLOB) AS x;

INSERT INTO XMLs(XMLData, nombre)
SELECT CONVERT(XML, BulkColumn) AS BulkColumn, 'productosImagenes'
FROM OPENROWSET(BULK 'C:\temporal\seg\productosImagenes.xml', SINGLE_BLOB) AS x;

-- Categorias
	DECLARE @xml AS XML, @hDoc AS INT

	SELECT @XML = XMLData
	FROM XMLs
	WHERE Nombre = 'Categorias'
	EXEC sp_xml_preparedocument @hDoc OUTPUT, @XML

	INSERT INTO Categorias(IdCategoria, Nombre)
	SELECT IdCategoria, Nombre
	FROM OPENXML(@hDoc, 'Categorias/Categoria')
	WITH 
	(
	IdCategoria int '@IdCategoria',
	Nombre [varchar](100) '@Nombre'
	)
	EXEC sp_xml_removedocument @hDoc
--

-- Especificaciones
	DECLARE @xml AS XML, @hDoc AS INT

	SELECT @XML = XMLData
	FROM XMLs
	WHERE Nombre = 'Especificaciones'
	EXEC sp_xml_preparedocument @hDoc OUTPUT, @XML

	INSERT INTO Especificaciones(IdEspecificacion, Texto, Orden)
	SELECT IdEspecificacion, Texto, ROW_NUMBER() OVER(ORDER BY IdEspecificacion) * 100 AS Orden
	FROM OPENXML(@hDoc, 'Especificaciones/Especificacion')
	WITH 
	(
	IdEspecificacion int '@IdEspecificacion',
	Texto [varchar](100) '@Nombre'
	)
	EXEC sp_xml_removedocument @hDoc
--

-- Productos
	DECLARE @xml AS XML, @hDoc AS INT

	SELECT @XML = XMLData
	FROM XMLs
	WHERE Nombre = 'Productos'
	EXEC sp_xml_preparedocument @hDoc OUTPUT, @XML

	INSERT INTO Productos(IdProducto, IdCategoria, Nombre, DescripcionBreve, Descripcion)
	SELECT IdProducto, IdCategoria, Nombre, DescripcionBreve, Descripcion
	FROM OPENXML(@hDoc, 'Productos/Producto')
	WITH 
	(
	IdProducto int '@IdProducto',
	IdCategoria int '@IdCategoria',
	Nombre varchar(max) '@Nombre',
	DescripcionBreve nvarchar(max) '@Breve',
	Descripcion nvarchar(max) '@Descripcion'
	)
	EXEC sp_xml_removedocument @hDoc
--

-- ProductoEspecificaciones
	DECLARE @xml AS XML, @hDoc AS INT

	SELECT @XML = XMLData
	FROM XMLs
	WHERE Nombre = 'productosEspecificacion'
	EXEC sp_xml_preparedocument @hDoc OUTPUT, @XML

	INSERT INTO ProductoEspecificaciones(IdEspecificacion, IdProducto, Valor)
	SELECT IdEspecificacion, IdProducto, Valor
	FROM OPENXML(@hDoc, 'ProductosEspecificaciones/Values')
	WITH 
	(
	IdEspecificacion int '@IdEspecificacion',
	IdProducto int '@IdProducto',
	Valor nvarchar(500) '@Texto'
	)
	EXEC sp_xml_removedocument @hDoc
--

-- ProductosImagenes
	create table #auxImg(IdImagen int, IdProducto int, ruta nvarchar(500), procesada int)
	
	DECLARE @xml AS XML, @hDoc AS INT

	SELECT @XML = XMLData
	FROM XMLs
	WHERE Nombre = 'ProductosImagenes'
	EXEC sp_xml_preparedocument @hDoc OUTPUT, @XML

	insert into #auxImg(IdImagen, IdProducto, ruta)
	SELECT IdImagen, IdProducto, 'C:\temporal\seg\imagenes\' + convert(nvarchar, IdImagen) + '_' + convert(nvarchar, IdProducto) + '.jpg'
	FROM OPENXML(@hDoc, 'ProductosImagenes/Values')
	WITH 
	(
	IdImagen int '@IdImagen',
	IdProducto int '@IdProducto'
	)

	EXEC sp_xml_removedocument @hDoc
	
	declare @idImagen int, @idProducto int, @sql nvarchar(max)

	while(exists(
		select *
		from #auxImg
		where procesada is null
	))
	begin
		select  @idImagen = min(idImagen)
		from #auxImg
		where procesada is null
		
		select  @idProducto = min(idProducto)
		from #auxImg
		where idImagen = @idImagen		
	
		SET @sql=N'INSERT INTO Imagenes(IdImagen, IdProducto, Imagen)
		select ' + convert(nvarchar, @idImagen) + ', ' + convert(nvarchar, @idProducto) + ', BulkColumn
		FROM 
		OPENROWSET(BULK ''C:\temporal\seg\imagenes\' + convert(nvarchar, @idImagen) + '_' + convert(nvarchar, @idProducto) + '.jpg'', SINGLE_BLOB) AS x;'

		EXEC sp_executesql @sql;

		update #auxImg set procesada = 1
		where idImagen = @idImagen
	end

--


select * from Imagenes
update #auxImg set procesada = null
commit

SELECT BulkColumn 
FROM Openrowset( Bulk 'C:\temporal\seg\imagenes\1_157.jpg', Single_Blob) as img


select *
from #auxImg a


DECLARE
@idImagen int = 1,
@idProducto int = 157,
@v_sql nvarchar(1000)


SET @v_sql=N'INSERT INTO Imagenes(IdImagen, IdProducto, Imagen)
select ' + convert(nvarchar, @idImagen) + ', ' + convert(nvarchar, @idProducto) + ', BulkColumn
FROM 
OPENROWSET(BULK ''C:\temporal\seg\imagenes\' + convert(nvarchar, @idImagen) + '_' + convert(nvarchar, @idProducto) + '.jpg'', SINGLE_BLOB) AS x;'

EXEC sp_executesql @v_sql;


select *
from Imagenes

SELECT @v_file

set @ruta = 'C:\temporal\seg\imagenes\1_157.jpg';
SELECT BulkColumn 
FROM Openrowset( Bulk @ruta, Single_Blob) as img


select * from Especificaciones
------

SELECT XMLData
FROM XMLs
WHERE nombre = 'Especificaciones'

select *
from Imagenes


	DECLARE @xml AS XML, @hDoc AS INT, @sql NVARCHAR (MAX)

	SELECT @XML = XMLData
	FROM XMLs
	WHERE Nombre = 'productosEspecificacion'
	EXEC sp_xml_preparedocument @hDoc OUTPUT, @XML

	
	SELECT distinct IdEspecificacion
	FROM OPENXML(@hDoc, 'ProductosEspecificaciones/Values')
	WITH 
	(
	IdEspecificacion int '@IdEspecificacion',
	IdProducto int '@IdProducto',
	Valor nvarchar(500) '@Texto'
	)
	order by IdEspecificacion
	EXEC sp_xml_removedocument @hDoc
/*
categorias
Portones corredizos
Portones levadizos
Portones pivotantes
Accesorios de automatización
Camaras CCTV
Camaras IP
Alarmas
*/


insert into Categorias
values(1,'Automatizaciones/Portones/Corredizos'),(2,'Automatizaciones/Portones/Levadizos'),(3,'Automatizaciones/Portones/Pivotantes'),(4,'Automatizaciones/Accesorios'),(5,'"CCTV"'),(6,'Intrusión/Alarma'),(7,'Control-de-Accesos'),(8,'"Accesorios"')



				(1, "Automatizaciones/Portones/Corredizos"),
				(2, "Automatizaciones/Portones/Levadizos"),
				(3, "Automatizaciones/Portones/Pivotantes"),
				(4, "Automatizaciones/Accesorios"),
				(5, "CCTV"),
				(6, "Intrusión/Alarmas"),
                (7, "Control-de-Accesos"),
                (8, "Accesorios")


delete from XMLs

select * from Categorias