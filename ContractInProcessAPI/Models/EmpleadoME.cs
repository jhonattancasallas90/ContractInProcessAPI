namespace ContractInProcessAPI.Models
{
    public class EmpleadoME
    {
            // POSIBLES DATOS A IMPLEMENTAR  Al convertir validar el tipo de dato
            public string? ImportId { get; set; }
            public string? CreationDate { get; set; } // <--  CLASE NUEVA - CreationDate en DB
            public string? CreatedBy { get; set; }   // <-- CLASE NUEVA - CreatedBy (Por validar)
            public string? AlternativeId { get; set; }   // <-- AlternativeId - STRING (I-1030587912-1-01) - Presuntamente es la concatenacion de CC + Tipo de Contrato - No se sabe si de agente o inscrito
            public string? Version { get; set; }   // <--  Se ajusta como tipo de dato string, porque puede incluir simbolos o caracteres alfabeticos
            public string? Prototipo { get; set; } // <-- Tipo de contrato en String

            public string? NumeroContrato { get; set; }
            public string? TipoDocumentoEmpleado { get; set; }
            public string? NumeroDocumentoEmpleado { get; set; }
            public string? FechaExpedicionEmpleado { get; set; }
            public string? PaisExpedicionDIAN { get; set; }
            public string? DepartamentoExpedicionDANE { get; set; }
            public string? CiudadExpedicion { get; set; } // <-- CADENA DE TEXTO - NOMBRE DE LA CIUDAD
            public string? MunicipioExpedicion { get; set; }
            public string? ApellidosEmpleado { get; set; }
            public string? NombresEmpleado { get; set; }
            public string? SedeEmpleado { get; set; }
            public string? AreaEmpleado { get; set; }
            public string? CargoEmpleado { get; set; }
            public string? EmailInstitucionalEmpleado { get; set; }
            public string? EmailPersonalEmpleado { get; set; }
            public string? TelefonoCasaEmpleado { get; set; }
            public string? CelularEmpleado { get; set; }
            public string? DireccionCasaEmpleado { get; set; }   // <-- Verifiar si deben dejarse espacios sencillos en domicilio   
            public string? BarrioEmpleado { get; set; }
            public string? CiudadResidencia { get; set; } // <-- la ciudad se obtiene desde una validación a través del dato de sede del Colaborador
            public string? TipoContratoEmpleado { get; set; }     // <-- ESTE VALOR SE REPITE MÁS ADELANTE COMO Tipo_Contrato
            public string? FechaInicioContratoEmpleado { get; set; }
            public string? FechaFinContratoEmpleado { get; set; } // <-- En caso de aplicar muestra Fecha fin del contrato
            public string? DuracionContrato { get; set; }              // CLASE NUEVA
            public string? SalarioEmpleado { get; set; }
            public string? SalarioEmpleadoLetras { get; set; } // Se obtiene por un grupo de metodos de .NET, para su obtención en base al elemento Salario_Empleado
            public string? CiudadFirmaContrato { get; set; } // Por default se evidencia que se deja el valor "BOGOTA"
            public string? EstadoEmpleado { get; set; }
            public string? SedeRegional { get; set; }
            public string? FechaConectividad { get; set; } // <-- Hace alusión a la tabla  NM_CONTR@SIGKACTUS a la columna FEC_CONT NM_CONTR@SIGKACTUS(CONECTIVIDAD)
            public string? FechaPersonales { get; set; }
            public string? FechaNacimiento { get; set; }
            public string? PaisNacimiento { get; set; }
            public string? DepartamentoNacimiento { get; set; }
            public string? MunicipioNacimiento { get; set; }
            public string? LugarNacimiento { get; set; } // <-- Departamento o Lugar de nacimiento se haya desde el metodo DeterminarCiudadesKactus a la tabla NM_CENTP@SIGKACTUS
            public string? DepartamentoPersonales { get; set; }  // Por default se evidencia que se deja el valor "BOGOTA"
            public string? CentroDeCostos { get; set; }                // CLASE NUEVA - Mapeada desde GN_CCOST@SIGKACTUS - Columna: NOM_CCOS
            public string? ClaseDeNomina { get; set; }                 // CLASE NUEVA - Mapeada desde NM_TNOMI@SIGKACTUS - Columna: NOM_TNOM
            public string? FechaFinMarcador { get; set; }              // CLASE NUEVA
            public string? FirmaElectronicaHtml2 { get; set; }          // CLASE NUEVA
            public string? TipoContrato { get; set; }                  // CLASE NUEVA - Validar si es la misma clase Tipo_Contrato_Empleado
            public string? InsertarEmpresa { get; set; }                // CLASE NUEVA
            public string? CodigoParaWord { get; set; }                 // CLASE NUEVA
            public string? FirmaElectronicaMod7 { get; set; }          // CLASE NUEVA
            public string? FirmaCapturadaMod7 { get; set; }              // CLASE NUEVA
            public string? Tercero { get; set; } // <-- ¿VALORES NO ESTABLECIDOS DESDE BASE DE DATOS!
            public string? FirmaElectronicaHtml { get; set; }              // CLASE NUEVA
            public string? ValidacionTipoCE { get; set; }              // CLASE NUEVA
            public string? CodContrato { get; set; }              // CLASE NUEVA
            public string? EmpresaPermisos { get; set; } // <-- ¿VALORES NO ESTABLECIDOS DESDE BASE DE DATOS? - Aparece como Usuarios Cun
            public string? Empresa { get; set; } // <-- ¿VALORES NO ESTABLECIDOS DESDE BASE DE DATOS? - Aparece como CUN
            public string? EquipoCapitalSocial { get; set; }              // CLASE NUEVA

            public string? FirmaCapturada2Mod7 { get; set; }              // CLASE NUEVA
            public string? CopiarPDFs { get; set; }                       // CLASE NUEVA
            public string? FirmaElectronica2Mod7 { get; set; }            // CLASE NUEVA
            public string? DatosConcatenados { get; set; }                 // CLASE NUEVA
            public string? DescripcionRegional { get; set; }              // CLASE NUEVA
            public string? Elaboracion { get; set; } // // Aqui contiene el nombre de usuario
            public string? FechaElaboracion { get; set; } // <-- Elaboración [Fecha] - Columna Vacia en el Excel
    }
}
