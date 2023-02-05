using System.Linq;
using System.Text.RegularExpressions;


class Parser
{
    string[] equations;
    public Parser(string[] data)
    {
        this.equations = data;
    }
    /*  / 1x + 2y + 3z = 1
     *  | 1x + 2y + 3z = 2
     *  \ 1x + 2y + 3z = 3
     * 
     * {
     *   {
     *    {1, 2, 3}
     *    {1, 2, 3}
     *    {1, 2, 3}
     *   }
     *    
     *   {1, 2, 3}
     * }
    */

    /*
    public coeffData Parse()
    {
        GetVars();
        
        float[][] c = new float[vars.Length][];
        for (int i = 0; i < vars.Length; i++)
            c[i] = new float[vars.Length];

        coeffData data = new coeffData()
        {
            coeffs = c,
            free_coeffs = new float[vars.Length],
            vars = vars
        };

        for (int i = 0;  i < vars.Length; i++)
        {
            for (int j = 0; j < vars.Length; j++)
            {
                Regex CoeffFinder = new Regex($@"-? ?\d*{vars[j]}|-? ?\d*[.,]?\d+{vars[j]}");
                string tmp = CoeffFinder.Matches(txt[i])[0].Value;

                tmp = tmp.Replace(" ", "").Replace(vars[j].ToString(), "").Replace(".", ","); // "1" "-1" "" "-"
                if (tmp == "" || tmp == "-") tmp += "1"; // "" || "-" -> += 1
                float coeff = System.Convert.ToSingle(tmp); 
                data.coeffs[i][j] = coeff;
            }

            Regex free_CoeffFinder = new Regex(@"-? ?\d+$|-? ?\d*[.,]?\d+$");
            string tmp2 = free_CoeffFinder.Matches(txt[i])[0].Value;

            tmp2 = tmp2.Replace(" ", "").Replace("=", "").Replace(".", ","); // "1" "-1"
            float free_coeff = System.Convert.ToSingle(tmp2); // "" || "-" -> += 1
            data.free_coeffs[i] = free_coeff;
        }

        return data;
        // /-? ?\d?x/g
        // /-?\d$/g
    }

    void GetVars()
    {
        Regex varsFinder = new Regex("[a-z]");
        var collection = varsFinder.Matches(txt[0]);
        foreach (Match item in collection)
            vars += item.Value;
    }
    */



    /*
    Идея функции в том, чтобы создать матрицу коэффициентов, где в последнем столбце будут свободные коэффициенты, а в остальных коэффициенты при переменных
    Типа так:

    vars = "yzx"
        y z x free
    eq1 1 0 5  10
    eq2 3 8 5  -7
    eq3 1 2 6  16

    И это все для удобства в одной матрице
    */
    public coeffData Parse()
    {
        // И снова мои шикарные комментарии

        // Достаем переменные без повторений
        string vars = "";
        Regex varEx = new Regex(@"[a-z]");
        foreach (string equ in equations)
        {
            foreach (Match match in varEx.Matches(equ))
            {
                if (!vars.Contains(match.Value))
                    vars += match.Value;
            }
        }

        vars = new string(vars.OrderBy(c => c).ToArray());

        // Заполняем матрицу готовых коэффициентов нулями
        float[][] parsedEquations = new float[equations.Length][];

        for (int i = 0; i < equations.Length; i++)
        {
            parsedEquations[i] = new float[vars.Length + 1];

            for (int j = 0; j < vars.Length + 1; j++)
                parsedEquations[i][j] = 0;
        }

        // Идем циклом по уравнениям, k нужно, чтобы соотносить данные уравнения и уравнения из матрицы коэффициентов
        int k = 0;
        foreach (string eq in equations)
        {
            // Пилим уравнение пополам, удаляем пробелы, меняем точки на запятые
            // Так же надо удостовериться, что перед каждой переменной стоит знак, иначе регулярное выражение не найдет просто х или просто у
            int eqInd = eq.IndexOf("=");
            string left = eq.Substring(0, eqInd), right = eq.Substring(eqInd + 1);
            left = left.Replace(" ", "").Replace(".", ",");
            if (left[0] != '-') left = "+" + left;
            right = right.Replace(" ", "").Replace(".", ",");
            if (right[0] != '-') right = "+" + right;

            // Для каждой переменной ищем коэффициенты, i нужно чтобы опять же в матрицу коэффициентов записать все четко и по порядку
            int i = 0;
            foreach (char var in vars)
            {
                Regex varCoefEx = new Regex(@$"[-+](\d+,?\d*)?{var}");
                foreach (Match match in varCoefEx.Matches(left))
                {
                    // Убираем найденный коэффициент вместе с переменной, это нужно, чтобы потом на изи найти свободные
                    // Ну и обрабатываем коэффициент на всякие частные случаи, прежде чем прибавлять его к соответствующему элементу матрицы (добавляем потому что левая часть)
                    string coef = match.Value;
                    left = left.Replace(coef, "");
                    coef = coef.Replace(var.ToString(), "");
                    if (coef == "+") coef = "1";
                    if (coef == "-") coef = "-1";
                    if (coef[0] == '+') coef = coef[1..];
                    parsedEquations[k][i] += float.Parse(coef);
                }
                foreach (Match match in varCoefEx.Matches(right))
                {
                    // Все то же самое только отнимаем, так как это уже правая часть
                    string coef = match.Value;
                    right = right.Replace(coef, "");
                    coef = coef.Replace(var.ToString(), "");
                    if (coef == "+") coef = "1";
                    if (coef == "-") coef = "-1";
                    if (coef[0] == '+') coef = coef[1..];
                    parsedEquations[k][i] -= float.Parse(coef);
                }

                i++;
            }

            // Ищем свободный коэффициент
            Regex freeCoefEx = new Regex(@"[-+]\d+,?\d*");
            foreach (Match match in freeCoefEx.Matches(left))
            {
                string coef = match.Value;
                if (coef[0] == '+') coef = coef[1..];
                parsedEquations[k][vars.Length] -= float.Parse(coef);
            }
            foreach (Match match in freeCoefEx.Matches(right))
            {
                string coef = match.Value;
                if (coef[0] == '+') coef = coef[1..];
                parsedEquations[k][vars.Length] += float.Parse(coef);
            }

            k++;
        }
        coeffData data = new coeffData()
        {
            matrix = parsedEquations,
            vars = vars
        };
        return data;
    }
}

public struct coeffData
{
    public float[][] matrix;
    public string vars;
}