using static System.Console;
using System.Text;
using System.Text.RegularExpressions;


Console.OutputEncoding = Encoding.UTF8;
//Matriz
int[,] original = new int[1,1]; //Inicializo con un valor por defecto
int tamañoFilas = 0;
int tamañoColumnas = 0;
//Vectores
string[] letras =
{
    " ","A", "B", "C", "D", "E", "F", "G"
};
string[] numeros =
{
    "1", "2", "3", "4", "5", "6", "7", "8", "9"
};

//Random
Random rnd = new();


// Contadores Globales
const double PrecioButaca = 6.5;
int contadorCompras = 0;

bool cineConfigurado = false;
do
{
    try
    {
        ConfigurarSala(ref original);
        cineConfigurado = true;
        Clear();
    }
    catch (ArgumentNullException)
    {
        WriteLine("ERROR: Faltan argumentos. Formato: -filas:X -columnas:Y");

    }
    catch (FormatException)
    {
        WriteLine("ERROR: Formato inválido. Use F:C. Ejemplo: 6:9");

    }
    catch (ArgumentOutOfRangeException)
    {
        WriteLine("ERROR: Rango inválido. Filas: [4-7], Columnas: [5-9].");

    }
} while (!cineConfigurado);

RellenarButacas(original); //Rellenamos Butacas de 0 y 2 (0 para libres y 2 para fuera de servicio) 
MostrarTabla(original); //Mostramos cómo quedaría la tabla de primeras




return;

void ConfigurarSala(ref int[,] valores)
{
    bool inputVálido = false;
    int primerNum;
    int segundoNum;
    do
    {
        WriteLine("=====CONFIGURAR SALA DE CINE=====");
        WriteLine("Introduce filas [4, 7] : columnas [5,9]: ");
        string? input = ReadLine()?.Trim() ?? "";

        var inputConfiguración = input.Split(':');
        if (string.IsNullOrEmpty(input))
        {
            throw new ArgumentNullException();
        }
        else if (inputConfiguración.Length!= 2 || !int.TryParse(inputConfiguración[0], out primerNum) || !int.TryParse(inputConfiguración[1], out segundoNum))
        {
            throw new FormatException();
        }
        primerNum = int.TryParse(inputConfiguración[0], out int n1) ? n1  : 0;
        segundoNum = int.TryParse(inputConfiguración[1], out int n2) ? n2 : 0;
        if ((primerNum < 4 || primerNum > 7) || (segundoNum < 5 || segundoNum > 9))
        {
            throw new ArgumentOutOfRangeException();
        }
        else
        {
            WriteLine($"Sala configurada a {primerNum} filas y {segundoNum} columnas");
            inputVálido = true;
        }
    } while (!inputVálido);

    tamañoFilas = primerNum;
    tamañoColumnas = segundoNum;
    int[,] nuevoArray = new int[primerNum + 1, segundoNum]; // +1 porque la fila 0 es la cabecera
    valores = nuevoArray;
}


void RellenarButacas(int[,] valores)
{
    int contadorButacasFueraServicio = 0;
    for (int i = 1; i < valores.GetLength(0); i++) // Empezamos en 1 para saltar la cabecera
    {
        for (int j = 0; j < valores.GetLength(1); j++)
        {
            valores[i, j] = 0;
            if (rnd.NextDouble() < 0.2 && contadorButacasFueraServicio <= 3)
            {
                valores[i, j] = 2; //Butaca fuera de servicio
                contadorButacasFueraServicio++;
            }
        }
    }
}

void MostrarTabla(int[,] valores)
{
    Clear();
    for (int i = 0; i < valores.GetLength(0); i++)
    {
        Write($"{letras[i], 2} ");
        for (int j = 0; j < valores.GetLength(1); j++)
        {
            if (i == 0)
            {
                Write($" {numeros[j], 2}  ");
            }
            else if (valores[i, j] == 1)
            {
                Write($"[✅] ");
            }
            else
            {
                if (valores[i, j] == 2)
                {
                    Write($"[❌] ");
                }
                else
                {
                    Write($"[◻️] ");
                }
            }
        }
        WriteLine();
    }
    Menú();
}

void Menú()
{
    bool opciónElegida = false;
    do
    {
        WriteLine("1. Ver Sala");
        WriteLine("2. Comprar ");
        WriteLine("3. Devolver ");
        WriteLine("4. Recaudación ");
        WriteLine("5. Informe ");
        WriteLine("6. Salir ");
        WriteLine("Opción: ");

        int? opción = int.TryParse(ReadLine(), out int i) ? i : null;

        if (opción is null)
        {
            WriteLine("Introduce un valor válido");
        }
        else if (opción < 1 || opción > 6)
        {
            WriteLine("Introduce un valor entre 1-6");
        }
        else
        {
            opciónElegida = true;
            switch (opción)
            {
                case 1:
                    MostrarTabla(original);
                    break;
                case 2:
                    Comprar();
                    break;
                case 3:
                    Devolver();
                    break;
                case 4:
                    Recaudación();
                    break;
                case 5:
                    Informe();
                    break;
                case 6:
                    Salir();
                    break;
                default:
                    Salir();
                    break;
            }
        }

    } while (!opciónElegida);
}


