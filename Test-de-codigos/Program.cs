using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
class Program
{
    static void Main(string[] args)
    {
        // Definir las tasas de conversión entre las monedas
        // La matriz ahora está configurada para que [i,j] sea la tasa de conversión de la moneda i a la moneda j
        double[,] conversionMatrizMoneda = new double[,] {
            {1.0,    0.8993,   137.58, 0.83,   1.48,  1.37,  0.88,  7.12,  10.37, 1.62},  // USD a otras
            {1.08,   1.0,    161.828, 0.89,   1.59,  1.48,  0.94,  7.66,  11.13, 1.74},  // EUR a otras
            {0.0073, 0.0068, 1.0,    0.0060, 0.0108,0.0100,0.0064,0.0520,0.0750,0.012}, // JPY a otras
            {1.21,   1.12,   165.43, 1.0,    1.79,  1.66,  1.12,  8.05,  11.70, 1.95},  // GBP a otras
            {0.68,   0.63,   92.80,  0.56,   1.0,   0.93,  0.63,  4.49,  6.63, 1.09},  // AUD a otras
            {0.73,   0.68,   100.23, 0.60,   1.08,  1.0,   0.68,  4.82,  7.05, 1.17},  // CAD a otras
            {1.14,   1.06,   156.12, 0.93,   1.81,  1.21,  1.0,   7.12,  10.40, 1.70},  // CHF a otras
            {0.14,   0.13,   19.23,  0.12,   0.22,  0.21,  0.14,  1.0,   1.43, 0.24},  // CNY a otras
            {0.096,  0.09,   13.98,  0.085,  0.15,  0.14,  0.096,0.70,  1.0,   0.17},  // SEK a otras
            {0.62,   0.57,   83.14,  0.51,   0.92,  0.85,  0.59,  4.20,  6.27, 1.0}    // NZD a otras
        };

          double[,] conversionMatrizTiempo = new double[,] {
    {1.0,     0.0167,  0.00027778, 0.00001157, 0.00000165, 0.00000055, 0.0000000167, 0.000000001, 0.0000000001, 1000},   // segundos a otras
    {60.0,    1.0,     0.0167,     0.00083333, 0.00011905, 0.0000397,  0.00000114, 0.0000000833, 0.000000002, 60000},   // minutos a otras
    {3600.0,  60.0,    1.0,        0.04166667, 0.00595238, 0.00138889, 0.00004167, 0.00000278, 0.0000001, 3600000}, // horas a otras
    {86400.0, 1440.0,  24.0,       1.0,        0.142857,  0.0208333,  0.00273973, 0.00019444, 0.00000685, 86400000}, // días a otras
    {604800.0, 10080.0, 168.0,      7.0,        1.0,       0.142857,  0.0208333, 0.001488,  0.0000506, 604800000}, // semanas a otras
    {2628000.0, 43800.0, 730.0,     30.0,       7.0,       1.0,       0.0833333, 0.0056,   0.000189, 2628000000}, // meses a otras
    {31536000.0, 525600.0, 8760.0,  365.0,      52.0,      12.0,      1.0,       0.0833,  0.00274, 31536000000}, // años a otras
    {315360000.0, 5256000.0, 87600.0, 3650.0,   520.0,     120.0,     10.0,      1.0,    0.0374, 315360000000}, // décadas a otras
    {3153600000.0, 52560000.0, 876000.0, 36500.0, 5200.0,  1200.0,    100.0,     10.0,   1.0,    3153600000000}, // siglos a otras
    {0.001,   1.6667e-05, 2.7778e-07, 1.1574e-08, 1.6534e-09, 5.55e-10,  1.67e-11, 1.0e-12, 1.0e-13, 1.0} // milisegundos a otras
};

        // Matriz de conversión de magnitudes de masa
        double[,] conversionMatrizMasa = new double[,] {
    {1.0,        0.001,      1e-6,        1e-9,        1e-12,        0.00220462, 0.03527396, 1.0e-9,      1.0e-6,     1e-12},    // g a otras
    {1000.0,     1.0,        0.001,       1e-6,        1e-9,         2.20462,   35.27396,   1.0e-6,      0.001,      1e-9},     // kg a otras
    {1e6,        1000.0,     1.0,         0.001,       1e-6,         2204.62,   35273.96,   0.001,       1.0,        1e-6},     // t a otras
    {1e9,        1e6,        1000.0,      1.0,         0.001,        2204622.0, 35273965.0, 1.0,         1000.0,     0.001},    // kt a otras
    {1e12,       1e9,        1e6,         1000.0,      1.0,          2.2046e9,  3.5274e10,  1e6,         1.0e9,      1000.0},   // Mt a otras
    {453.592,    0.453592,   0.000453592, 4.53592e-7,  4.53592e-10,  1.0,       16.0,       4.5359e-7,   4.5359e-4,  4.5359e-10}, // lb a otras
    {28.3495,    0.0283495,  2.83495e-5,  2.83495e-8,  2.83495e-11,  0.0625,    1.0,        2.83495e-8,  2.83495e-5, 2.83495e-11}, // oz a otras
    {1.0e9,      1.0e6,      1000.0,      1.0,         0.001,        2.2046e6,  35273.96,   1.0,         1000.0,     0.001},   // µg a otras
    {1e6,        1000.0,     1.0,         0.001,       1e-6,         2204.62,   35273.96,   0.001,       1.0,        1e-6},    // mg a otras
    {1e12,       1e9,        1e6,         1000.0,      1.0,          2.2046e9,  3.5274e10,  1e6,         1.0e9,      1000.0}   // Gg a otras
};


        // Matriz de conversión de magnitudes de volumen
        double[,] conversionMatrizVolumen = new double[,] {
    {1.0,        1000.0,      1e6,         1e9,         1e12,        1e15,         0.001,         3.78541,     33.814,      28.3168},    // L a otras
    {0.001,      1.0,         1000.0,      1e6,         1e9,         1e12,         1e-6,          0.00378541,  0.033814,    0.0283168},  // mL a otras
    {1e-6,       0.001,       1.0,         1000.0,      1e6,         1e9,          1e-9,          3.7854e-6,   2.9574e-8,   2.8317e-5},  // cm³ a otras
    {1e-9,       1e-6,        0.001,       1.0,         1000.0,      1e6,          1e-12,         3.7854e-9,   2.9574e-11,  2.8317e-8},  // mm³ a otras
    {1e-12,      1e-9,        1e-6,        0.001,       1.0,         1000.0,       1e-15,         3.7854e-12,  2.9574e-14,  2.8317e-11}, // µL a otras
    {1e-15,      1e-12,       1e-9,        1e-6,        0.001,       1.0,          1e-18,         3.7854e-15,  2.9574e-17,  2.8317e-14}, // nL a otras
    {1000.0,     1e6,         1e9,         1e12,        1e15,        1e18,         1.0,           3.7854e3,    3.3814e4,    2.8317e5},   // kL a otras
    {0.264172,   264.172,     264172.0,    2.64172e8,   2.64172e11,  2.64172e14,   2.64172e-4,    1.0,         128.0,       7.48052},    // gal (US) a otras
    {0.0295735,  29.5735,     2.95735e4,   2.95735e7,   2.95735e10,  2.95735e13,   2.95735e-5,    0.0078125,   1.0,         0.0353147},  // fl oz (US) a otras
    {0.0353147,  35.3147,     35314.7,     3.53147e7,   3.53147e10,  3.53147e13,   3.53147e-5,    0.133681,    28.3168,     1.0}         // ft³ a otras
};

        // Matriz de conversión de magnitudes de longitud
        double[,] conversionMatrizLongitud = new double[,] {
    {1.0,        100.0,      1000.0,    1000000.0,   1e9,        1e10,      0.001,     0.000621371, 39.3701,  3.28084},    // m a otras
    {0.01,       1.0,        10.0,      10000.0,     1e7,        1e8,       1e-5,      0.00000621371, 0.393701, 0.0328084},  // cm a otras
    {0.001,      0.1,        1.0,       1000.0,      1e6,        1e7,       1e-6,      0.000000621371, 0.0393701, 0.00328084}, // mm a otras
    {0.000001,   0.0001,     0.001,     1.0,         1000.0,     10000.0,   1e-9,      0.000000000621371, 0.0000393701, 0.00000328084}, // µm a otras
    {1e-9,       1e-7,       0.000001,  0.001,       1.0,        10.0,      1e-12,     0.000000000000621371, 0.0000000393701, 0.00000000328084}, // nm a otras
    {1e-10,      1e-8,       1e-7,      0.0001,      0.1,        1.0,       1e-13,     0.0000000000000621371, 0.00000000393701, 0.000000000328084}, // Å a otras
    {1000.0,     100000.0,   1000000.0, 1e9,         1e12,       1e13,      1.0,       0.621371,    39370.1,  3280.84},    // km a otras
    {1609.34,    160934.0,   1609344.0, 1.609e9,     1.609e12,   1.609e13,  1.60934,   1.0,         63360.0,  5280.0},     // mi a otras
    {0.0254,     2.54,       25.4,      25400.0,     2.54e7,     2.54e8,    2.54e-5,   0.0000157828, 1.0,      0.0833333},  // in a otras
    {0.3048,     30.48,      304.8,     304800.0,    3.048e8,    3.048e9,   0.0003048, 0.000189394, 12.0,      1.0}        // ft a otras
};
        // Matriz de conversión de magnitudes de almacenamiento
        // Matriz de conversión de magnitudes de almacenamiento
        double[,] conversionMatrizAlmacenamiento = new double[,] {
   // Matriz de conversión de magnitudes de almacenamiento
    {1.0,        1e-3,       1e-6,        1e-9,        1e-12,       1e-15,       1e-18,       1e-21,       1e-24,       1e-27},  // B a otras
    {1e3,        1.0,        1e-3,        1e-6,        1e-9,        1e-12,       1e-15,       1e-18,       1e-21,       1e-24},  // KB a otras
    {1e6,        1e3,        1.0,         1e-3,        1e-6,        1e-9,        1e-12,       1e-15,       1e-18,       1e-21},  // MB a otras
    {1e9,        1e6,        1e3,         1.0,         1e-3,        1e-6,        1e-9,        1e-12,       1e-15,       1e-18},  // GB a otras
    {1e12,       1e9,        1e6,         1e3,         1.0,         1e-3,        1e-6,        1e-9,        1e-12,       1e-15},  // TB a otras
    {1e15,       1e12,       1e9,         1e6,         1e3,         1.0,         1e-3,        1e-6,        1e-9,        1e-12},  // PB a otras
    {1e18,       1e15,       1e12,        1e9,         1e6,         1e3,         1.0,         1e-3,        1e-6,        1e-9},   // EB a otras
    {1e21,       1e18,       1e15,        1e12,        1e9,         1e6,         1e3,         1.0,         1e-3,        1e-6},  // ZB a otras
    {1e24,       1e21,       1e18,        1e15,        1e12,        1e9,        1e6,         1e3,         1.0,         1e-3},  // YB a otras
    {1e27,       1e24,       1e21,        1e18,        1e15,        1e12,       1e9,         1e6,         1e3,         1.0}    // BB a otras
};



        // Nombre de unidades de tiempo

        Console.WriteLine("*** MENU ***");
        Console.WriteLine("Seleccione el tipo de conversión:");
        Console.WriteLine("1. Conversión de monedas");
        Console.WriteLine("2. Conversión de tiempo");
        Console.WriteLine("3. Conversión de masa");
        Console.WriteLine("4. Conversión de volumen");
        Console.WriteLine("5. Conversión de longitud");
        Console.WriteLine("6. Conversión de almacenamiento");
        Console.Write("Ingrese su opción (): ");
        int tipoConversion = int.Parse(Console.ReadLine());


        switch (tipoConversion)
        {
            case 1:
                ConversorMoneda(conversionMatrizMoneda);
                break;

            case 2:
                ConversorTiempo(conversionMatrizTiempo);
                break;

            case 3:
                ConversorMasa(conversionMatrizMasa);
                break;

            case 4:
                ConversorVolumen(conversionMatrizVolumen);
                break;

            case 5:
                ConversorLongitud(conversionMatrizLongitud);
                break;

            case 6:
                ConversorAlmacenamiento(conversionMatrizAlmacenamiento);
                break;

            default:
                Console.WriteLine("Opción inválida.");
                break;
        }
    }
    public static void ConversorMoneda(double[,] matriz)
    {
        /* USD - Estados Unidos
           EUR - Eurozona (países de la Unión Europea que usan el Euro)
           JPY - Japón
           GBP - Reino Unido
           AUD - Australia
           CAD - Canadá
           CHF - Suiza y Liechtenstein
           CNY - China
           SEK - Suecia
           NZD - Nueva Zelanda 
         */

        // Nombre de monedas
        string[] monedas = { "USD", "EUR", "JPY", "GBP", "AUD", "CAD", "CHF", "CNY", "SEK", "NZD" };

        // Mostrar opciones de monedas
        Console.WriteLine("Opciones de moneda:");
        int contador = 0;
        foreach (var moneda in monedas)
        {
            Console.WriteLine($"{contador}: {moneda}");
            contador++;
        }


        // Solicitar al usuario el monto a convertir
        Console.Write("Ingrese el monto a convertir: ");
        double monto;

        monto = double.Parse(Console.ReadLine());
        if (monto <= 0)
        {
            Console.WriteLine("Monto inválido. Debe ingresar un número positivo.");
            Console.ReadLine();
            return;
        }


        // Solicitar al usuario la moneda de origen
        Console.Write("Seleccione la moneda de origen (0-9): ");
        int Valormoneda;

        Valormoneda = int.Parse(Console.ReadLine());
        // Verificar si el valor está dentro del rango permitido
        if (Valormoneda < 0 || Valormoneda >= monedas.Length)
        {
            Console.WriteLine("Selección de moneda de origen inválida.");
            Console.ReadLine();
            return;
        }

        // Solicitar al usuario la moneda de destino
        Console.Write("Seleccione la moneda de destino (0-9): ");
        int Valormonedadestino;

        Valormonedadestino = int.Parse(Console.ReadLine());

        if (Valormonedadestino < 0 || Valormonedadestino >= monedas.Length)
        {
            Console.WriteLine("Selección de moneda de destino inválida.");
            return;
        }

        // Convertir y mostrar el resultado
        double convertirmonto = ConvertirMonedas(monto, Valormoneda, Valormonedadestino, matriz);
        Console.WriteLine($"Monto convertido: {convertirmonto} {monedas[Valormonedadestino]}");

        Console.ReadLine(); // Mantener la consola abierta


    } 

