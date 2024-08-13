using System.Diagnostics.CodeAnalysis;

namespace ContractInProcessAPI.Conversor
{
    public static class Conversor
    {
        public static string NumeroALetras(this decimal number)
        {
            var entero = (long)Math.Truncate(number);
            var decimales = (int)Math.Round((number - entero) * 100, 2);
            var letras = NumeroALetras(entero);
            if (decimales > 0)
            {
                letras += " CON " + NumeroALetras(decimales) + " CENTAVOS";
            }
            return letras;
        }

        private static string NumeroALetras(long value)
        {
            if (value == 0) return "CERO";
            if (value < 0) return "MENOS " + NumeroALetras(Math.Abs(value));

            var result = "";

            if (value >= 1000000000000)
            {
                result += NumeroALetras(value / 1000000000000) + " BILLONES ";
                value %= 1000000000000;
            }
            if (value >= 1000000000)
            {
                result += NumeroALetras(value / 1000000000) + " MIL MILLONES ";
                value %= 1000000000;
            }
            if (value >= 1000000)
            {
                result += NumeroALetras(value / 1000000) + " MILLONES ";
                value %= 1000000;
            }
            if (value >= 1000)
            {
                result += NumeroALetras(value / 1000) + " MIL ";
                value %= 1000;
            }
            if (value >= 100)
            {
                result += Centenas(value / 100);
                value %= 100;
            }
            if (value >= 10)
            {
                result += Decenas(value);
            }
            else if (value > 0)
            {
                result += Unidades(value);
            }

            return result.Trim();
        }

        private static string Centenas(long value)
        {
            return value switch
            {
                1 => "CIEN",
                2 => "DOSCIENTOS",
                3 => "TRESCIENTOS",
                4 => "CUATROCIENTOS",
                5 => "QUINIENTOS",
                6 => "SEISCIENTOS",
                7 => "SETECIENTOS",
                8 => "OCHOCIENTOS",
                9 => "NOVECIENTOS",
                _ => ""
            };
        }

        private static string Decenas(long value)
        {
            if (value < 10) return "";
            if (value < 20)
            {
                // Manejo de casos especiales entre 10 y 19
                switch (value)
                {
                    case 10: return "DIEZ";
                    case 11: return "ONCE";
                    case 12: return "DOCE";
                    case 13: return "TRECE";
                    case 14: return "CATORCE";
                    case 15: return "QUINCE";
                    case 16: return "DIECISEIS";
                    case 17: return "DIECISIETE";
                    case 18: return "DIECIOCHO";
                    case 19: return "DIECINUEVE";
                    default: return ""; // Aunque no debería llegar aquí
                }
            }
            if (value < 30) return "VEINTI" + Unidades(value - 20);
            if (value == 30) return "TREINTA";
            if (value == 40) return "CUARENTA";
            if (value == 50) return "CINCUENTA";
            if (value == 60) return "SESENTA";
            if (value == 70) return "SETENTA";
            if (value == 80) return "OCHENTA";
            if (value == 90) return "NOVENTA";
            return "Y " + Unidades(value % 10);
        }

        private static string Unidades(long value)
        {
            return value switch
            {
                1 => "UNO",
                2 => "DOS",
                3 => "TRES",
                4 => "CUATRO",
                5 => "CINCO",
                6 => "SEIS",
                7 => "SIETE",
                8 => "OCHO",
                9 => "NUEVE",
                _ => ""
            };
        }
    }

}