using ContractInProcessAPI.Models;
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

            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var query = @"SELECT UNIQUE 
                        B.TIP_DOCU AS TIPO_DOCUMENTO,
                        (B.COD_EMPL) AS CEDULA,
                        to_char(B.FEC_EXPE, 'YYYY-MM-DD') AS FECHA_EXPEDICION, 
                        B.PAI_EXPE AS PAIS_EXPEDICION_DIAN, 
                        B.DTO_EXPE AS DPTO_EXPEDICION_DANE, 
                        B.MPI_EXPE AS MUNICIPIO_EXPEDICION, 
                        B.NOM_EMPL AS NOMBRES, 
                        B.APE_EMPL AS APELLIDOS,                        
                        ct.NOM_CENP AS SEDE,
                        CC.NOM_CCOS AS AREA,
                        BI.NOM_CARG AS CARGO, 
                        B.BOX_MAIL AS EMAIL_INSTITUCIONAL,
                        BB.DIR_EMAIL AS CORREO_SINU, 
                        B.EEE_MAIL AS EMAIL_PERSONAL, 
                        B.TEL_RESI AS TELEFONO_CASA, 
                        B.TEL_MOVI AS CELULAR, 
                        B.DIR_RESI AS DIRECCION_CASA, 
                        B.BAR_RESI AS BARRIO, 
                        C.TIP_CONT AS TIPO_CONTRATO, 
                        to_char(C.FEC_POSE, 'YYYY-MM-DD') AS FEC_CONTRATO, 
                        to_char(C.FEC_RESO, 'YYYY-MM-DD') AS FEC_FIN,
                        C.SUE_BASI AS SALARIO, 
                        decode (C.IND_ACTI, 'A', 'ACTIVO', 'INACTIVO') ESTADO, 
                        C.NRO_CONT AS CONSECUTIVO,  
                        c.fec_cont AS CONECTIVIDAD, 
                        B.FEC_NACI AS FECHANACIMIENTO, 
                        B.PAI_NACI AS PAISNACIMIENTO, 
                        B.DTO_NACI AS DPTNACIMIENTO, 
                        B.MPI_NACI AS MUNNACIMIENTO, 
                        F.NOM_MPIO AS MUNICIPIONACIMIENTO,
                        CC.NOM_CCOS AS NOM_CENTRO_COSTO, 
                        TN.NOM_TNOM AS CLASE_NOMINA, 
                        CT.NOM_CENP AS REGIONAL,
                        CASE WHEN c.cod_tnom = 3101 
                            THEN TRIM(TO_CHAR(b.cod_empl, '999G999G999G999'))||'-'||c.nro_cont
                            ELSE UPPER(sc.nom_seccional)||' '||DECODE(c.cod_tnom, 3200, 'TC', 3201, 'MT', 3202, 'HC')||'-'||TRIM(TO_CHAR(b.cod_empl, '999G999G999G999'))||'-'||c.nro_cont
                        END AS DATOS_CONCATENADOS
                        FROM NM_CONTR@SIGKACTUS C
                        LEFT JOIN NM_CENTP@SIGKACTUS CT ON
                        C.COD_EMPR = CT.COD_EMPR
                        AND C.COD_CENP = CT.COD_CENP
                        LEFT JOIN NM_TNOMI@SIGKACTUS TN ON
                        TN.COD_EMPR = C.COD_EMPR
                        AND C.COD_TNOM = TN.COD_TNOM
                        LEFT JOIN GN_CCOST@SIGKACTUS CC ON
                        C.COD_EMPR = CC.COD_EMPR
                        AND C.COD_CCOS = CC.COD_CCOS
                        LEFT JOIN BI_EMPLE@SIGKACTUS B ON
                        C.COD_EMPR = B.COD_EMPR
                        AND C.COD_EMPL = B.COD_EMPL
                        LEFT JOIN gn_divip@SIGKACTUS f ON
                        f.cod_dpto = b.dto_naci
                        AND f.cod_pais = b.pai_naci
                        AND f.cod_mpio = b.mpi_naci
                        LEFT JOIN BAS_TERCERO BB ON
                        BB.NUM_IDENTIFICACION = CAST (b.COD_EMPL AS VARCHAR2(12))
                        LEFT JOIN BI_CARGO@SIGKACTUS BI ON
                        BI.COD_CARG = C.COD_CARG
                        AND BI.COD_EMPR = C.COD_EMPR
                        LEFT JOIN AC_NIVEL@SIGKACTUS A ON
                        BI.COD_NIVE = A.COD_NIVE
                        AND BI.COD_EMPR = A.COD_EMPR
                        LEFT JOIN SRC_SEDE S ON S.COD_SEDE = CT.COD_CENP
                        LEFT JOIN SRC_SECCIONAL SC ON SC.ID_SECCIONAL = S.ID_SECCIONAL
                        WHERE to_char(C.FEC_POSE, 'YYYY-MM-DD')= '" + date + "' " +
                        "AND C.COD_EMPR = 3031 AND C.IND_ACTI = 'A' AND C.COD_ARBO IN (1000, 2000)";

                    var results = await connection.QueryAsync<dynamic>(query, new { Date = queryDate });
                    foreach (var result in results)
                    {
                        var paisNacimiento = Convert.ToString(result.PAISNACIMIENTO);
                        var dptoNacimiento = Convert.ToString(result.DPTNACIMIENTO);
                        var munNacimiento = Convert.ToString(result.MUNNACIMIENTO);

                        var ciudadExpedicion = await DeterminarCiudadesKactus(
                            Convert.ToString(result.PAIS_EXPEDICION_DIAN),
                            Convert.ToString(result.DPTO_EXPEDICION_DANE),
                            Convert.ToString(result.MUNICIPIO_EXPEDICION)
                        );

                        var lugarNacimiento = string.IsNullOrEmpty(Convert.ToString(result.MUNICIPIONACIMIENTO))
                            ? await DeterminarCiudadesKactus(paisNacimiento, dptoNacimiento, munNacimiento)
                            : await DeterminarCiudadesKactus(paisNacimiento, dptoNacimiento, munNacimiento);

                        // Obtener el salario como string para convertirlo a letras más tarde
                        var salarioString = Convert.ToString(result.SALARIO);
                        var salarioDecimal = decimal.Parse(salarioString);
                        var salarioLetras = Conversor.Conversor.NumeroALetras(salarioDecimal);

                        var empleado = new EmpleadoME
                        {
                            Tipo_Documento_Empleado = Convert.ToString(result.TIPO_DOCUMENTO),
                            Numero_Documento_Empleado = Convert.ToString(result.CEDULA),
                            Fecha_Expedicion_Empleado = Convert.ToString(result.FECHA_EXPEDICION),
                            Pais_Expedicion_DIAN = Convert.ToString(result.PAIS_EXPEDICION_DIAN),
                            Departamento_Expedicion_DANE = Convert.ToString(result.DPTO_EXPEDICION_DANE),
                            Municipio_Expedicion_DANE = Convert.ToString(result.MUNICIPIO_EXPEDICION),
                            Ciudad_Expedicion = ciudadExpedicion,
                            Nombres_Empleado = Convert.ToString(result.NOMBRES),
                            Apellidos_Empleado = Convert.ToString(result.APELLIDOS),
                            Sede_Empleado = Convert.ToString(result.SEDE),
                            Area_Empleado = Convert.ToString(result.AREA),
                            Cargo_Empleado = Convert.ToString(result.CARGO),
                            Email_Institucional_Empleado = Convert.ToString(result.EMAIL_INSTITUCIONAL),
                            Email_Personal_Empleado = Convert.ToString(result.EMAIL_PERSONAL),
                            Telefono_Casa_Empleado = Convert.ToString(result.TELEFONO_CASA),
                            Celular_Empleado = Convert.ToString(result.CELULAR),
                            Direccion_Casa_Empleado = Convert.ToString(result.DIRECCION_CASA),
                            Barrio_Empleado = Convert.ToString(result.BARRIO),
                            Ciudad_Residencia = !string.IsNullOrEmpty(Convert.ToString(result.SEDE)) ? Convert.ToString(result.SEDE).Split(" ")[0] : string.Empty,
                            Tipo_Contrato_Empleado = Convert.ToString(result.TIPO_CONTRATO),
                            Fecha_Inicio_Contrato_Empleado = Convert.ToString(result.FEC_CONTRATO),
                            Fecha_Fin_Contrato_Empleado = string.IsNullOrEmpty(Convert.ToString(result.FEC_FIN)) ? "N/A" : Convert.ToString(result.FEC_FIN),
                            Salario_Empleado = salarioString,
                            Estado_Empleado = Convert.ToString(result.ESTADO),
                            Salario_Empleado_Letras = salarioLetras,
                            Ciudad_Firma_Contrato = "BOGOTA",
                            Numero_Contrato = Convert.ToString(result.CONSECUTIVO),
                            Sede_Regional = Convert.ToString(result.SEDE),
                            Fecha_Conectividad = Convert.ToString(result.CONECTIVIDAD),
                            Fecha_Personales = Convert.ToString(result.CONECTIVIDAD), // ¿Debe ser diferente de Fecha_Conectividad?
                            Fecha_Nacimiento = Convert.ToString(result.FECHANACIMIENTO),
                            Pais_Nacimiento = paisNacimiento,
                            Departamento_Nacimiento = dptoNacimiento,
                            Municipio_Nacimiento = munNacimiento,
                            Lugar_Nacimiento = lugarNacimiento,
                            Departamento_Personales = "BOGOTA",
                            Nom_Centro_Costo = Convert.ToString(result.NOM_CENTRO_COSTO),
                            Clase_Nomina = Convert.ToString(result.CLASE_NOMINA),
                            Regional = Convert.ToString(result.REGIONAL),
                            Datos_concatenados = Convert.ToString(result.DATOS_CONCATENADOS)
                        };

                        empleados.Add(empleado);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al convertir salario: {ex.Message}");
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