    public static double ConvertirMonedas(double monto, int Valormoneda, int Valormonedadestino, double[,] matriz)
    {
        // Convertir el monto de la moneda de origen a la moneda de destino
        double conversionTasa = matriz[Valormoneda, Valormonedadestino];
        double convertirmonto = monto * conversionTasa;
        return convertirmonto;
    }



    public static void ConversorTiempo(double[,] matriz)
    {
        /* Segundos (s)
            Minutos (min)
            Horas (h)
            Días (d)
            Semanas (w)
            Meses (mo)
            Años (y)
            Decadas (dec)
            Siglos (c)
            Milisegundos (ms) */

        // Nombre de monedas
        string[] monedas = { "s", "min", "h", "d", "w", "mo", "y", "dec", "c", "ms" };

        // Mostrar opciones de monedas
        Console.WriteLine("Opciones de Tiempo:");
        int contador = 0;
        foreach (var moneda in monedas)
        {
            Console.WriteLine($"{contador}: {moneda}");
            contador++;
        }


        // Solicitar al usuario el monto a convertir
        Console.Write("Ingrese la magnitud a convertir: ");
        double monto;

        monto = double.Parse(Console.ReadLine());
        if (monto <= 0)
        {
            Console.WriteLine("Valor inválido. Debe ingresar un numero.");
            Console.ReadLine();
            return;
        }


        // Solicitar al usuario la moneda de origen
        Console.Write("Seleccione la magnitud de tiempo de origen (0-9): ");
        int Valormoneda;

        Valormoneda = int.Parse(Console.ReadLine());
        // Verificar si el valor está dentro del rango permitido
        if (Valormoneda < 0 || Valormoneda >= monedas.Length)
        {
            Console.WriteLine("Seleccion de magnitud de tiempo de origen inválida.");
            Console.ReadLine();
            return;
        }

        // Solicitar al usuario la moneda de destino
        Console.Write("Seleccione la magnitud de tiempo de destino (0-9): ");
        int Valormonedadestino;

        Valormonedadestino = int.Parse(Console.ReadLine());

        if (Valormonedadestino < 0 || Valormonedadestino >= monedas.Length)
        {
            Console.WriteLine("Selección de magnitud de tiempo de destino inválida.");
            return;
        }

        // Convertir y mostrar el resultado
        double convertirmonto = ConvertirTiempo(monto, Valormoneda, Valormonedadestino, matriz);
        Console.WriteLine($"Magnitud de tiempo convertida: {convertirmonto} {monedas[Valormonedadestino]}");

        Console.ReadLine(); // Mantener la consola abierta


    }

