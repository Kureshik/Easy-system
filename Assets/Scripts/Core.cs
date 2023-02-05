using System.Linq;
using TMPro;
using UnityEngine;

public class Core : MonoBehaviour
{
    public TextMeshProUGUI solutionText;
    public TextMeshProUGUI answerText;
    public TMP_InputField EquationsText;

    [SerializeField] DragHandle dragHandle;

    string Vars = "";

    public void StartSolving()
    {
        dragHandle.moveToSolution();
        answerText.text = "Здесь будет ответ...";
        solutionText.text = "Здесь будет решение...";

        string[] temp = EquationsText.text.Split('\n'); //массив строчек с уравнениями
        //System.Array.Reverse(temp);
        Parser parser = new Parser(temp);

        coeffData data = parser.Parse();
        float[][] matrix = data.matrix;
        Vars = data.vars;

        solutionText.text = "";
        DocPrintLine("<b>Исходная система:</b>");
        Print(matrix, null);
        DocPrintLine("<b>Решаем:</b>");

        string[] answers = GaussSolve(matrix) ?? System.Array.Empty<string>();

        answerText.text = "";
        foreach (var item in answers)
            answerText.text += $"{item}\n";

        DocPrintLine();
        DocPrintLine();
        DocPrintLine();
    }

    void Print(float[][] matrix, int[] selected)
    {
        DocPrintLine();
        for (int row = 0; row < matrix.Length; row++)
        {
            DocPrint("(");
            for (int col = 0; col < matrix[0].Length - 1; col++)
            {
                DocPrint(string.Format("{0,7:f2}", matrix[row][col]));
                if (selected == null || selected[0] != row || selected[1] != col)
                    DocPrint("  ");
                else
                    DocPrint("<");
            }
            DocPrintLine(string.Format(" |{0,7:f2})", matrix[row][matrix[0].Length - 1]));
        }
        DocPrintLine();
    }

    void DocPrint(string txt)
    {
        solutionText.text += txt;
    }

    void DocPrintLine(string txt = "")
    {
        solutionText.text += $"{txt}\n";
    }


    // --- перемена местами двух строк системы
    void SwapRows(float[][] matrix, int row1, int row2)
    {
        (matrix[row2], matrix[row1]) = (matrix[row1], matrix[row2]);
    }

    // --- деление строки системы на число
    void DivideRow(float[][] matrix, int row, float divider)
    {
        for (int i = 0; i < matrix[0].Length; i++)
            matrix[row][i] /= divider;
    }

    // --- сложение строки системы с другой строкой, умноженной на число
    void CombineRows(float[][] matrix, int row, int source_row, float weight)
    {
        for (int i = 0; i < matrix[0].Length; i++)
            matrix[row][i] += matrix[source_row][i] * weight;
    }

    // --- решение системы методом Гаусса (приведением к треугольному виду)
    string[] GaussSolve(float[][] matrix)
    {
        int column = 0;
        while (column < matrix.Length)
        {
            DocPrintLine($"Ищем максимальный по модулю элемент в {column + 1}-м столбце:");
            int current_row = -1;
            for (int r = column; r < matrix.Length; r++)
            {
                if (current_row == -1 || Mathf.Abs(matrix[r][column]) > Mathf.Abs(matrix[current_row][column]))
                    current_row = r;
            }
            if (current_row == -1)
            {
                DocPrintLine("решений нет");
                return null;
            }
            Print(matrix, new int[] { current_row, column });

            if (current_row != column)
            {
                DocPrintLine("Переставляем строку с найденным элементом повыше:");
                SwapRows(matrix, current_row, column);
                Print(matrix, new int[] { column, column });
            }

            DocPrintLine("Нормализуем строку с найденным элементом:");
            DivideRow(matrix, column, matrix[column][column]);
            Print(matrix, new int[] { column, column });

            DocPrintLine("Обрабатываем нижележащие строки:");
            for (int r = column + 1; r < matrix.Length; r++)
                CombineRows(matrix, r, column, -matrix[r][column]);
            Print(matrix, new int[] { column, column });

            column++;
        }

        DocPrintLine("Матрица приведена к треугольному виду, считаем решение");
        float[] X = new float[matrix.Length];

        for (int i = matrix.Length - 1; i > -1; i--)
        {
            System.Collections.Generic.List<float> T = new();

            var x = X[(i + 1)..];
            var a = matrix[i][(i + 1)..^1];

            for (int k = 0; k < x.Length; k++)
                T.Add(x[k] * a[k]);

            X[i] = matrix[i][matrix[0].Length - 1] - T.ToArray().Sum();
        }

        DocPrintLine();
        string[] answers = new string[Vars.Length];
        for (int i = 0; i < X.Length; i++)
        {
            DocPrint($"{Vars[i]} = "); answers[i] = $"{Vars[i]} = ";
            DocPrintLine(string.Format("{0,8:f2}", X[i])); answers[i] += string.Format("{0,13:f2}", X[i]);
        }
        return answers;
    }
}