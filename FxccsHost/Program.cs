using SharpDX.D3DCompiler;

if (args.Length < 6)
{
    Console.Error.WriteLine("Usage: fxccs <src> <function> <profile> <flags> <displayPath> <dest>");
    return 1;
}

var sourcePath = args[0];
var shaderFunction = args[1];
var shaderProfile = args[2];
var shaderFlagsValue = int.Parse(args[3], System.Globalization.CultureInfo.InvariantCulture);
var displayPath = args[4];
var destinationPath = args[5];

if (!File.Exists(sourcePath))
{
    Console.Error.WriteLine($"Source file '{sourcePath}' not found.");
    return 2;
}

var sourceText = File.ReadAllText(sourcePath);
var shaderFlags = (ShaderFlags)shaderFlagsValue;
CompilationResult? result = null;
try
{
result = ShaderBytecode.Compile(sourceText, shaderFunction, shaderProfile, shaderFlags, 0, null, null, displayPath);
    if (!string.IsNullOrEmpty(result.Message))
    {
        Console.WriteLine(result.Message);
    }

    if (result.Bytecode == null)
    {
        Console.Error.WriteLine("FXC returned no bytecode.");
        return 3;
    }

    using var output = File.Create(destinationPath);
    output.Write(result.Bytecode.Data, 0, result.Bytecode.Data.Length);
}
catch (Exception ex)
{
    Console.Error.WriteLine(ex.Message);
    return 4;
}
finally
{
    result?.Dispose();
}

return 0;