    public static double ConvertirTiempo(double monto, int Valormoneda, int Valormonedadestino, double[,] matriz)
    {
        // Convertir el monto de la moneda de origen a la moneda de destino
        double conversionTasa = matriz[Valormoneda, Valormonedadestino];
        double convertirmonto = monto * conversionTasa;
        return Math.Round(convertirmonto);
        ;
    }

    public static void ConversorMasa(double[,] matriz)
    {


        // Nombre de monedas 
        /* g (gramo)
           kg(kilogramo)
           t(tonelada)
           kt(kilotonelada)
           Mt(megatonelada)
           lb(libra)
           oz(onza)
           µg(microgramo)
           mg(miligramo)
           Gg(gigagramo */
        string[] monedas = { "g", "kg", "t", "kt", "mt", "lb", "oz", "µg", "mg", "cg" };

        // Mostrar opciones de monedas
        Console.WriteLine("Opciones de Masa:");
        int contador = 0;
        foreach (var moneda in monedas)
        {
            Console.WriteLine($"{contador}: {moneda}");
            contador++;
        }


        // Solicitar al usuario el monto a convertir
        Console.Write("Ingrese la magnitud a convertir: ");
        double monto;

        monto = double.Parse(Console.ReadLine());
        if (monto <= 0)
        {
            Console.WriteLine("Valor inválido. Debe ingresar un numero.");
            Console.ReadLine();
            return;
        }


        // Solicitar al usuario la moneda de origen
        Console.Write("Seleccione la magnitud de masa de origen (0-9): ");
        int Valormoneda;

        Valormoneda = int.Parse(Console.ReadLine());
        // Verificar si el valor está dentro del rango permitido
        if (Valormoneda < 0 || Valormoneda >= monedas.Length)
        {
            Console.WriteLine("Seleccion de magnitud de masa de origen inválida.");
            Console.ReadLine();
            return;
        }

        // Solicitar al usuario la moneda de destino
        Console.Write("Seleccione la magnitud de masa de destino (0-9): ");
        int Valormonedadestino;

        Valormonedadestino = int.Parse(Console.ReadLine());

        if (Valormonedadestino < 0 || Valormonedadestino >= monedas.Length)
        {
            Console.WriteLine("Selección de magnitud de masa de destino inválida.");
            return;
        }

        // Convertir y mostrar el resultado
        double convertirmonto = ConvertirMasa(monto, Valormoneda, Valormonedadestino, matriz);
        Console.WriteLine($"Magnitud de masa convertida: {convertirmonto} {monedas[Valormonedadestino]}");

        Console.ReadLine(); // Mantener la consola abierta


    }

