﻿using ContractInProcessAPI.Models;
using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace ContractInProcessAPI.DataAccess
{
    public class ContractRepository : IContractRepository
    {
        private readonly string _connectionString;

        public ContractRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        private OracleConnection GetConnection()
        {
            return new OracleConnection(_connectionString);
        }

        public async Task<List<EmpleadoME>> GetEmpleados(string? date)
        {
            var empleados = new List<EmpleadoME>();
            var queryDate = date ?? DateTime.Now.ToString("yyyy-MM-dd");
            var count = 1; // Iniciar el contador de importación

            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @" SELECT DISTINCT
                                        (SELECT LISTAGG(B2.TIP_CONT || '-' || B2.COD_EMPL || '-' || B2.NRO_CONT || '-01', ', ')
                                                 WITHIN GROUP (ORDER BY B2.TIP_CONT, B2.COD_EMPL, B2.NRO_CONT)
                                                 FROM NM_CONTR@SIGKACTUS B2
                                                 WHERE B2.TIP_CONT || '-' || B2.COD_EMPL || '-' || B2.NRO_CONT || '-01' = C.TIP_CONT || '-' || C.COD_EMPL || '-' || C.NRO_CONT || '-01') AS CONCATENATED_CONTRACTS,
	                                    C.COD_EMPR AS CREATEDBY,
                                        C.NRO_CONT AS NUMERO_CONTRATO,
                                        B.TIP_DOCU AS TIPO_DOCUMENTO,
                                        B.COD_EMPL AS CEDULA,
                                        TO_CHAR(B.FEC_EXPE, 'YYYY-MM-DD') AS FECHA_EXPEDICION,
                                        B.PAI_EXPE AS PAIS_EXPEDICION_DIAN,
                                        B.DTO_EXPE AS DPTO_EXPEDICION_DANE,
                                        B.MPI_EXPE AS MUNICIPIO_EXPEDICION,
                                        B.APE_EMPL AS APELLIDOS,
                                        B.NOM_EMPL AS NOMBRES,
                                        CT.NOM_CENP AS SEDE,
                                        CC.NOM_CCOS AS AREA,
                                        BI.NOM_CARG AS CARGO,
                                        B.BOX_MAIL AS EMAIL_INSTITUCIONAL,
                                        B.EEE_MAIL AS EMAIL_PERSONAL,
                                        B.TEL_RESI AS TELEFONO_CASA,
                                        B.TEL_MOVI AS CELULAR,
                                        B.DIR_RESI AS DIRECCION_CASA,
                                        B.BAR_RESI AS BARRIO,
                                        C.TIP_CONT AS TIPO_CONTRATO,
                                        TO_CHAR(C.FEC_POSE, 'YYYY-MM-DD') AS FEC_CONTRATO,
                                        TO_CHAR(C.FEC_RESO, 'YYYY-MM-DD') AS FEC_FIN,
                                        C.SUE_BASI AS SALARIO,
                                        DECODE(C.IND_ACTI, 'A', 'ACTIVO', 'INACTIVO') AS ESTADO,
                                        CT.NOM_CENP AS REGIONAL,
                                        C.FEC_CONT AS CONECTIVIDAD,
                                        B.FEC_NACI AS FECHANACIMIENTO,
                                        B.PAI_NACI AS PAISNACIMIENTO,
                                        B.DTO_NACI AS DPTNACIMIENTO,
                                        B.MPI_NACI AS MUNNACIMIENTO
                                    FROM NM_CONTR@SIGKACTUS C
                                        LEFT JOIN NM_CENTP@SIGKACTUS CT ON C.COD_EMPR = CT.COD_EMPR AND C.COD_CENP = CT.COD_CENP
                                        LEFT JOIN NM_TNOMI@SIGKACTUS TN ON TN.COD_EMPR = C.COD_EMPR AND TN.COD_TNOM = C.COD_TNOM
                                        LEFT JOIN GN_CCOST@SIGKACTUS CC ON C.COD_EMPR = CC.COD_EMPR AND C.COD_CCOS = CC.COD_CCOS
                                        LEFT JOIN BI_EMPLE@SIGKACTUS B ON C.COD_EMPR = B.COD_EMPR AND C.COD_EMPL = B.COD_EMPL
                                        LEFT JOIN GN_DIVIP@SIGKACTUS F ON F.COD_DPTO = B.DTO_NACI AND F.COD_PAIS = B.PAI_NACI AND F.COD_MPIO = B.MPI_NACI
                                        LEFT JOIN BAS_TERCERO BB ON BB.NUM_IDENTIFICACION = CAST(B.COD_EMPL AS VARCHAR2(12))
                                        LEFT JOIN BI_CARGO@SIGKACTUS BI ON BI.COD_CARG = C.COD_CARG AND BI.COD_EMPR = C.COD_EMPR
                                        LEFT JOIN AC_NIVEL@SIGKACTUS A ON BI.COD_NIVE = A.COD_NIVE AND BI.COD_EMPR = A.COD_EMPR
                                        LEFT JOIN SRC_SEDE S ON S.COD_SEDE = CT.COD_CENP
                                        LEFT JOIN SRC_SECCIONAL SC ON SC.ID_SECCIONAL = S.ID_SECCIONAL
                                WHERE to_char(C.FEC_POSE, 'YYYY-MM-DD')= '" + date + @"'
                                AND C.COD_EMPR = 3031 AND C.IND_ACTI = 'A' AND C.COD_ARBO IN(1000, 2000)";

                    var results = await connection.QueryAsync<dynamic>(query, new { Date = queryDate });
                    foreach (var result in results)
                    {
                        var empleado = new EmpleadoME
                        {
                            ImportId = count.ToString(),
                            CreationDate = DateTime.Now.ToString(),
                            CreatedBy = Convert.ToString(result.CREATEDBY),

                            AlternativeId = Convert.ToString(result.CONCATENATED_CONTRACTS),
                            Version = "1",
                            Prototipo = "AQUI VA EL NOMBRE DE CONTRATO EN CADENA DE TEXTO", // Modificar según tus necesidades

                            NumeroContrato = Convert.ToString(result.NUMERO_CONTRATO),
                            TipoDocumentoEmpleado = Convert.ToString(result.TIPO_DOCUMENTO),
                            NumeroDocumentoEmpleado = Convert.ToString(result.CEDULA),
                            FechaExpedicionEmpleado = Convert.ToString(result.FECHA_EXPEDICION),
                            PaisExpedicionDIAN = Convert.ToString(result.PAIS_EXPEDICION_DIAN),
                            DepartamentoExpedicionDANE = Convert.ToString(result.DPTO_EXPEDICION_DANE),
                            CiudadExpedicion = Convert.ToString(result.MUNICIPIO_EXPEDICION),
                            MunicipioExpedicion = await DeterminarCiudadesKactus(
                                Convert.ToString(result.PAIS_EXPEDICION_DIAN),
                                Convert.ToString(result.DPTO_EXPEDICION_DANE),
                                Convert.ToString(result.MUNICIPIO_EXPEDICION)
                            ),
                            ApellidosEmpleado = Convert.ToString(result.APELLIDOS),
                            NombresEmpleado = Convert.ToString(result.NOMBRES),
                            SedeEmpleado = Convert.ToString(result.SEDE),
                            AreaEmpleado = Convert.ToString(result.AREA),
                            CargoEmpleado = Convert.ToString(result.CARGO),
                            EmailInstitucionalEmpleado = Convert.ToString(result.EMAIL_INSTITUCIONAL),
                            EmailPersonalEmpleado = Convert.ToString(result.EMAIL_PERSONAL),
                            TelefonoCasaEmpleado = Convert.ToString(result.TELEFONO_CASA),
                            CelularEmpleado = Convert.ToString(result.CELULAR),
                            DireccionCasaEmpleado = Convert.ToString(result.DIRECCION_CASA),
                            BarrioEmpleado = Convert.ToString(result.BARRIO),
                            CiudadResidencia = !string.IsNullOrEmpty(Convert.ToString(result.SEDE)) ? Convert.ToString(result.SEDE).Split(" ")[0] : string.Empty,
                            TipoContratoEmpleado = Convert.ToString(result.TIPO_CONTRATO),
                            FechaInicioContratoEmpleado = Convert.ToString(result.FEC_CONTRATO),
                            FechaFinContratoEmpleado = string.IsNullOrEmpty(Convert.ToString(result.FEC_FIN)) ? "N/A" : Convert.ToString(result.FEC_FIN),

                            //TARGET DESDE DB
                            DuracionContrato = "POR MAPEAR DESDE DB",

                            SalarioEmpleado = Convert.ToString(result.SALARIO),
                            SalarioEmpleadoLetras = Conversor.Conversor.NumeroALetras(decimal.Parse(Convert.ToString(result.SALARIO))),
                            CiudadFirmaContrato = "BOGOTA",
                            EstadoEmpleado = Convert.ToString(result.ESTADO),
                            SedeRegional = Convert.ToString(result.SEDE),
                            FechaConectividad = Convert.ToString(result.CONECTIVIDAD),
                            FechaPersonales = Convert.ToString(result.CONECTIVIDAD),
                            FechaNacimiento = Convert.ToString(result.FECHANACIMIENTO),
                            PaisNacimiento = Convert.ToString(result.PAISNACIMIENTO),
                            DepartamentoNacimiento = Convert.ToString(result.DPTNACIMIENTO),
                            MunicipioNacimiento = Convert.ToString(result.MUNNACIMIENTO),
                            LugarNacimiento = await DeterminarCiudadesKactus(
                                Convert.ToString(result.PAISNACIMIENTO),
                                Convert.ToString(result.DPTNACIMIENTO),
                                Convert.ToString(result.MUNNACIMIENTO)
                            ) ?? Convert.ToString(result.MUNICIPIONACIMIENTO),
                            DepartamentoPersonales = "BOGOTA",

                            //TARGET DESDE DB
                            CentroDeCostos = Convert.ToString(result.NOM_CCOS),     // <-- Validar que el dato venga de aca
                            ClaseDeNomina = Convert.ToString(result.NOM_TNOM),      // <-- Validar que el dato venga de aca
                            FechaFinMarcador = "POR MAPEAR DESDE DB",
                            FirmaElectronicaHtml2 = "POR MAPEAR DESDE DB",
                            TipoContrato = "POR MAPEAR DESDE DB",
                            InsertarEmpresa = "POR MAPEAR DESDE DB",
                            CodigoParaWord = "POR MAPEAR DESDE DB",
                            FirmaElectronicaMod7 = "POR MAPEAR DESDE DB",
                            FirmaCapturadaMod7 = "POR MAPEAR DESDE DB",

                            Tercero = "POR MAPEAR DESDE DB",

                            //TARGET DESDE DB
                            FirmaElectronicaHtml = "POR MAPEAR DESDE DB",
                            ValidacionTipoCE = "POR MAPEAR DESDE DB",
                            CodContrato = "POR MAPEAR DESDE DB",

                            EmpresaPermisos = "POR MAPEAR DESDE DB",
                            Empresa = "POR MAPEAR DESDE DB",

                            FirmaCapturada2Mod7 = "POR MAPEAR DESDE DB",
                            CopiarPDFs = "POR MAPEAR DESDE DB",
                            FirmaElectronica2Mod7 = "POR MAPEAR DESDE DB",
                            DatosConcatenados = "POR MAPEAR DESDE DB",
                            DescripcionRegional = "POR MAPEAR DESDE DB",
                            Elaboracion = "POR MAPEAR DESDE DB",
                            FechaElaboracion = "POR MAPEAR DESDE DB"
                        };

                        empleados.Add(empleado);
                        count++; // Incrementar el ID temporal
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return empleados;
        }


        public async Task<string?> DeterminarCiudadesKactus(string codPais, string codDepartamento, string codMunicipio)
        {
            string? Ciudad = string.Empty;

            if (codPais == "169" && codDepartamento == "1" && codMunicipio == "1")
            {
                Ciudad = "BOGOTA";
            }
            else
            {
                using (var oracleConnection = new OracleConnection(this._connectionString))
                {
                    try
                    {
                        // Abre la conexión
                        await oracleConnection.OpenAsync();

                        // Define la consulta con parámetros
                        var query = @"
                                SELECT NOM_CENP
                                FROM NM_CENTP@SIGKACTUS
                                WHERE COD_PAIS = :CodPais
                                  AND COD_DPTO = :CodDepto
                                  AND COD_MPIO = :CodMpio
                ";

                        // Ejecuta la consulta usando Dapper
                        var results = await oracleConnection.QueryAsync<string>(query, new { CodPais = codPais, CodDepto = codDepartamento, CodMpio = codMunicipio });

                        // Maneja el caso de múltiples resultados
                        if (results.Any())
                        {
                            // Aquí tomamos el primer resultado. Si esperas manejar múltiples resultados, ajusta la lógica según sea necesario.
                            Ciudad = results.First().Split(" ")[0];
                        }
                    }
                    catch (Exception e)
                    {
                        // Manejo de errores; puedes registrar el error si es necesario
                        Ciudad = null;
                        // Es buena idea registrar el error en lugar de solo convertir a string
                        Console.WriteLine(e.Message);
                    }
                }
            }

            return Ciudad;
        }

    }
}


