using System.Diagnostics;
using System.Drawing;

var ASCII = " `.-':_,^=;><+!rc*/z?sLTv)J7(|Fi{C}fI31tlu[neoZ5Yxjya]2ESwqkP6h9d4VpOGbUAKXHm8RD#$Bg0MNWQ%&@".ToCharArray();
var TRESHOLDS = new[]
{
    0.0751, 0.0829, 0.0848, 0.1227, 0.1403, 0.1559, 0.185, 0.2183, 0.2417, 0.2571, 0.2852, 0.2902, 0.2919, 0.3099, 0.3192, 0.3232, 0.3294, 0.3384, 0.3609, 0.3619, 0.3667,
    0.3737, 0.3747, 0.3838, 0.3921, 0.396, 0.3984, 0.3993, 0.4075, 0.4091, 0.4101, 0.42, 0.423, 0.4247, 0.4274, 0.4293, 0.4328, 0.4382, 0.4385, 0.442, 0.4473, 0.4477, 0.4503,
    0.4562, 0.458, 0.461, 0.4638, 0.4667, 0.4686, 0.4693, 0.4703, 0.4833, 0.4881, 0.4944, 0.4953, 0.4992, 0.5509, 0.5567, 0.5569, 0.5591, 0.5602, 0.5602, 0.565, 0.5776, 0.5777,
    0.5818, 0.587, 0.5972, 0.5999, 0.6043, 0.6049, 0.6093, 0.6099, 0.6465, 0.6561, 0.6595, 0.6631, 0.6714, 0.6759, 0.6809, 0.6816, 0.6925, 0.7039, 0.7086, 0.7235, 0.7302, 0.7332,
    0.7602, 0.7834, 0.8037, 1
}.ToList();

#pragma warning disable CA1416

var min = 0f;
var max = 1f;
var file = args[0];

var bitmap = new Bitmap(file);
var height = bitmap.Height;
var width = bitmap.Width;

var ratio = bitmap.Width / 200;
var brightnessList = new List<float>();

var consoleMem = new List<List<char>>();

var sw = Stopwatch.StartNew();
for (int y = 0; y < height; y += ratio)
    DrawRow(y);

min = brightnessList.Min();
max = brightnessList.Max();

Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
Console.ReadKey();

while (true)
{
    Console.Clear();
    consoleMem.Clear();
    
    sw = Stopwatch.StartNew();
    for (int y = 0; y < height; y += ratio)
        DrawRow(y);

    Console.WriteLine($"{sw.ElapsedMilliseconds} ms");
    Console.ReadKey();
}

void DrawRow(int y)
{
    var memoryRow = new List<char>();
    consoleMem.Add(memoryRow);
    
    for (int x = 0; x < width; x += ratio)
    {
        // var brightness = GetBrightness2(x, y, aRatio) - min / max - min;
        var brightness = GetBrightness(x, y) - min / max - min;
        var character = GetCharacter(brightness);
        
        Console.Write(character);
        Console.Write(character);
        Console.Write(character);
        
        memoryRow.Add(character);
        memoryRow.Add(character);
        memoryRow.Add(character);

        brightnessList.Add(brightness);
    }

    Console.WriteLine();
}

float GetBrightness(int x, int y)
{
    return 1 - bitmap.GetPixel(x, y).GetBrightness();
}

float GetBrightness2(int pointX, int pointY, int sampleSize)
{
    var sum = 0f;
    var count = 0;
    for (int y = pointY - sampleSize / 2; y < pointY + sampleSize / 2; y++)
    {
        if (y < 0)
            continue;

        if (y >= height)
            break;

        for (int x = pointX - sampleSize / 2; x < pointX + sampleSize / 2; x++)
        {
            if (x < 0)
                continue;

            if (x >= width)
                break;

            sum += GetBrightness(x, y);
            count++;
        }
    }

    return sum / count;
}

char GetCharacter(float brightness)
{
    var idx = TRESHOLDS.FindIndex(t => t >= brightness);
    return ASCII[idx];
}

#pragma warning restore CA1416