void Comprar()
{
    SolicitarCoordenadas(original, 1);
}

void Devolver()
{
    SolicitarCoordenadas(original, 2);
}

void SolicitarCoordenadas(int[,] valores, int acción)
{
    Clear();
    bool inputVálido = false;
    int primerNum = 0;
    int segundoNum = 0;
    do
    {
        try
        {
            Write($"Introduce las coordenadas (F:C) o ENTER para volver al menú: ");
            string? input = ReadLine()?.Trim() ?? "";

            if (string.IsNullOrEmpty(input))
            {
                Menú(); 
            }

            var inputConfiguración = input.Split(':');
            if (inputConfiguración.Length != 2 || !int.TryParse(inputConfiguración[0], out primerNum) ||
                !int.TryParse(inputConfiguración[1], out segundoNum))
            {
                throw new FormatException();
            }

            primerNum = int.TryParse(inputConfiguración[0], out int n1) ? n1 : 0;
            segundoNum = int.TryParse(inputConfiguración[1], out int n2) ? n2 - 1 : 0;
            if ((primerNum < 1 || primerNum > tamañoFilas) || (segundoNum < 0 || segundoNum >= tamañoColumnas))
            {
                throw new ArgumentOutOfRangeException();
            }
            else
            {
                WriteLine("Las coordenadas son válidas");
                inputVálido = true;
            }
        }
        catch (FormatException)
        {
            WriteLine("ERROR: Formato NO VÁLIDO. Usa Fila:Columna (ej. 2:4).\n");
        }
        catch (ArgumentOutOfRangeException)
        {
            WriteLine($"ERROR: Coordenadas fuera de rango. Filas [1-{tamañoFilas}], Columnas [1-{tamañoColumnas}].\n");
        }
        
    } while (!inputVálido);
    
    Verificador(primerNum, segundoNum, acción);
}

void Verificador(int n1, int n2, int acción)
{
    if (acción == 1) //Compramos
    {
        if (original[n1, n2] == 0) //Butaca libre
        {
            original[n1, n2] = 1; //Ocupamos la butaca
            contadorCompras++;
            Menú();
        }
        else if (original[n1, n2] == 1)
        {
            WriteLine("No se puede comprar ya que la butaca ha sido comprada");
            Comprar();
        }
        else 
        {
            WriteLine("La butaca está fuera de Servicio, por favor, eliga otra butaca");
            Comprar();
        }
    }

    if (acción == 2) //Devolver
    {
        if (original[n1, n2] == 1)
        {
            original[n1, n2] = 0; //Liberamos la butaca
            contadorCompras--;
            Menú();
        }
        else if (original[n1, n2] == 0)
        {
            WriteLine("No se puede devolver ya que no hay nadie en esa butaca");
            Devolver();
        }
        else 
        {
            WriteLine("La butaca está fuera de Servicio, por favor, eliga otra butaca");
            Devolver();
        }
    }
}

void Recaudación()
{
    double sumaTotal = contadorCompras * PrecioButaca;
    Clear();
    WriteLine($"El total recaudado hasta ahora es de {sumaTotal:F2} €");
    WriteLine();
    Menú();
}

void Informe()
{
    Clear();
    int contadorLibres = 0;
    int contadorFueraServicio = 0;
    for (int i = 1; i < original.GetLength(0); i++)
    {
        for (int j = 0; j < original.GetLength(1); j++) // Desde 0 para incluir la columna 1
        {
            if (original[i, j] == 0)
            {
                contadorLibres++;
            }

            if (original[i, j] == 2)
            {
                contadorFueraServicio++;
            }
        }
    }
    WriteLine($"Butacas Ocupadas: {contadorCompras}");
    WriteLine($"Butacas Libres : {contadorLibres}");
    WriteLine($"Butacas Fuera Servicio : {contadorFueraServicio}");
    WriteLine($"Total Recaudado: {contadorCompras * PrecioButaca:F2} €");
    WriteLine($"Porcentaje Ocupación : {((double)contadorCompras / (contadorCompras + contadorLibres)) * 100:F2} %");
    Menú();
}

void Salir()
{
    Clear();
    WriteLine("=====Has salido Correctamente del sistema de CineLuisVives=====");
    WriteLine("                            Hasta Pronto!"); 
}