    public static double ConvertirMasa(double monto, int Valormoneda, int Valormonedadestino, double[,] matriz)
    {
        // Convertir el monto de la moneda de origen a la moneda de destino
        double conversionTasa = matriz[Valormoneda, Valormonedadestino];
        double convertirmonto = monto * conversionTasa;
        return convertirmonto;
    }


    public static void ConversorVolumen(double[,] matriz)
    {


        // Nombre de monedas 
        /* L (Litro)
           mL (Mililitro)
           cm³ (Centímetro cúbico)
           mm³ (Milímetro cúbico)
           µL (Microlitro)
           nL (Nanolitro)
           kL (Kilolitro)
           gal (US) (Galón estadounidense)
           fl oz (US) (Onza líquida estadounidense)
           ft³ (Pie cúbico)
           */
        string[] monedas = { "L", "mL", "cm³", "mm³", "µL", "nL", "kL", "gal", "fl", "ft" };

        // Mostrar opciones de monedas
        Console.WriteLine("Opciones de volumen:");
        int contador = 0;
        foreach (var moneda in monedas)
        {
            Console.WriteLine($"{contador}: {moneda}");
            contador++;
        }


        // Solicitar al usuario el monto a convertir
        Console.Write("Ingrese la magnitud a convertir: ");
        double monto;

        monto = double.Parse(Console.ReadLine());
        if (monto <= 0)
        {
            Console.WriteLine("Valor inválido. Debe ingresar un numero.");
            Console.ReadLine();
            return;
        }


        // Solicitar al usuario la moneda de origen
        Console.Write("Seleccione la magnitud de volumen de origen (0-9): ");
        int Valormoneda;

        Valormoneda = int.Parse(Console.ReadLine());
        // Verificar si el valor está dentro del rango permitido
        if (Valormoneda < 0 || Valormoneda >= monedas.Length)
        {
            Console.WriteLine("Seleccion de magnitud de volumen de origen inválida.");
            Console.ReadLine();
            return;
        }

        // Solicitar al usuario la moneda de destino
        Console.Write("Seleccione la magnitud de volumen de destino (0-9): ");
        int Valormonedadestino;

        Valormonedadestino = int.Parse(Console.ReadLine());

        if (Valormonedadestino < 0 || Valormonedadestino >= monedas.Length)
        {
            Console.WriteLine("Selección de magnitud de volumen de destino inválida.");
            return;
        }

        // Convertir y mostrar el resultado
        double convertirmonto = ConvertirVolumen(monto, Valormoneda, Valormonedadestino, matriz);
        Console.WriteLine($"Magnitud de volumen convertida: {convertirmonto} {monedas[Valormonedadestino]}");

        Console.ReadLine(); // Mantener la consola abierta


    }

