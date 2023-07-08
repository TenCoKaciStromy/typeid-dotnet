return args.Length switch {
  0 => PrintHelp(),
  _ => args[0] switch {
    "new" => PrintNew(args),
    "decode" => PrintDecode(args),
    "encode" => PrintEncode(args),
    "help" or "-help" or "--help" or "/help"
      or "?" or "-?" or "--?" or "/?"
      or "h" or "-h"
      => PrintHelp(),
    _ => PrintHelpForNotSupportedCommand(args)
  }
};

#region Command New
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
#endregion Command New

#region Command Decode
static int PrintDecode(string[] args)
  => args.Length switch {
    2 => args[1] switch {
      "-h" or "--help" => PrintDecodeHelp(),
      _ => TypeId.TryParse(args[1], out var typeId)
        ? PrintOk($"type: {typeId.Type}\nuuid: {typeId.Id}")
        : PrintError("Decode failed. Can not parse input.")
    },
    _ => PrintHelpForInvalidCommandParametersCount(args)
  };

static int PrintDecodeHelp()
  => PrintOkLines(
    "Decode the given TypeID into a UUID",
    "",
    "Usage:",
    "  typeid decode <type_id> [flags]",
    "",
    "Flags:",
    "  -h, --help   help for decode"
  );
#endregion Command Decode

#region Command Encode
static int PrintEncode(string[] args)
  => args.Length switch {
    3 => EncodeAndPrint(args[1], args[2]),
    2 => args[1] switch {
      "-h" or "--help" => PrintEncodeHelp(),
      _ => EncodeAndPrint(string.Empty, args[1])
    },
    _ => PrintHelpForInvalidCommandParametersCount(args)
  };

static int EncodeAndPrint(string prefix, string suffix)
  => Guid.TryParse(suffix, out var uuid)
    ? PrintOk(new TypeId(prefix, uuid))
    : PrintError("Encode failed. Can not parse input uuid.");

static int PrintEncodeHelp()
  => PrintOkLines(
    "Encode the given UUID into a TypeID using the given type prefix",
    "",
    "Usage:",
    "  typeid encode [<type_prefix>] <uuid> [flags]",
    "",
    "Flags:",
    "  -h, --help   help for encode"
  );
#endregion Command Encode

#region Shared print-help methods
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
    // Command "completion" is present in original implementation in GO, and is
    // provided by Cobra project (https://github.com/spf13/cobra) which is not
    // available for .NET environment. Therefore this command is not available.
    // "  completion  Generate the autocompletion script for the specified shell",
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
#endregion Shared print-help methods

#region Generic helper methods
static int PrintOk(object valueToPrint)
  => Print(0, valueToPrint);

static int PrintOkLines(params object[] lines) {
  if (lines is null)
    return 0;

  foreach (var line in lines) {
    Console.WriteLine(line);
  }

  return 0;
}

static int PrintError(object errorToPrint)
  => Print(-1, errorToPrint);
  
static int Print(int returnCode, object valueToPrint) {
  Console.WriteLine(valueToPrint);
  return returnCode;
}
#endregion Generic helper methods
