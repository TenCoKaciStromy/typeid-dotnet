// See https://aka.ms/new-console-template for more information

using TcKs.TypeId;
using static System.Console;

if (args.Length < 1)
  return PrintHelp();

return args[0] switch {
  "new" => PrintNew(args),
  "decode" => PrintDecode(args),
  "encode" => PrintEncode(args),
  "help" or "-help" or "--help" or "/help"
    or "?" or "-?" or "--?" or "/?"
    or "h" or "-h"
    => PrintHelp(),
_ => PrintHelpForNotSupportedCommand(args)
};

static int PrintNew(string[] args)
  => args.Length switch {
    1 => PrintOk(TypeId.NewId(string.Empty)),
    2 => args[1] switch {
      "-h" or "--help" => PrintNewHelp(),
      _ => PrintOk(TypeId.NewId(args[1]))
    },
    _ => PrintHelpForInvalidCommandParametersCount(args)
  };

static int PrintNewHelp()
  => PrintOkLines(
    "Generate a new TypeID using the given type prefix",
    "",
    "Usage:",
    "typeid new [<type_prefix>] [flags]",
    "",
    "Flags:",
    "-h, --help   help for new"
  );

static int PrintDecode(string[] args)
  => args.Length != 2
    ? PrintHelpForInvalidCommandParametersCount(args)
    : TypeId.TryParse(args[1], out var typeId)
      ? PrintOk($"type: {typeId.Type}\nuuid: {typeId.Id}")
      : PrintError("Decode failed. Can not parse input.");

static int PrintEncode(string[] args)
  => args.Length != 3
    ? PrintHelpForInvalidCommandParametersCount(args)
    : Guid.TryParse(args[2], out var uuid)
      ? PrintOk(new TypeId(args[1], uuid))
      : PrintError("Encode failed. Can not parse input uuid.");

// >>> Shared print-help methods >>>

static int PrintHelpForInvalidCommandParametersCount(string[] args)
  => PrintError($"Command ({args[0]}) has invalid count of parameters.");

static int PrintHelpForNotSupportedCommand(string[] args)
  => PrintError($"Command ({args[0]}) is not supported.");

static int PrintHelp()
  => PrintOkLines(
    "Type-safe, K-sortable, globally unique identifiers",
    "",
    "Usage:",
    "  typeid [flags]",
    "  typeid [command]",
    "",
    "Available Commands:",
    "  completion  Generate the autocompletion script for the specified shell",
    "  decode      Decode the given TypeID into a UUID",
    "  encode      Encode the given UUID into a TypeID using the given type prefix",
    "  help        Help about any command",
    "  new         Generate a new TypeID using the given type prefix",
    "",
    "Flags:",
    "  -h, --help   help for typeid",
    "",
    "  Use \"typeid [command] --help\" for more information about a command."
  );

// >>> Generic helper methods >>>

static int PrintOk(object valueToPrint)
  => Print(0, valueToPrint);

static int PrintOkLines(params object[] lines) {
  if (lines is null)
    return 0;

  foreach (var line in lines) {
    WriteLine(line);
  }

  return 0;
}

static int PrintError(object errorToPrint)
  => Print(-1, errorToPrint);
  
static int Print(int returnCode, object valueToPrint) {
  WriteLine(valueToPrint);
  return returnCode;
}