    public static double ConvertirVolumen(double monto, int Valormoneda, int Valormonedadestino, double[,] matriz)
    {
        // Convertir el monto de la moneda de origen a la moneda de destino
        double conversionTasa = matriz[Valormoneda, Valormonedadestino];
        double convertirmonto = monto * conversionTasa;
        return convertirmonto;
    }

    public static void ConversorLongitud(double[,] matriz)
    {


        // Nombre de monedas 
        /* m (Metro)
cm (Centímetro)
mm (Milímetro)
µm (Micrómetro)
nm (Nanómetro)
Å (Ångström)
km (Kilómetro)
mi (Milla)
in (Pulgada)
ft (Pie)
           */
        string[] monedas = { "L", "mL", "cm³", "mm³", "µL", "nL", "kL", "gal", "fl", "ft" };

        // Mostrar opciones de longitud
        Console.WriteLine("Opciones de longitud:");
        int contador = 0;
        foreach (var moneda in monedas)
        {
            Console.WriteLine($"{contador}: {moneda}");
            contador++;
        }


        // Solicitar al usuario el longitud a convertir
        Console.Write("Ingrese la magnitud que quiere convertir: ");
        double monto;

        monto = double.Parse(Console.ReadLine());
        if (monto <= 0)
        {
            Console.WriteLine("Valor inválido. Debe ingresar un numero.");
            Console.ReadLine();
            return;
        }


        // Solicitar al usuario la longitud de origen
        Console.Write("Seleccione la magnitud de longitud de origen (0-9): ");
        int Valormoneda;

        Valormoneda = int.Parse(Console.ReadLine());
        // Verificar si el valor está dentro del rango permitido
        if (Valormoneda < 0 || Valormoneda >= monedas.Length)
        {
            Console.WriteLine("Seleccion de magnitud longitud de origen inválida.");
            Console.ReadLine();
            return;
        }

        // Solicitar al usuario la longitud de destino
        Console.Write("Seleccione la magnitud de longitud de destino (0-9): ");
        int Valormonedadestino;

        Valormonedadestino = int.Parse(Console.ReadLine());

        if (Valormonedadestino < 0 || Valormonedadestino >= monedas.Length)
        {
            Console.WriteLine("Selección de magnitud de longitud de destino inválida.");
            return;
        }

        // Convertir y mostrar el resultado
        double convertirmonto = ConvertirVolumen(monto, Valormoneda, Valormonedadestino, matriz);
        Console.WriteLine($"Magnitud de longitud convertida: {convertirmonto} {monedas[Valormonedadestino]}");

        Console.ReadLine(); // Mantener la consola abierta


    }

