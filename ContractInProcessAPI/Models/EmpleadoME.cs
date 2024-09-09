namespace ContractInProcessAPI.Models
{
    public class EmpleadoME
    {
        // POSIBLES DATOS A IMPLEMENTAR  Al convertir validar el tipo de dato
        public string? Import_Id { get; set; }
        public string? Fecha_Creacion { get; set; } // <--  CLASE NUEVA - CreationDate en DB
        public string? Creado_Por_Id { get; set; }   // <-- CLASE NUEVA - CreatedBy (Por validar)
        public string? Contrato_Concatenado { get; set; }   // <-- AlternativeId - STRING (I-1030587912-1-01) - Presuntamente es la concatenacion de CC + Tipo de Contrato - No se sabe si de agente o inscrito
        public string? Version { get; set; }   // <--  Se ajusta como tipo de dato string, porque puede incluir simbolos o caracteres alfabeticos
        public string? Prototipo { get; set; } // <-- Tipo de contrato en String
        public string? Numero_Contrato { get; set; }
        public string? Tipo_Documento_Empleado { get; set; }
        public string? Numero_Documento_Empleado { get; set; }
        public string? Fecha_Expedicion_Empleado { get; set; }
        public string? Pais_Expedicion_DIAN { get; set; }
        public string? Departamento_Expedicion_DANE { get; set; }
        public string? Municipio_Expedicion { get; set; }
        public string? Ciudad_Expedicion { get; set; } // <-- CADENA DE TEXTO - NOMBRE DE LA CIUDAD
        public string? Apellidos_Empleado { get; set; }
        public string? Nombres_Empleado { get; set; }
        public string? Sede_Empleado { get; set; }
        public string? Area_Empleado { get; set; }
        public string? Cargo_Empleado { get; set; }
        public string? Email_Institucional_Empleado { get; set; }
        public string? Email_Personal_Empleado { get; set; }
        public string? Telefono_Casa_Empleado { get; set; }
        public string? Celular_Empleado { get; set; }
        public string? Direccion_Casa_Empleado { get; set; }   // <-- Verifiar si deben dejarse espacios sencillos en domicilio   
        public string? Barrio_Empleado { get; set; }
        public string? Ciudad_Residencia { get; set; } // <-- la ciudad se obtiene desde una validación a través del dato de sede del Colaborador
        public string? Tipo_Contrato_Empleado { get; set; }
        public string? Fecha_Inicio_Contrato_Empleado { get; set; }
        public string? Fecha_Fin_Contrato_Empleado { get; set; } // <-- En caso de aplicar muestra Fecha fin del contrato
        public string? Salario_Empleado { get; set; }
        public string? Salario_Empleado_Letras { get; set; } // Se obtiene por un grupo de metodos de .NET, para su obtención en base al elemento Salario_Empleado
        public string? Ciudad_Firma_Contrato { get; set; } // Por default se evidencia que se deja el valor "BOGOTA"
        public string? Estado_Empleado { get; set; }
        public string? Sede_Regional { get; set; }
        public string? Fecha_Conectividad { get; set; } // <-- Hace alusión a la tabla  NM_CONTR@SIGKACTUS a la columna FEC_CONT NM_CONTR@SIGKACTUS(CONECTIVIDAD)
        public string? Fecha_Personales { get; set; }
        public string? Fecha_Nacimiento { get; set; }
        public string? Pais_Nacimiento { get; set; }
        public string? Departamento_Nacimiento { get; set; }
        public string? Municipio_Nacimiento { get; set; }
        public string? Lugar_Nacimiento { get; set; } // <-- Departamento o Lugar de nacimiento se haya desde el metodo DeterminarCiudadesKactus a la tabla NM_CENTP@SIGKACTUS
        public string? Departamento_Personales { get; set; }  // Por default se evidencia que se deja el valor "BOGOTA"

        // ¡ATENCIÓN! -  // <-- ¿VALORES NO ESTABLECIDOS DESDE BASE DE DATOS!
        public string? Firma_Electronica { get; set; } // <-- De acuerdo a especificaciones aqui se coloca un guion sencillo "-"
        public string? Tercero { get; set; } // <-- ¿VALORES NO ESTABLECIDOS DESDE BASE DE DATOS!
        public string? Empresa_Permisos { get; set; } // <-- ¿VALORES NO ESTABLECIDOS DESDE BASE DE DATOS!
        public string? Empresa { get; set; } // <-- ¿VALORES NO ESTABLECIDOS DESDE BASE DE DATOS!

        //public string? Elaboración { get; set; } // // Aqui contiene el nombre de usuario (Version alterna)

        public string? Elaboración { get; set; } // // Aqui contiene el nombre de usuario
        public string? Fecha_Elaboracion { get; set; } // <-- Elaboración [Fecha] - Columna Vacia en el Excel


        // CAMPOS NO APARECEN EN EL FORMULARIO, PERO QUE ENTREGO ANTERIOR DESARROLLADOR NICOLAS ALVARADO - ENVIADO POR TATIANA RINCON
        // VERIFICAR ESTAS CLASES ----------------------------------------------------------------------------------------------------
        //public string Nom_Centro_Costo { get; set; }
        //public string Clase_Nomina { get; set; }
        //public string Regional { get; set; }
        //public string Datos_concatenados { get; set; }
        // ----------------------------------------------------------------------------------------------------------------------------
    }
}