    public static double ConvertirLongitud(double monto, int Valormoneda, int Valormonedadestino, double[,] matriz)
    {
        // Convertir el monto de la moneda de origen a la longitud de destino
        double conversionTasa = matriz[Valormoneda, Valormonedadestino];
        double convertirmonto = monto * conversionTasa;
        return convertirmonto;
    }

    public static void ConversorAlmacenamiento(double[,] matriz)
    {


        // Nombre 
        /* B (Byte)
        KB (Kilobyte)
        MB (Megabyte)
        GB (Gigabyte)
        TB (Terabyte)
        PB (Petabyte)
        EB (Exabyte)
        ZB (Zetabyte)
        YB (Yottabyte)
        BB (Brontobyte)
           */
        string[] monedas = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB", "BB" };

        // Mostrar opciones de Almacenamiento
        Console.WriteLine("Opciones de Almacenamiento:");
        int contador = 0;
        foreach (var moneda in monedas)
        {
            Console.WriteLine($"{contador}: {moneda}");
            contador++;
        }


        // Solicitar al usuario el Almacenamiento a convertir
        Console.Write("Ingrese la magnitud a convertir: ");
        double monto;

        monto = double.Parse(Console.ReadLine());
        if (monto <= 0)
        {
            Console.WriteLine("Valor inválido. Debe ingresar un numero.");
            Console.ReadLine();
            return;
        }


        // Solicitar al usuario la Almacenamiento de origen
        Console.Write("Seleccione la magnitud de Almacenamiento de origen (0-9): ");
        int Valormoneda;

        Valormoneda = int.Parse(Console.ReadLine());
        // Verificar si el valor está dentro del rango permitido
        if (Valormoneda < 0 || Valormoneda >= monedas.Length)
        {
            Console.WriteLine("Seleccion de magnitud de Almacenamiento de origen inválida.");
            Console.ReadLine();
            return;
        }

        // Solicitar al usuario la Almacenamiento de destino
        Console.Write("Seleccione la magnitud de Almacenamiento de destino (0-9): ");
        int Valormonedadestino;

        Valormonedadestino = int.Parse(Console.ReadLine());

        if (Valormonedadestino < 0 || Valormonedadestino >= monedas.Length)
        {
            Console.WriteLine("Selección de magnitud de Almacenamiento de destino inválida.");
            return;
        }

        // Convertir y mostrar el resultado
        double convertirmonto = ConvertirVolumen(monto, Valormoneda, Valormonedadestino, matriz);
        Console.WriteLine($"Magnitud de Almacenamiento convertida: {convertirmonto} {monedas[Valormonedadestino]}");

        Console.ReadLine(); // Mantener la consola abierta


    }

    public static double ConvertirAlmacenamiento(double monto, int Valormoneda, int Valormonedadestino, double[,] matriz)
    {
        // Convertir el monto de la longitud de origen a la moneda de destino
        double conversionTasa = matriz[Valormoneda, Valormonedadestino];
        double convertirmonto = monto * conversionTasa;
        return convertirmonto;
    }
}
